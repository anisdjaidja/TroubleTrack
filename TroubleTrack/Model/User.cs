using MongoDB.Bson.Serialization.Attributes;

namespace TroubleTrack.Model
{
    public class User
    {
        [BsonId]
        public int Id { get; set; }

        public string Email { get; set; }

        [BsonElement("Password")]
        public string Password { get; set; }
    }
}
