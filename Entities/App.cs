using System;
using System.Net.Http;

namespace calendar_backend_dotnet.Entities
{
    public static class App
    {
        public static class Settings
        {
            public static string HereApiKey = Environment.GetEnvironmentVariable("HERE_API_KEY");
            public static string GoogleClientId = Environment.GetEnvironmentVariable("GOOGLE_CLIENT_ID");
            public static string GoogleClientSecret = Environment.GetEnvironmentVariable("GOOGLE_CLIENT_SECRET");
            public static string MongoDbName = Environment.GetEnvironmentVariable("MONGODB_DB_NAME");
            public static string MongoDbPassword = Environment.GetEnvironmentVariable("MONGODB_PASSWORD");
            public static string FrontendUri = Environment.GetEnvironmentVariable("FRONTEND_URI");
            public static bool IsDevelopment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Development";
            public static string JwtSecret = Environment.GetEnvironmentVariable("JWT_SECRET");
        }
        public static class Http
        {
            public static HttpClient Client;
            static Http()
            {
                Client = new HttpClient();
            }
        }
    }
}
