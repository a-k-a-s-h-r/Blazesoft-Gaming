using Blazesoft.Models;
using Blazesoft.Services;
using Microsoft.AspNetCore.Mvc;
using Blazesoft.Helper;

namespace Blazesoft.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SpinController : ControllerBase
    {
        private readonly IPlayerService _playerService;
        private readonly int _width;
        private readonly int _height;
        Helpers helpers = new Helpers();

        public SpinController(IPlayerService playerService, IConfiguration configuration)
        {
            _playerService = playerService;
            _width = int.Parse(configuration["SlotMachineSettings:Width"]);
            _height = int.Parse(configuration["SlotMachineSettings:Height"]);
        }

        [HttpPost("spin")]
        public async Task<ActionResult<SpinResult>> Spin([FromBody] SpinRequest request)
        {
            var player = await _playerService.GetPlayerAsync(request.PlayerId);
            if (player == null) return NotFound("Player not found");

            if (player.Balance < request.Bet)
            {
                return BadRequest("Insufficient balance.");
            }

            player.Balance -= request.Bet;

            int[,] resultMatrix = GenerateRandomMatrix(_width, _height);

            decimal win = CalculateWin(resultMatrix, request.Bet);

            player.Balance += win;

            await _playerService.UpdatePlayerAsync(player);

            return Ok(new SpinResult
            {
                ResultMatrix = helpers.ConvertMatrixToList(resultMatrix),
                Win = win,
                NewBalance = player.Balance
            });
        }

        private int[,] GenerateRandomMatrix(int width, int height)
        {
            var random = new Random();
            int[,] matrix = new int[height, width];
            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    matrix[i, j] = random.Next(0, 10);
                }
            }
            return matrix;
        }

        private decimal CalculateWin(int[,] resultMatrix, decimal bet)
        {
            decimal win = 0;
            win = helpers.CalculateWin(resultMatrix, bet);

            return win;
        }
    }

}
