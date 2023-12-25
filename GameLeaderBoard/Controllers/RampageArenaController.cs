using GameLeaderBoard.DTOs;
using GameLeaderBoard.Service.Interface;
using GameLeaderBoard.Utility;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace GameLeaderBoard.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RampageArenaController : ControllerBase
    {
        private readonly IRampageArena _arena;

        public RampageArenaController(IRampageArena arena)
        {
            _arena = arena;
        }

        [HttpPost("submit-score")]
        [SwaggerResponse(StatusCodes.Status200OK, Type = typeof(Result<int>))]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, Type = typeof(Result<int>))]
        public async Task<IActionResult> SubmitScore([FromBody] SubmitScoreDto request)
        {
            var response = await _arena.SubmitScore(request);
            if (response.Data == null)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }

        [HttpGet("get-scores")]
        [SwaggerResponse(StatusCodes.Status200OK, Type = typeof(Result<List<GetScoreDto>>))]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, Type = typeof(Result<List<GetScoreDto>>))]
        public async Task<IActionResult> GetScores([FromQuery] string? playerId)
        {
            var response = await _arena.GetScores(playerId);
            if (response.Data == null)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }
    }
}