using System.Reflection;
using System.Runtime.CompilerServices;
using System;
using MongoDB.Driver;
using MongoDB.Bson;
using calendar_backend_dotnet.Entities;

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
        private static string GetCollectionName<T>(T document)
        {
            Type t = document.GetType();
            string collectionName = (string)t.GetField("_CollectionName").GetValue(null);

            return collectionName;
        }
        public static IMongoCollection<T> GetCollection<T>(string collectionName)
        {
            var collection = client.GetCollection<T>(collectionName);

            return collection;
        }
        public static IMongoCollection<T> GetCollection<T>(T document)
        {
            var collectionName = GetCollectionName(document);
            var collection = client.GetCollection<T>(collectionName);

            return collection;
        }
        public static void InsertDocument<T>(T document)
        {
            var collection = GetCollection(document);

            collection.InsertOne(document);
        }
    }
}