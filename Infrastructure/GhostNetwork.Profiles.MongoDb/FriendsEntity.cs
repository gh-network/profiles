using GhostNetwork.Profiles.FriendsFunctionality;
using MongoDB.Bson.Serialization.Attributes;
using System;
using MongoDB.Bson;

namespace GhostNetwork.Profiles.MongoDb
{
    public class FriendsEntity
    {
        public ObjectId Id { get; set; }
        
        [BsonElement("fromUser")]
        public Guid FromUser { get; set; }
        
        [BsonElement("fromUserName")]
        public string FromUserName { get; set; }

        [BsonElement("toUser")]
        public Guid ToUser { get; set; }
        
        [BsonElement("toUserName")]
        public string ToUserName { get; set; }

        [BsonElement("requestStatus")]
        public RequestStatus Status { get; set; }
    }
}
