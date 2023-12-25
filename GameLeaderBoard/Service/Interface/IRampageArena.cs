using GameLeaderBoard.DTOs;
using GameLeaderBoard.Utility;

namespace GameLeaderBoard.Service.Interface
{
    public interface IRampageArena
    {
        Task<Result<string>> SubmitScore(SubmitScoreDto request);
        Task<Result<List<GetScoreDto>>> GetScores(string? playerId = null);
    }
}