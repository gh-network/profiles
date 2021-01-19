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

        [BsonElement("userId")]
        public Guid UserId { get; set; }

        [BsonElement("friendId")]
        public Guid FriendId { get; set; }
    }
}
