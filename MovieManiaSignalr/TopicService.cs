using Infrastructure.Utility;
using Microsoft.EntityFrameworkCore;
using static Infrastructure.DTOs.MovieManiaDtos;

namespace MovieManiaSignalr
{
    public partial class MovieManiaService
    {
        public async Task<Result<ICollection<TopicResponseDto>>> FetchAvailableTopics(string userId)
        {
            var topics = _cache.GetDataByKey<ICollection<TopicResponseDto>>("allTopic")?.SelectMany(x => x)?.ToList();

            if (topics != null)
            {
                return Result<ICollection<TopicResponseDto>>.Success("Successfully retrieved all topics",data: topics);
            }

            topics = await (from t in _context.Topics
                            where !t.IsDeleted
                            join f in _context.FollowedTopics.Where(x => x.UserId == userId && !x.IsDeleted)
                            on t.Id equals f.TopicId into followedTopics
                            from f in followedTopics.DefaultIfEmpty()
                            select new TopicResponseDto
                            {
                                Id = t.Id,
                                Name = t.Name,
                                Description = t.Description,
                                Category = t.Category.GetDescription(),
                                Image = t.Image,
                                FollowersCount = t.FollowersCount,
                                QuestionCount = t.FollowersCount,
                                IsFollowed = f != null
                            }).ToListAsync() ?? new List<TopicResponseDto>();

            if (topics.Any())
            {
                _cache.CreateData("allTopic", string.Empty, topics);
            }

            return Result<ICollection<TopicResponseDto>>.Success("All topics retrieved successfully", data: topics);
        }

        /*public async Task<Result<ICollection<TopicResponseDto>>> FetchUserFollowedTopic(string userId)
        {
            var topics = _cache.GetDataById<ICollection<TopicResponseDto>>("allTopic", userId);

            if (topics != null)
            {
                return Result<ICollection<TopicResponseDto>>.Success("Successfully retrieved all followed topics", data: topics);
            }

            topics = await (from t in _context.Topics
                             where !t.IsDeleted
                             join f in _context.FollowedTopics on t.Id equals f.TopicId
                             where f.AppUserId == userId
                              && !f.IsDeleted
                             select new TopicResponseDto
                             {
                                 Id = t.Id,
                                 Name = t.Name,
                                 Description = t.Description,
                                 Category = t.Category.GetDescription(),
                                 Image = t.Image,
                                 FollowersCount = t.FollowersCount,
                                 QuestionCount = t.FollowersCount
                             }).ToListAsync() ?? new List<TopicResponseDto>();

            if (topics.Any())
            {
                _cache.CreateData("allTopic", userId, topics);
            }

            return Result<ICollection<TopicResponseDto>>.Success("topics retrieved successfully", data: topics);
        }*/
    }
}
