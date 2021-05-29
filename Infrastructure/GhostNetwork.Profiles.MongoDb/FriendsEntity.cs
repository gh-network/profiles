using System;
using GhostNetwork.Profiles.FriendsFunctionality;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace GhostNetwork.Profiles.MongoDb
{
    public class FriendsEntity
    {
        public ObjectId Id { get; set; }
        
        [BsonElement("userOne")]
        public Guid UserOne { get; set; }
        
        [BsonElement("userTwo")]
        public Guid UserTwo { get; set; }
        
        [BsonElement("status")]
        public RequestStatus Status { get; set; }
    }
}