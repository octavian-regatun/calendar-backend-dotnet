using System.Linq;
using System.Security.Claims;
using System.IO.Enumeration;
using System.Text;
using System.Net.Http;
using System.Net.Http.Headers;
using System;
using Microsoft.AspNetCore.Mvc;
using calendar_backend_dotnet.Entities;
using System.Threading.Tasks;
using System.Text.Json;
using MongoDB.Driver;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using System.IdentityModel.Tokens.Jwt;
using System.Collections.Generic;
using Microsoft.IdentityModel.Tokens;
using calendar_backend_dotnet.Google;
using calendar_backend_dotnet.Models;
using Microsoft.AspNetCore.Http;
using MongoDB.Bson;

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
                return Redirect(AppSettings.FRONTEND_URI);
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

            if (AppSettings.IS_DEVELOPMENT)
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
            var session = SessionModel.CreateSession();
            var cookieOptions = SessionModel.CreateCookieOptions(session);
            Response.Cookies.Append("session", session.Token, cookieOptions);

            session.UserId = user.Id;
            user.sessionsId.Add(session.Id);

            Database.InsertDocument(session);

            return session;
        }

        private string GetSessionFromCookie()
        {
            return Request.Cookies["session"];
        }

        private bool IsLoggedIn()
        {
            return Authentication.IsLoggedIn(GetSessionFromCookie());
        }
    }
}