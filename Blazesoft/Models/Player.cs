using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace Blazesoft.Models
{
    public class Player
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public string Name { get; set; }
        public decimal Balance { get; set; }
    }

}
