using Microsoft.AspNetCore.Mvc;
using MovieManiaSignalr;
using static Infrastructure.DTOs.MovieManiaDtos;

namespace GameLeaderBoard.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MovieController : ControllerBase
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<MovieController> _logger;
        private readonly MovieManiaService _movieService;

        public MovieController(ILogger<MovieController> logger, HttpClient httpClient, MovieManiaService maniaService)
        {
            _logger = logger;
            _httpClient = httpClient;
            _movieService = maniaService;
        }

        [HttpGet]
        [Route("pending-challenge")]
        public IActionResult FetchChallengedData(string playerId)
        {
            var challenges = _movieService.FetchChallengedData(playerId);

            return Ok(challenges);
        }

        [HttpPost]
        [Route("save-challenge")]
        public IActionResult SaveChallengedData([FromBody] UserChallengeData request)
        {
            _movieService.SaveScoreForOfflineOpponent(request);
            return Ok();
        }

        [HttpPost]
        [Route("submit-score")]
        public async Task<IActionResult> DeleteCompletedChallenge([FromBody] List<SaveScoreForLeaderBoardDto> request)
        {
            await _movieService.DeleteDataAfterCompetition(request);
            return Ok();
        }

        [HttpGet]
        [Route("topics/{userId}")]
        public async Task<IActionResult> FetchAvailableTopics(string userId)
        {
            var topics = await _movieService.FetchAvailableTopics(userId);
            return Ok(topics);
        }

        [HttpGet]
        [Route("game-count/{userId}")]
        public async Task<IActionResult> FetchUserGamingCount(string userId)
        {
            var userGamingCount = await _movieService.FetchUserGamingCount(userId);
            return Ok(userGamingCount);
        }

        /*[HttpGet]
        [Route("followed-topic/{userId}")]
        public async Task<IActionResult> FetchUserFollowedTopic(string userId)
        {
            var followedTopic = await _movieService.FetchUserFollowedTopic(userId);
            return Ok(followedTopic);
        }*/

        [HttpPost]
        [Route("create-user")]
        public async Task<IActionResult> Login(UserDetailRequestDto request)
        {
            await _movieService.Login(request);
            return Ok();
        }

        [HttpPost]
        [Route("~/api/login")]
        public IActionResult Login(string userId)
        {
            var token = _movieService.Login(userId);
            return Ok(new { token });
        }
    }
}