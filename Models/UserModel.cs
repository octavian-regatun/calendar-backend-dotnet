using System;
using calendar_backend_dotnet.Google;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;

namespace calendar_backend_dotnet.Entities
{
    public class UserModel
    {
        public static string _CollectionName = "users";
        public ObjectId Id { get; set; }
        public string Provider { get; set; }
        public string ProviderId { get; set; }
        public string[] Roles { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public Nullable<DateTime> Birthday { get; set; }
        public string Gender { get; set; }
        public ObjectId[] sessionsId { get; set;}

        public static bool IsUserInCollection(UserModel user)
        {
            var collection = Database.GetCollection(user);
            var documents = collection.Find<UserModel>(x => x.ProviderId == user.ProviderId);

            if (documents.CountDocuments() == 0)
            {
                return false;
            }

            return true;
        }
    }
}