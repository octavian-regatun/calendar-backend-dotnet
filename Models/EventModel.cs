using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace calendar_backend_dotnet.Models
{
    public class EventModel
    {
        public ObjectId Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime StartAt { get; set; }
        public string Image { get; set; }
    }

    public class GpsCoordinates
    {
        public double Lat { get; set; }
        public double Lon { get; set; }
    }
}