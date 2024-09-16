using Blazesoft.Models;
using MongoDB.Driver;

namespace Blazesoft.Data
{
        public class DbContext: IDbContext
    {
            private readonly IMongoDatabase _db;
            private readonly IConfiguration _configuration;
            public DbContext(IConfiguration configuration)
            {
                _configuration = configuration;
            var cns = _configuration.GetSection("MongoDBSettings:ConnectionString").Value;

            var client = new MongoClient(cns);
                _db = client.GetDatabase(_configuration["MongoDBSettings:DatabaseName"]);
            }

            public virtual IMongoCollection<Player> Players => _db.GetCollection<Player>(_configuration["MongoDBSettings:PlayersCollection"]);
        }

}