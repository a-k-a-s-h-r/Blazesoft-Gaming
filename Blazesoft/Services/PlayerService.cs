using Blazesoft.Data;
using Blazesoft.Models;
using MongoDB.Driver;

namespace Blazesoft.Services
{
    public class PlayerService: IPlayerService
    {
        private readonly IMongoCollection<Player> _players;

        public PlayerService(IDbContext context)
        {
            _players = context.Players;
        }

        public virtual async Task SeedInitialPlayersAsync()
        {
            var existingPlayers = await _players.Find(_ => true).ToListAsync();
            if (!existingPlayers.Any())
            {
                var initialPlayers = new List<Player>
                {
                    new Player { Name = "Player1", Balance = 2000 },
                    new Player { Name = "Player2", Balance = 800 },
                    new Player { Name = "Player3", Balance = 900 }
                };

                await _players.InsertManyAsync(initialPlayers);
            }
        }

        public virtual async Task<Player> GetPlayerAsync(string playerId)
        {
            return await _players.Find(p => p.Id == playerId).FirstOrDefaultAsync();
        }

        public virtual async Task UpdatePlayerAsync(Player player)
        {
            await _players.ReplaceOneAsync(p => p.Id == player.Id, player);
        }
    }

}
