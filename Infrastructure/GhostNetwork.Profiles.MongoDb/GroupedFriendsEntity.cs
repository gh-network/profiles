using System;
using System.Collections;
using System.Collections.Generic;
using GhostNetwork.Profiles.FriendsFunctionality;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace GhostNetwork.Profiles.MongoDb
{
    public class GroupedFriendsEntity
    {
        [BsonElement("_id")]
        public Guid Id { get; set; }
        
        [BsonElement("groupedFriends")]
        public IEnumerable<FriendsEntity> GroupedFriends { get; set; }
        
        [BsonElement("count")]
        public int TotalCount { get; set; }
    }
}