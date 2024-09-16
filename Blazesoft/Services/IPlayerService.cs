using Blazesoft.Models;

namespace Blazesoft.Services
{
    public interface IPlayerService
    {
        Task<Player> GetPlayerAsync(string playerId);
        Task UpdatePlayerAsync(Player player);
        Task SeedInitialPlayersAsync();
    }

}
