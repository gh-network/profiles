﻿using System;
using MongoDB.Bson.Serialization.Attributes;

namespace GhostNetwork.Profiles.MongoDb
{
    #nullable disable
    public class ProfileEntity
    {
        [BsonId]
        public Guid Id { get; set; }

        [BsonElement("firstName")]
        public string FirstName { get; set; }

        [BsonElement("lastName")]
        public string LastName { get; set; }

        [BsonElement("gender")]
        public string Gender { get; set; }

        [BsonElement("dateOfBirth")]
        public long? DateOfBirth { get; set; }

        [BsonElement("city")]
        public string City { get; set; }

        [BsonElement("profilePicture")]
        public string ProfilePicture { get; set; }
    }
}
