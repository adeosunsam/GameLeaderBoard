using Domain.Entity;
using GameLeaderBoard.Context;
using Infrastructure.DTOs;
using Infrastructure.Service.Interface;
using Infrastructure.Utility;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Service.Implementation
{
    public class RampageArena : IRampageArena
    {
        private readonly LeaderBoardContext _context;
        private readonly ILogger<RampageArena> _logger;

        public RampageArena(LeaderBoardContext context, ILogger<RampageArena> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<Result<string>> SubmitScore(SubmitScoreDto request)
        {
            string? playerId = null;
            if (request.PlayerId == null && request.PlayerName == null)
            {
                _logger.LogError("----------------PLAYERID OR PLAYERNAME IS NULL: unable to submit score at the moment----------------");
                return Result<string>.Fail("unable to submit score at the moment", "400");
            }

            //submit score for new player
            if (request.PlayerId == null)
            {
                var leaderboard = new RampageArenaLeaderBoard
                {
                    PlayerName = request.PlayerName,
                    PlayerScore = request.Score
                };

                await _context.RampageArenaLeaderBoards.AddAsync(leaderboard);

                await _context.SaveChangesAsync();
                playerId = leaderboard.Id;
            }

            //update existing player score
            if (request.PlayerName == null)
            {
                var leaderboard = await _context.RampageArenaLeaderBoards.FirstOrDefaultAsync(x => x.Id == request.PlayerId);
                if (leaderboard == null)
                {
                    _logger.LogError("----------------LEADERBOARD NOT FOUND:Invalid player id provided----------------");
                    return Result<string>.Fail("Invalid player id provided", "404");
                }
                playerId = leaderboard.Id;

                leaderboard.PlayerScore = request.Score;
            }

            //update rank
            {
                var leaderboards = await _context.RampageArenaLeaderBoards
                    .OrderBy(x => x.CreatedOn)
                    .OrderByDescending(x => x.PlayerScore).ToListAsync();

                for (int i = 0; i < leaderboards.Count; i++)
                {
                    if (i > 0 && leaderboards[i - 1].PlayerScore == leaderboards[i].PlayerScore)
                    {
                        leaderboards[i].Rank = leaderboards[i - 1].Rank;
                        continue;
                    }
                    leaderboards[i].Rank = i + 1;
                }
            }

            await _context.SaveChangesAsync();

            _logger.LogInformation("----------------SCORE SUBMITTED: successfully submitted----------------");

            return Result<string>.Success("Successfully submitted", data: playerId);
        }

        public async Task<Result<List<GetScoreDto>>> GetScores(string? playerId = null)
        {
            var leaderboards = await _context.RampageArenaLeaderBoards
                    .OrderBy(x => x.CreatedOn)
                    .OrderByDescending(x => x.PlayerScore).Take(10).ToListAsync();

            if (playerId != null && !leaderboards.Exists(x => x.Id == playerId))
            {
                leaderboards.Remove(leaderboards[^1]);

                var playerScore = await _context.RampageArenaLeaderBoards.FirstOrDefaultAsync(x => x.Id == playerId);

                if (playerScore == null)
                {
                    _logger.LogError("----------------PLAYER LEADERBOARD:player leaderboard not found----------------");
                }
                else
                {
                    leaderboards.Add(playerScore);
                }
            }

            var result = leaderboards.Select(x => new GetScoreDto
            {
                Rank = x.Rank,
                PlayerName = x.PlayerName,
                Score = x.PlayerScore
            }).ToList();

            return Result<List<GetScoreDto>>.Success("Leaderboard fetched successfully", data: result);

        }
    }
}
