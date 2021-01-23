using System.Linq;
using System;
using Microsoft.AspNetCore.Mvc;
using calendar_backend_dotnet.Entities;
using System.Threading.Tasks;
using MongoDB.Driver;
using Microsoft.AspNetCore.Hosting;
using System.Collections.Generic;
using calendar_backend_dotnet.Models;
using MongoDB.Bson;
using calendar_backend_dotnet.Auth.Google;
using calendar_backend_dotnet.Auth;

namespace calendar_backend_dotnet.Controllers
{

    [Route("api/auth/google")]
    [ApiController]
    public class AuthGoogleController : ControllerBase
    {
        private GoogleOAuth googleOAuth;
        public AuthGoogleController(IWebHostEnvironment env)
        {
            googleOAuth = new GoogleOAuth(env);
        }

        [HttpGet]
        public IActionResult Get()
        {
            if (IsLoggedIn())
            {
                return Redirect(App.Settings.FRONTEND_URI);
            }
            else
            {
                string codeUri = googleOAuth.GetCodeUri();

                return Redirect(codeUri);
            }
        }

        [HttpGet]
        [Route("callback")]
        public async Task<IActionResult> Callback([FromQuery] string code)
        {
            var accessToken = await googleOAuth.GetAccessTokenAsync(code);
            var meApi = await googleOAuth.GetMeApi(accessToken);

            return HandleUserData(meApi);
        }

        private IActionResult HandleUserData(GoogleMeApi meApiData)
        {
            if (!UserModel.IsUserInCollection(meApiData.Id))
            {
                var user = new UserModel
                {
                    Id = ObjectId.GenerateNewId(),
                    Provider = Providers.Google,
                    ProviderId = meApiData.Id,
                    Roles = new List<string> { Roles.User, Roles.Free },
                    FirstName = meApiData.GivenName,
                    LastName = meApiData.FamilyName,
                    Email = meApiData.Email,
                    Birthday = null,
                    Gender = null
                };

                CreateSessionCookie(ref user);

                Database.InsertDocument(user);
            }
            else
            {
                var collection = Database.GetCollection<UserModel>(Collections.Users);
                var user = collection.Find(x => x.ProviderId == meApiData.Id).FirstOrDefault();

                CreateSessionCookie(ref user);

                collection.ReplaceOne<UserModel>(x => x.ProviderId == meApiData.Id, user);
            }

            if (App.Settings.IS_DEVELOPMENT)
            {
                string frontendUri = Environment.GetEnvironmentVariable("FRONTEND_URI");
                return Redirect(frontendUri);
            }
            else
            {
                return Redirect("/");
            }
        }

        private SessionModel CreateSessionCookie(ref UserModel user)
        {
            var session = new SessionModel();
            var cookieOptions = SessionModel.CreateCookieOptions(session);
            Response.Cookies.Append("session", session.Token, cookieOptions);

            session.User = user.Id;
            user.Sessions.Add(session.Id);

            Database.InsertDocument(session);

            return session;
        }

        private string GetSessionFromCookie()
        {
            return Request.Cookies["session"];
        }

        private bool IsLoggedIn()
        {
            return AuthService.IsLoggedIn(GetSessionFromCookie());
        }
    }
}