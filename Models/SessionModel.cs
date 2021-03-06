using System.Net;
using System;
using System.Web;
using MongoDB.Bson.Serialization.Attributes;
using calendar_backend_dotnet.Entities;
using Microsoft.AspNetCore.Http;
using MongoDB.Bson;

namespace calendar_backend_dotnet.Models
{
    public class SessionModel
    {
        public static string _CollectionName = "sessions";
        public ObjectId Id { get; set; }
        public ObjectId UserId { get; set; }
        public IPAddress Ip { get; set; }
        public DateTime Create { get; set; }
        public DateTime LastUse { get; set; }
        public DateTime Expire { get; set; }

        public static SessionModel CreateSession(IPAddress ip = null)
        {
            var session = new SessionModel
            {
                Id = ObjectId.GenerateNewId(),
                Ip = ip,
                Create = DateTime.Now,
                LastUse = DateTime.Now,
                Expire = DateTime.Now.AddDays(7),
            };

            return session;
        }

        public static CookieOptions CreateCookieOptions(SessionModel session)
        {
            var cookie = new CookieOptions
            {
                Expires = session.Expire,
                Secure = true,
                HttpOnly = true,
            };

            return cookie;
        }
    }
}