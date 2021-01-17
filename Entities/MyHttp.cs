using System.Net.Http;
namespace calendar_backend_dotnet.Entities
{
    public static class MyHttp
    {
        public static HttpClient client;
        static MyHttp()
        {
            client = new HttpClient();
        }
    }
}