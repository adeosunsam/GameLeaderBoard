using Domain.Entity.MovieMania;
using Infrastructure.Utility;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
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
            var friends = _cache.GetDataById<ICollection<UserDetailResponseDto>>("friendList", userId);

            if (friends != null)
            {
                return Result<ICollection<UserDetailResponseDto>>.Success("Successfully retrieved all friends in user list", data: friends);
            }

            friends = await (from f in _context.UserFriends
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

            if (friends.Any())
            {
                _cache.CreateData("friendList", userId, friends);
            }

            return Result<ICollection<UserDetailResponseDto>>.Success("friends retrieved successfully", data: friends);
        }

        public async Task<Result<UserGamingCountDto>> FetchUserGamingCount(string userId)
        {
            var gameCount = _cache.GetDataById<UserGamingCountDto>("gamingCount", userId);

            if (gameCount != null)
            {
                return Result<UserGamingCountDto>.Success("Successfully retrieved game count", data: gameCount);
            }

            gameCount = await (from f in _context.UserGamingNumbers
                             where f.AppUserId == userId
                             && !f.IsDeleted
                             select new UserGamingCountDto
                             {
                                 Id = f.Id,
                                 TotalGamePlayed = f.TotalGamePlayed
                             }).FirstOrDefaultAsync() ?? new UserGamingCountDto { Id = userId };

            if (gameCount != null)
            {
                _cache.CreateData("gamingCount", userId, gameCount);
            }

            return Result<UserGamingCountDto>.Success("game count retrieved successfully", data: gameCount);
        }

        public async Task Login(UserDetailRequestDto request)
        {
            try
            {
                var user = _cache.GetDataById<AppUser>("users", request.UserId);

                if (user != null)
                {
                    return;
                }

                var imageBytes = await _httpClient.GetByteArrayAsync(request.Image);

                var image = Convert.ToBase64String(imageBytes);

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

                await _context.SaveChangesAsync();

                _cache.CreateData("users", request.UserId, appUser);

                return;
            }
            catch (Exception ex)
            {
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
