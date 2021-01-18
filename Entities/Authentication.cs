using calendar_backend_dotnet.Models;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;

namespace calendar_backend_dotnet.Entities
{
    public static class Authentication
    {
        public static bool IsLoggedIn(string sessionId)
        {
            if (sessionId == null)
            {
                return false;
            }

            var collection = Database.GetCollection<SessionModel>(Collections.Sessions);
            var session = collection.Find(x => x.Token == sessionId).FirstOrDefault();

            if (session != null)
            {
                return true;
            }

            return false;
        }
    }
}