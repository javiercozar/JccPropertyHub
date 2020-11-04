using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace JccPropertyHub.Domain.Core.Dto {
    public class Log {
        [BsonId]
        public ObjectId ID { get; set; }
        public string Response { get; set; }
        public string Request { get; set; }
        public DateTime CreatetionDate { get; set; }
    }
}