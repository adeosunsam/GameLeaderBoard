using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using MovieManiaSignalr;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using static Infrastructure.DTOs.MovieManiaDtos;

namespace GameLeaderBoard.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MovieController : ControllerBase
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<MovieController> _logger;
        private readonly IConfiguration _configuration;
        private readonly MovieManiaService _movieService;

        public MovieController(ILogger<MovieController> logger, HttpClient httpClient,
            IConfiguration configuration, MovieManiaService maniaService)
        {
            _logger = logger;
            _httpClient = httpClient;
            _configuration = configuration;
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

        [HttpPost]
        [Route("~/api/login")]
        public async Task<IActionResult> Login(string userId)
        {
            var token = GenerateJwtToken(userId);

            return Ok(new { token });
        }

        private string GenerateJwtToken(string userId)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, userId),
                // Add other claims as needed
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtSettings:SecretKey"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _configuration["JwtSettings:ValidIssuer"],
                audience: _configuration["JwtSettings:ValidAudience"],
                claims: claims,
                expires: DateTime.Now.AddDays(30),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}