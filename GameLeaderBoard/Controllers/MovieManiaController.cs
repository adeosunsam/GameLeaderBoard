using System.ComponentModel.DataAnnotations;
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
/*
        [HttpPost("tazama-webbhook")]
        public IActionResult GenerateMessagePair([FromBody]string messageObject)
        {
            return Ok(messageObject);
        }
*/
        [HttpGet]
        [Route("pending-challenge")]
        public IActionResult FetchChallengedData(string playerId)
        {
            var challenges = _movieService.FetchChallengedData(playerId);

            return Ok(challenges);
        }

        [HttpGet]
        [Route("activity/{userId}")]
        public async Task<IActionResult> FetchUserActivity(string userId)
        {
            var activities = await _movieService.FetchUserActivity(userId);
            return Ok(activities);
        }

        [HttpPost]
        [Route("follow-user")]
        public async Task<IActionResult> FollowUserRequest([FromBody] UserFollowRequest request)
        {
            var activities = await _movieService.RequestToFollowUser(request);
            return Ok(activities);
        }

        [HttpPost]
        [Route("manage-request")]
        public async Task<IActionResult> ManageFriendRequest([FromBody] ManageFriendRequest request)
        {
            var activities = await _movieService.ManageFriendRequest(request);
            return Ok(activities);
        }

        [HttpPost]
        [Route("save-challenge")]
        public IActionResult SaveChallengedData([FromBody] UserChallengeData request)
        {
            _movieService.SaveScoreForOfflineOpponent(request);
            return Ok();
        }

        [HttpPost]
        [Route("follow-topic/{topicId}/{userId}")]
        public async Task<IActionResult> FollowTopic(string topicId, string userId)
        {
            var result = await _movieService.FollowTopic(userId, topicId);
            return Ok(result);
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

        [HttpGet]
        [Route("user/{userId}")]
        public async Task<IActionResult> GetUser(string userId)
        {
            var userDetail = await _movieService.GetUserById(userId);
            return Ok(userDetail);
        }

        [HttpGet]
        [Route("user/{userId}/list")]
        public async Task<IActionResult> SearchUser(string userId, [Required][FromQuery] string searchParam)
        {
            var userDetail = await _movieService.Search(userId, searchParam);
            return Ok(userDetail);
        }

        [HttpGet]
        [Route("friends/{userId}")]
        public async Task<IActionResult> FetchUserFriends(string userId)
        {
            var userFriends = await _movieService.FetchUserFriends(userId);
            return Ok(userFriends);
        }

        [HttpGet]
        [Route("question/{topicId}")]
        public async Task<IActionResult> FetchQuestionByTopic(string topicId)
        {
            var question = await _movieService.FetchQuestionByTopic(topicId);
            return Ok(question);
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
        public async Task<IActionResult> Login(UserDetailDto request)
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

        [HttpGet]
        [Route("~/ping")]
        public IActionResult Ping()
        {
            return Ok();
        }
    }
}