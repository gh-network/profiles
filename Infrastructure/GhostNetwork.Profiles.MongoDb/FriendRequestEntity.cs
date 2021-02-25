using GhostNetwork.Profiles.FriendsFuntionality;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace GhostNetwork.Profiles.MongoDb
{
    public class FriendRequestEntity
    {
        [BsonId]
        public Guid Id { get; set; }

        [BsonElement("fromUser")]
        public Guid FromUser { get; set; }

        [BsonElement("toUser")]
        public Guid ToUser { get; set; }

        [BsonElement("requestStatus")]
        public RequestStatus Status { get; set; }
    }
}
