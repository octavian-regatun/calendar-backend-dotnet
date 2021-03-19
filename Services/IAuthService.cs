using calendar_backend_dotnet.Entities;
using calendar_backend_dotnet.Models;

namespace calendar_backend_dotnet.Services
{
    public interface IAuthService
    {
        UserModel Authenticate(Google.Apis.Auth.GoogleJsonWebSignature.Payload payload);
    }
    public class AuthService : IAuthService
    {
        public UserModel Authenticate(Google.Apis.Auth.GoogleJsonWebSignature.Payload payload)
        {
            UserModel newUser = Collections.AddDocument(payload);

            return newUser;
        }
    }
}