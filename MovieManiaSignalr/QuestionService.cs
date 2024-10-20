using Infrastructure.Utility;
using Microsoft.EntityFrameworkCore;
using static Infrastructure.DTOs.MovieManiaDtos;

namespace MovieManiaSignalr
{
    public partial class MovieManiaService
    {
        public async Task<Result<ICollection<QuestionDto>>> FetchQuestionByTopic(string topicId)
        {
            var response = new List<QuestionDto>();

            var questions = (await (from option in _context.QuestionOptions
                                    where !option.IsDeleted
                                    join q in _context.Questions on option.QuestionId equals q.Id
                                    where q.TopicId == topicId
                                    && !q.IsDeleted
                                    select new
                                    {
                                        option,
                                        question = q
                                    }).ToListAsync()).GroupBy(x => x.question).ToDictionary(y => y.Key, x => x.ToList());

            foreach (var question in questions)
            {
                var options = question.Value;

                response.Add(new QuestionDto
                {
                    Id = question.Key.Id,
                    Title = question.Key.Title,
                    Image = question.Key.Image,
                    IndexNumber = question.Key.IndexNumber,
                    Options = options.Select(x => new QuestionOption
                    {
                        Title = x.option.Title,
                        IsCorrectOption = x.option.IsCorrectOption
                    }).ToList()
                });
            }

            return Result<ICollection<QuestionDto>>.Success("questions retrieved successfully", data: response);
        }
    }
}
