using System;
using System.Collections.Generic;
using GhostNetwork.Profiles.SecuritySettings;
using MongoDB.Bson.Serialization.Attributes;

namespace GhostNetwork.Profiles.MongoDb
{
    public class SecuritySettingsEntity
    {
        [BsonId]
        public Guid UserId { get; set; }

        [BsonElement("accessToPosts")]
        public Access AccessToPosts { get; set; }

        [BsonElement("accessToFriends")]
        public Access AccessToFriends { get; set; }

        [BsonElement("certainUsersForPosts")]
        public List<Guid> CertainUsersForPosts { get; set; }

        [BsonElement("certainUsersForFriends")]
        public List<Guid> CertainUsersForFriends { get; set; }
    }
}
