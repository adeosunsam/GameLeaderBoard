﻿using Domain.Entity.MovieMania;
using Infrastructure.Utility;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using static Infrastructure.DTOs.MovieManiaDtos;

namespace MovieManiaSignalr
{
    public partial class MovieManiaService
    {
        public async Task<Result<ICollection<UserDetailResponseDto>>> FetchUserFriends(string userId)
        {
            /*var friends = _cache.GetDataById<ICollection<UserDetailResponseDto>>("friendList", userId);

            if (friends != null)
            {
                return Result<ICollection<UserDetailResponseDto>>.Success("Successfully retrieved all friends in user list", data: friends);
            }*/

            var friends = await (from f in _context.UserFriends
                             where f.AppUserId == userId
                             && !f.IsDeleted
                             join user in _context.AppUsers on f.FriendId equals user.Id
                             where !f.IsDeleted
                             select new UserDetailResponseDto
                             {
                                 Id = user.Id,
                                 UserId = user.UserId,
                                 FirstName = user.FirstName,
                                 LastName = user.LastName,
                                 Email = user.Email,
                                 Image = user.Image,
                                 UserName = user.UserName
                             }).ToListAsync() ?? new List<UserDetailResponseDto>();

            /*if (friends.Any())
            {
                _cache.CreateData("friendList", userId, friends);
            }*/

            return Result<ICollection<UserDetailResponseDto>>.Success("friends retrieved successfully", data: friends);
        }

        public async Task<Result<UserGamingCountDto>> FetchUserGamingCount(string userId)
        {
            /*var gameCount = _cache.GetDataById<UserGamingCountDto>("gamingCount", userId);

            if (gameCount != null)
            {
                return Result<UserGamingCountDto>.Success("Successfully retrieved game count", data: gameCount);
            }*/

            var gameCount = await (from user in _context.AppUsers
                                   where user.UserId == userId
                                   && !user.IsDeleted
                                   join f in _context.UserGamingNumbers.Where(x => !x.IsDeleted) on user.Id equals f.AppUserId into gameNumber
                                   from f in gameNumber.DefaultIfEmpty()
                                   join friend in _context.UserFriends on user.Id equals friend.AppUserId into friends
                                   from friend in friends.DefaultIfEmpty()
                                   group friend by new { user.UserId, TotalGamePlayed = (f == null ? 0 : f.TotalGamePlayed) } into grouped
                                   select new UserGamingCountDto
                                   {
                                       Id = grouped.Key.UserId,
                                       TotalGamePlayed = grouped.Key.TotalGamePlayed,
                                       TotalFriends = grouped.Count(friend => friend != null)
                                   }).FirstOrDefaultAsync() ?? new UserGamingCountDto { Id = userId };

            /*if (gameCount != null)
            {
                _cache.CreateData("gamingCount", userId, gameCount);
            }*/

            return Result<UserGamingCountDto>.Success("game count retrieved successfully", data: gameCount);
        }

        public async Task Login(UserDetailRequestDto request)
        {
            try
            {
                var user = await _context.AppUsers.FirstOrDefaultAsync(x  => x.UserId == request.UserId);

                var imageBytes = await _httpClient.GetByteArrayAsync(request.Image);

                var image = Convert.ToBase64String(imageBytes);

                if (user != null)
                {
                    user.FirstName = request.FirstName;
                    user.LastName = request.LastName;
                    user.Email = request.Email;
                    user.Image = image;
                    user.UserName = request.UserName;
                }
                else
                {
                    var appUser = new AppUser
                    {
                        UserId = request.UserId,
                        FirstName = request.FirstName,
                        LastName = request.LastName,
                        Email = request.Email,
                        Image = image,
                        UserName = request.UserName
                    };

                    _context.AppUsers.Add(appUser);
                }

                await _context.SaveChangesAsync();

                return;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"====================={ex.Message}=====================");
                Console.WriteLine($"====================={ex.InnerException?.Message}=====================");
                return;
            }
            
        }

        public string Login(string userId)
        {
            return GenerateJwtToken(userId);
        }

        private string GenerateJwtToken(string userId)
        {
            var claims = new List<Claim>
            {
                new (ClaimTypes.NameIdentifier, userId)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtSettings:SecretKey"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _configuration["JwtSettings:ValidIssuer"],
                audience: _configuration["JwtSettings:ValidAudience"],
                claims: claims,
                expires: DateTime.Now.AddDays(30),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

    }
}
