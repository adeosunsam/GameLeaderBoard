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
            string? playerId = request.PlayerId;
            if (string.IsNullOrWhiteSpace(request.PlayerId) && string.IsNullOrWhiteSpace(request.PlayerName))
            {
                _logger.LogError("----------------PLAYERID OR PLAYERNAME IS NULL: unable to submit score at the moment----------------");
                return Result<string>.Fail("unable to submit score at the moment", "400");
            }

            //submit score for new player
            if (string.IsNullOrWhiteSpace(request.PlayerId))
            {
                var existingName = await _context.RampageArenaLeaderBoards.FirstOrDefaultAsync(x => x.PlayerName.ToLower() == request.PlayerName.ToLower());

                if (existingName != null)
                {
                    _logger.LogError("----------------EXISTING LEADERBOARD FOUND:Name already exist in the satabase----------------");
                    return Result<string>.Fail("Name already taken", "404");
                }

                var leaderboard = new RampageArenaLeaderBoard
                {
                    PlayerName = request.PlayerName,
                    PlayerScore = request.Score
                };

                await _context.RampageArenaLeaderBoards.AddAsync(leaderboard);

                await _context.SaveChangesAsync();
                playerId = leaderboard.Id;
            }

            ////update existing player score
            //if (!string.IsNullOrWhiteSpace(request.PlayerId))
            //{
            //    var leaderboard = await _context.RampageArenaLeaderBoards.FirstOrDefaultAsync(x => x.Id == request.PlayerId);
            //    if (leaderboard == null)
            //    {
            //        _logger.LogError("----------------LEADERBOARD NOT FOUND:Invalid player id provided----------------");
            //        return Result<string>.Fail("Invalid player id provided", "404");
            //    }
            //    playerId = leaderboard.Id;

            //    leaderboard.PlayerScore = request.Score;
            //}

            //update rank
            {
                var leaderboards = await (from l in _context.RampageArenaLeaderBoards
                                   select l).ToListAsync();

                if (!string.IsNullOrWhiteSpace(request.PlayerId))
                {
                    var player = leaderboards.FirstOrDefault(x => x.Id == request.PlayerId);

                    if (player == null)
                    {
                        _logger.LogError("----------------LEADERBOARD NOT FOUND:Invalid player id provided----------------");
                        return Result<string>.Fail("Invalid player id provided", "404");
                    }
                    player.PlayerScore = request.Score;
                }

                leaderboards = leaderboards.OrderByDescending(x => x.PlayerScore).ToList();

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
                    .OrderByDescending(x => x.PlayerScore)
                    .ThenByDescending(x => x.ModifiedOn).Take(10).ToListAsync();

            if (playerId != null && !leaderboards.Exists(x => x.Id == playerId))
            {
                var playerScore = await _context.RampageArenaLeaderBoards.FirstOrDefaultAsync(x => x.Id == playerId);

                if (playerScore == null)
                {
                    _logger.LogError("----------------PLAYER LEADERBOARD:player leaderboard not found----------------");
                }
                else if(playerScore.PlayerScore > 0)
                {
                    leaderboards.Remove(leaderboards[^1]);

                    leaderboards.Add(playerScore);
                }
            }

            var result = leaderboards.Select(x => new GetScoreDto
            {
                PlayerId = x.Id,
                Rank = x.Rank,
                PlayerName = x.PlayerName,
                Score = x.PlayerScore,
                ModifiedDate = x.ModifiedOn
            }).OrderBy(y => y.Rank).ThenBy(z => z.ModifiedDate).ToList();

            return Result<List<GetScoreDto>>.Success("Leaderboard fetched successfully", data: result);

        }
    }
}
