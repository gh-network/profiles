using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace GhostNetwork.Profiles.MongoDb
{
    public class FriendEntity
    {
        [BsonId]
        public Guid Id { get; set; }

        [BsonElement("fromUser")]
        public Guid FromUser { get; set; }

        [BsonElement("toUser")]
        public Guid ToUser { get; set; }

        [BsonElement("isFriends")]
        public bool IsFriends { get; set; }

        [BsonElement("isFollowing")]
        public bool IsFollowing { get; set; }

        [BsonElement("isFollower")]
        public bool IsFollower { get; set; }
    }
}
