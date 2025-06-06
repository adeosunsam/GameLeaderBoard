using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Domain.Entity.MovieMania;
using Infrastructure.DTOs;
using Infrastructure.Utility;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using static Infrastructure.DTOs.MovieManiaDtos;

namespace MovieManiaSignalr
{
    public partial class MovieManiaService
    {
        public async Task<Result<ICollection<UserDetailResponseDto>>> FetchUserFriends(string userId)
        {
            var friends = await (from f in _context.UserFriends
                                 where f.UserId == userId
                                 && !f.IsDeleted
                                 join user in _context.AppUsers on f.FriendId equals user.UserId
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

            _logger.LogInformation($"Friends count is {friends.Count}");

            return Result<ICollection<UserDetailResponseDto>>.Success("friends retrieved successfully", data: friends);
        }

        public async Task<Result<bool>> RequestToFollowUser(UserFollowRequest request)
        {
            var userActivity = await (from activity in _context.UserActivities
                                      where activity.SenderId == request.UserId
                                      && activity.UserId == request.FriendId
                                      && activity.ActivityAction == ActivityEnum.Follow
                                      join f in _context.UserFriends.Where(x => x.UserId == request.UserId && !x.IsDeleted) on activity.UserId equals f.FriendId into friends
                                      from f in friends.DefaultIfEmpty()
                                      select new
                                      {
                                          UserFriend = f,
                                          Activity = activity
                                      }).FirstOrDefaultAsync();

            if (userActivity != null && userActivity.UserFriend != null)
            {
                return Result<bool>.Success($"{userActivity.UserFriend.FriendId} is already a friend.", data: true);
            }

            if (userActivity != null && userActivity.Activity != null && !userActivity.Activity.IsDeleted)
            {
                return Result<bool>.Fail("friend request already sent", "400");
            }

            //add new user activities record

            var userActivityRecord = new UserActivity
            {
                UserId = request.FriendId,
                SenderId = request.UserId,
                ActivityAction = ActivityEnum.Follow
            };

            await _context.UserActivities.AddAsync(userActivityRecord);

            await _context.SaveChangesAsync();

            return Result<bool>.Success("request sent successfully", data: true);
        }

        public async Task<Result<bool>> ManageFriendRequest(ManageFriendRequest request)
        {
            var userActivity = await (from activity in _context.UserActivities
                                      where activity.UserId == request.UserId
                                      && activity.SenderId == request.FriendId
                                      && activity.ActivityAction == ActivityEnum.Follow
                                      && !activity.IsDeleted
                                      join f in _context.UserFriends.Where(x => x.UserId == request.UserId && !x.IsDeleted) on activity.SenderId equals f.FriendId into friends
                                      from f in friends.DefaultIfEmpty()
                                      select new
                                      {
                                          UserFriend = f,
                                          Activity = activity
                                      }).FirstOrDefaultAsync();

            if (userActivity == null)
            {
                return Result<bool>.Fail($"Invalid activity request");
            }

            if(userActivity.Activity != null && request.Action == ManageFriend.Decline)
            {
                userActivity.Activity.IsDeleted = true;
                await _context.SaveChangesAsync();

                return Result<bool>.Success("request declined successfully", data: true);
            }

            if (userActivity.Activity != null && userActivity.UserFriend != null)
            {
                //user has been previously accepted.
                userActivity.Activity.IsDeleted = true;
                await _context.SaveChangesAsync();

                return Result<bool>.Fail("request has been previously accepted.");
            }

            await _context.UserFriends.AddRangeAsync(new List<UserFriend>
            {
                new()
                {
                    UserId = request.UserId,
                    FriendId = request.FriendId
                },
                new()
                {
                    UserId = request.FriendId,
                    FriendId = request.UserId
                }
            });

            var data = await _context.UserActivities.Where(x => (x.UserId == request.UserId && x.SenderId == request.FriendId) 
            || (x.UserId == request.FriendId && x.SenderId == request.UserId)).ToListAsync();

            if (data != null && data.Any())
            {
                foreach (var activity in data)
                {
                    activity.IsDeleted = true;
                }
            }

            await _context.SaveChangesAsync();

            return Result<bool>.Success("request accepted successfully", data: true);
        }

        public async Task<Result<UserGamingCountDto>> FetchUserGamingCount(string userId)
        {
            var gameCount = await (from user in _context.AppUsers
                                   where user.UserId == userId
                                   && !user.IsDeleted
                                   join f in _context.UserGamingNumbers.Where(x => !x.IsDeleted) on userId equals f.UserId into gameNumber
                                   from f in gameNumber.DefaultIfEmpty()
                                   join friend in _context.UserFriends on user.UserId equals friend.UserId into friends
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

        public async Task<Result<UserDetailDto>> GetUserById(string userId)
        {
            var userDetail = await (from user in _context.AppUsers
                                    where user.UserId == userId
                                    && !user.IsDeleted
                                    select new UserDetailDto
                                    {
                                        UserId = user.UserId,
                                        FirstName = user.FirstName,
                                        LastName = user.LastName,
                                        Email = user.Email,
                                        UserName = user.UserName,
                                        Image = user.Image
                                    }).FirstOrDefaultAsync() ?? new UserDetailDto();

            return Result<UserDetailDto>.Success("user retrieved successfully", data: userDetail);
        }

        public async Task<Result<List<UserDetailDto>>> Search(string userId,string input)
        {
            var userDetails = await (from user in _context.AppUsers
                                    where user.UserId != userId 
                                    && !user.IsDeleted
                                    select new UserDetailDto
                                    {
                                        UserId = user.UserId,
                                        FirstName = user.FirstName,
                                        LastName = user.LastName,
                                        Email = user.Email,
                                        UserName = user.UserName,
                                        Image = user.Image
                                    }).ToListAsync() ?? new List<UserDetailDto>();

            userDetails.RemoveAll(user => StringHelper.SimilarityPercentage(input, $"{user.FirstName} {user.LastName}") <= 15d);

            return Result<List<UserDetailDto>>.Success("users retrieved successfully", data: userDetails);
        }

        public async Task Login(UserDetailDto request)
        {
            try
            {
                var user = await _context.AppUsers.FirstOrDefaultAsync(x => x.UserId == request.UserId);

                string image = string.Empty;
                try
                {
                    var imageBytes = await _httpClient.GetByteArrayAsync(request.Image);

                    image = Convert.ToBase64String(imageBytes);
                }
                catch (Exception ex)
                {
                    _logger.LogError($"====================={ex.InnerException}=====================");
                    _logger.LogError($"====================={ex.InnerException?.Message}=====================");
                    _logger.LogError($"====================={ex.Message}=====================");
                }

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
            }
            catch (Exception ex)
            {
                _logger.LogError($"====================={ex.InnerException}=====================");
                _logger.LogError($"====================={ex.InnerException?.Message}=====================");
                _logger.LogError($"====================={ex.Message}=====================");
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

            var keys = GetEnvironmentVariable();

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(keys.SecretKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: keys.Issuer,
                audience: keys.Audience,
                claims: claims,
                expires: DateTime.Now.AddDays(30),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public TokenValidation GetEnvironmentVariable()
        {
            if (!_env.IsDevelopment())
            {
                return new TokenValidation
                {
                    Audience = Environment.GetEnvironmentVariable("ValidAudience"),
                    Issuer = Environment.GetEnvironmentVariable("ValidIssuer"),
                    SecretKey = Environment.GetEnvironmentVariable("SecretKey")
                };
            }
            return new TokenValidation
            {
                Audience = _configuration["JwtSettings:ValidAudience"],
                Issuer = _configuration["JwtSettings:ValidIssuer"],
                SecretKey = _configuration["JwtSettings:SecretKey"]
            };
        }

    }
}
