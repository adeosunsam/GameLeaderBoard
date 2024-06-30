using Infrastructure.Utility;
using Infrastructure.DTOs;

namespace Infrastructure.Service.Interface
{
    public interface IRampageArena
    {
        Task<Result<string>> SubmitScore(SubmitScoreDto request);
        Task<Result<List<GetScoreDto>>> GetScores(string? playerId = null);
    }
}