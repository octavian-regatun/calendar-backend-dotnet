using System;
using System.Collections.Generic;
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
        public List<string> Roles = new List<string>();
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public Nullable<DateTime> Birthday { get; set; }
        public string Gender { get; set; }
        public List<ObjectId> Sessions = new List<ObjectId>();

        public static bool IsUserInCollection(string providerId)
        {
            var collection = Database.GetCollection<UserModel>(Collections.Users);
            var documents = collection.Find<UserModel>(x => x.ProviderId == providerId);

            if (documents.CountDocuments() == 0)
            {
                return false;
            }

            return true;
        }
    }

    public static class Gender
    {
        public static string Male = "M";
        public static string Female = "F";
        public static string Other = "O";
    }
}