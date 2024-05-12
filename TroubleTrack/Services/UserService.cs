using Microsoft.IdentityModel.Tokens;
using MongoDB.Driver;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TroubleTrack.Database;
using TroubleTrack.Model;

namespace TroubleTrack.Services
{
    public class UserService
    {
        public MongoClient DBClient;
        public IMongoDatabase mongoDB;
        public IMongoCollection<User> UsersCollection;
        private readonly string _key;

        public UserService(MongoDatabase databaseConnector)
        {
            while (true)
            {
                if (databaseConnector.Connected)
                    break;
            }
            DBClient = databaseConnector.DBclient;
            mongoDB = DBClient.GetDatabase("TroubleTrackDB");
            UsersCollection = mongoDB.GetCollection<User>("Users");

            _key = DbConfig.JwtKey;
        }
        public string? Auth(string email, string password)
        {
            var user = UsersCollection.Find(x => x.Email == email && x.Password == password).FirstOrDefault();
            if (user == null)
                return null;

            var handler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_key);
            var tokenDesc = new SecurityTokenDescriptor()
            {
                Subject = new System.Security.Claims.ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Email, email),
                }),

                Expires = DateTime.UtcNow.AddHours(12),
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature
                    )

            };

            var token = handler.CreateToken(tokenDesc);

            return handler.WriteToken(token);
        }
    }
}
