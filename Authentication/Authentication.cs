using calendar_backend_dotnet.Entities;
using calendar_backend_dotnet.Models;
using MongoDB.Driver;

namespace calendar_backend_dotnet.AuthenticationServices
{
    public static class Authentication
    {
        public static class Providers
        {
            public static string Google = "google";
            public static string Facebook = "facebook";
            public static string Twitter = "twitter";
            public static string Apple = "apple";
            public static string GitHub = "github";
        }

        public static class Roles
        {
            public static string User = "U";
            public static string Free = "F";
            public static string Admin = "A";
        }

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