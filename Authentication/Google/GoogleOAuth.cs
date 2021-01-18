using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using calendar_backend_dotnet.Entities;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;

namespace calendar_backend_dotnet.AuthenticationServices.Google
{
    public class GoogleOAuth
    {
        private string redirectUri;
        public GoogleOAuth(IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                redirectUri = "https://me.mydomain.com:5001/api/auth/google/callback";
            }
            else
            {
                redirectUri = "/";
            }
        }
        public string GetCodeUri()
        {
            return
                "https://accounts.google.com/o/oauth2/v2/auth?" +
                $"client_id={App.Settings.GOOGLE_CLIENT_ID}&" +
                $"redirect_uri={redirectUri}&" +
                "response_type=code&" +
                "scope=openid profile email&" +
                "prompt=select_account&" +
                "access_type=offline";
        }

        public async Task<string> GetAccessTokenAsync(string code)
        {
            const string TOKEN_URI = "https://oauth2.googleapis.com/token";

            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, TOKEN_URI);
            var content = new StringContent(
            $"code={code}&" +
            $"client_id={App.Settings.GOOGLE_CLIENT_ID}&" +
            $"client_secret={App.Settings.GOOGLE_CLIENT_SECRET}&" +
            "grant_type=authorization_code&" +
            $"redirect_uri={redirectUri}&",
            Encoding.UTF8,
            "application/x-www-form-urlencoded"
            );
            request.Content = content;
            var responseToken = await App.Http.client.SendAsync(request);
            var jsonToken = await responseToken.Content.ReadAsStringAsync();
            GoogleToken token = JsonSerializer.Deserialize<GoogleToken>(jsonToken);

            return token.AccessToken;
        }

        public async Task<GoogleMeApi> GetMeApi(string accessToken)
        {
            const string API_URI = "https://www.googleapis.com/userinfo/v2/me";

            App.Http.client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            var meApiResponse = await App.Http.client.GetStringAsync(API_URI);
            var meApi = JsonSerializer.Deserialize<GoogleMeApi>(meApiResponse);

            return meApi;
        }
    }
}