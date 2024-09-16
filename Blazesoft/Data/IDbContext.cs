using Blazesoft.Models;
using MongoDB.Driver;

namespace Blazesoft.Data
{
    public interface IDbContext
    {
        IMongoCollection<Player> Players { get; }
    }

}
