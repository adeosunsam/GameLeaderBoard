using Domain.Entity.MovieMania;
using GameLeaderBoard.Context;
using Infrastructure.Utility.Caching;
using Microsoft.Extensions.Configuration;
using static Infrastructure.DTOs.MovieManiaDtos;

namespace MovieManiaSignalr
{
    public partial class MovieManiaService
    {
        //private readonly ICacheDistribution _cache;
        private readonly LeaderBoardContext _context;
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;

        public MovieManiaService(LeaderBoardContext context, HttpClient httpClient,
            IConfiguration configuration)
        {
            //_cache = cache;
            _context = context;
            _httpClient = httpClient;
            _configuration = configuration;
        }

        //fetch all challenge for the current player
        public ICollection<UserChallengeData> FetchChallengedData(string id)// current playerId
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
