using Blazesoft.Models;
using Blazesoft.Services;
using Microsoft.AspNetCore.Mvc;

namespace Blazesoft.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PlayerController : ControllerBase
    {
        private readonly IPlayerService _playerService;

        public PlayerController(IPlayerService playerService)
        {
            _playerService = playerService;
        }

        [HttpPost("update-balance")]
        public async Task<ActionResult> UpdateBalance([FromBody] UpdateBalanceRequest request)
        {
            var player = await _playerService.GetPlayerAsync(request.PlayerId);
            if (player == null) return NotFound("Player not found");

            player.Balance += request.Amount;
            await _playerService.UpdatePlayerAsync(player);

            return Ok(new { Balance = player.Balance });
        }
    }

}
