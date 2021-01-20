using System;
using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;

namespace GhostNetwork.Profiles.MongoDb
{
    public class SecuritySettingsEntity
    {
        [BsonId]
        public Guid Id { get; set; }

        [BsonElement("userId")]
        public Guid UserId { get; set; }

        [BsonElement("accessToPosts")]
        public string AccessToPosts { get; set; }

        [BsonElement("accessToFriends")]
        public string AccessToFriends { get; set; }

        [BsonElement("certainUsersForPosts")]
        public List<Guid> CertainUsersForPosts { get; set; }

        [BsonElement("certainUsersForFriends")]
        public List<Guid> CertainUsersForFriends { get; set; }
    }
}
