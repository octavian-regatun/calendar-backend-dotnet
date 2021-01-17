using System.Text.Json.Serialization;

namespace calendar_backend_dotnet.Google
{
    public class GoogleToken
    {
        [JsonPropertyName("access_token")]
        public string AccessToken { get; set; }

        [JsonPropertyName("expires_in")]
        public int ExpiresIn { get; set; }

        [JsonPropertyName("refresh_token")]
        public string RefreshToken { get; set; }

        public string Scope { get; set; }

        [JsonPropertyName("token_type")]
        public string TokenType { get; set; }

        [JsonPropertyName("id_token")]
        public string IdToken { get; set; }
    }
}