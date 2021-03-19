using System.Globalization;
using System.Reflection;
using System.Runtime.CompilerServices;
using System;
using MongoDB.Driver;
using MongoDB.Bson;
using calendar_backend_dotnet.Entities;
using calendar_backend_dotnet.Models;

namespace calendar_backend_dotnet.Entities
{
    public static class Database
    {
        public static IMongoDatabase client;
        static Database()
        {
            string MONGODB_PASSWORD = Environment.GetEnvironmentVariable("MONGODB_PASSWORD");
            string MONGODB_DB_NAME = Environment.GetEnvironmentVariable("MONGODB_DB_NAME");

            string URI =
                $"mongodb+srv://admin:{MONGODB_PASSWORD}@main.bcluj.mongodb.net/{MONGODB_DB_NAME}";

            var client = new MongoClient(URI);
            Database.client = client.GetDatabase(MONGODB_DB_NAME);
        }
    }

    public static class Collections
    {
        private static IMongoCollection<UserModel> users = Database.client.GetCollection<UserModel>("users");
        private static IMongoCollection<EventModel> events = Database.client.GetCollection<EventModel>("events");

        public static IMongoCollection<UserModel> Users
        {
            get
            {
                return users;
            }
        }
        public static IMongoCollection<EventModel> Events
        {
            get
            {
                return events;
            }
        }

        public static IMongoCollection<T> GetCollectionFromDocument<T>(T document)
        {
            if (typeof(T) == typeof(UserModel))
            {
                return Database.client.GetCollection<T>("users");
            }
            else if (typeof(T) == typeof(EventModel))
            {
                return Database.client.GetCollection<T>("events");
            }

            return null;
        }

        public static void AddDocument<T>(T document)
        {
            var collection = Collections.GetCollectionFromDocument<T>(document);

            collection.InsertOne(document);
        }

        public static UserModel AddDocument(Google.Apis.Auth.GoogleJsonWebSignature.Payload payload)
        {
            UserModel newUser = new UserModel
            {
                Id = ObjectId.GenerateNewId(),
                FirstName = payload.GivenName,
                LastName = payload.FamilyName,
                OAuthIssuer = payload.Issuer,
                OAuthSubject = payload.Subject
            };

            Collections.users.InsertOne(newUser);

            return newUser;
        }

        public static UserModel FindDocumentOrAdd(Google.Apis.Auth.GoogleJsonWebSignature.Payload payload)
        {
            var foundUser = users.Find<UserModel>(foundUser => foundUser.Email == payload.Email).FirstOrDefault();

            if (foundUser != null)
            {
                AddDocument(payload);
            }
            else
            {
                return foundUser;
            }

            return null;
        }
    }
}