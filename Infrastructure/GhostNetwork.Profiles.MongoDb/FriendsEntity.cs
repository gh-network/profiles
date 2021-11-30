using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace GhostNetwork.Profiles.MongoDb
{
    public class FriendsEntity
    {
        public ObjectId Id { get; set; }

        [BsonElement("fromUser")]
        public Guid FromUser { get; set; }

        [BsonElement("toUser")]
        public Guid ToUser { get; set; }

        [BsonElement("status")]
        public RequestStatus Status { get; set; }
    }
}