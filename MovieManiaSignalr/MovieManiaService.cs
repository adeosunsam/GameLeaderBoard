using Domain.Entity.MovieMania;
using GameLeaderBoard.Context;
using Infrastructure.Utility;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using static Infrastructure.DTOs.MovieManiaDtos;

namespace MovieManiaSignalr
{
    public partial class MovieManiaService
    {
        //private readonly ICacheDistribution _cache;
        private readonly LeaderBoardContext _context;
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;
        private readonly ILogger<MovieManiaService> _logger;
        private readonly IWebHostEnvironment _env;

        public MovieManiaService(LeaderBoardContext context, HttpClient httpClient,
            IConfiguration configuration, ILogger<MovieManiaService> logger, IWebHostEnvironment env)
        {
            //_cache = cache;
            _context = context;
            _httpClient = httpClient;
            _configuration = configuration;
            _logger = logger;
            _env = env;
        }

        public async Task<Result<bool>> FollowTopic(string userId, string topicId)
        {
            var topicDetail = await (from t in _context.Topics
                        where t.Id == topicId
                        && !t.IsDeleted
                        join u in _context.AppUsers on userId equals u.UserId
                        where !u.IsDeleted
                        join f in _context.FollowedTopics.Where(x => x.UserId == userId) on t.Id equals f.TopicId into followedTopics
                        from f in followedTopics.DefaultIfEmpty()
                        select new
                        {
                            Topic = t,
                            FollowedTopic = f
                        }).ToListAsync();

            if(topicDetail == null || !topicDetail.Any())
            {
                return Result<bool>.Fail("topic not found", "404");
            }

            if(topicDetail.Exists(x => x.FollowedTopic != null))
            {
                return Result<bool>.Success("topic followed already", data: true);
            }
            await _context.FollowedTopics.AddAsync(new FollowedTopic
            {
                UserId = userId,
                TopicId = topicId
            });

            topicDetail.First().Topic.FollowersCount += 1;

            await _context.SaveChangesAsync();

            return Result<bool>.Success("topic followed successfully", data: true);
        }

        public async Task<Result<ICollection<UserActivityResponse>>> FetchUserActivity(string userId)
        {
            var userActivity = await (from activity in _context.UserActivities
                                      where activity.UserId == userId
                                      && !activity.IsDeleted
                                      join user in _context.AppUsers on activity.SenderId equals user.UserId
                                      where !user.IsDeleted
                                      join topic in _context.Topics on activity.TopicId equals topic.Id into t
                                      from topic in t.DefaultIfEmpty()
                                      select new UserActivityResponse
                                      {
                                          Id = activity.Id,
                                          TopicId = topic == null ? null : topic.Id,
                                          SenderName = $"{user.LastName} {user.FirstName}",
                                          UserImage = user.Image,
                                          Activity = activity.ActivityAction,
                                          TopicName = topic == null ? null : topic.Name,
                                          GroupId = activity.GroupId,
                                          SenderId = activity.SenderId,
                                      }).ToListAsync() ?? new List<UserActivityResponse>();

            return Result<ICollection<UserActivityResponse>>.Success("All user activities retrieved successfully", data: userActivity);
        }

        //fetch all challenge for the current player
        public ICollection<UserChallengeData> FetchChallengedData(string userId)
        {
            var response = new List<UserChallengeData>();

            /*var userChallengeDatas = _cache.GetDataByKey<UserChallengeData>("score", x => x.OpponentId == id);

            if (userChallengeDatas != null && userChallengeDatas.Any())
            {
                response = userChallengeDatas.ToList();
            }*/
            return response;
        }

        public void SaveScoreForOfflineOpponent(UserChallengeData request)
        {
            //_cache.UpdateData("score", request.GameId, request);
        }

        public async Task DeleteDataAfterCompetition(List<SaveScoreForLeaderBoardDto> request)
        {
            //remove record from cache
            //_cache.DeleteData("score", request.First().GameId);

            //save to db
            await _context.MovieManiaLeaderBoards.AddRangeAsync(request.Select(x => new MovieManiaLeaderBoard
            {
                PlayerId = x.Id,
                PlayerName = x.PlayerName,
                PlayerScore = x.Score,
                MovieId = x.MovieId
            }).ToList());

            await _context.SaveChangesAsync();
        }


    }
}
