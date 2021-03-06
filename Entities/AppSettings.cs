using System;
namespace calendar_backend_dotnet.Entities
{
    public static class AppSettings
    {
        public static string HERE_API_KEY = Environment.GetEnvironmentVariable("HERE_API_KEY");
        public static string GOOGLE_CLIENT_ID = Environment.GetEnvironmentVariable("GOOGLE_CLIENT_ID");
        public static string GOOGLE_CLIENT_SECRET = Environment.GetEnvironmentVariable("GOOGLE_CLIENT_SECRET");
        public static string MONGODB_DB_NAME = Environment.GetEnvironmentVariable("MONGODB_DB_NAME");
        public static string MONGODB_PASSWORD = Environment.GetEnvironmentVariable("MONGODB_PASSWORD");
        public static string FRONTEND_URI = Environment.GetEnvironmentVariable("FRONTEND_URI");
        public static string JWT_SECURITY_KEY = Environment.GetEnvironmentVariable("JWT_SECURITY_KEY");
        public static bool IS_DEVELOPMENT = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Development";
    }
}
