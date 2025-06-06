using Infrastructure.Utility;
using Microsoft.EntityFrameworkCore;
using static Infrastructure.DTOs.MovieManiaDtos;

namespace MovieManiaSignalr
{
    public partial class MovieManiaService
    {
        public async Task<Result<ICollection<TopicResponseDto>>> FetchAvailableTopics(string userId)
        {
            var topics = await (from t in _context.Topics
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

            return Result<ICollection<TopicResponseDto>>.Success("All topics retrieved successfully", data: topics);
        }
    }
}
