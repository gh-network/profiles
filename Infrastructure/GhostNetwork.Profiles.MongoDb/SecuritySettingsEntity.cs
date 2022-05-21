using System;
using MongoDB.Bson.Serialization.Attributes;

namespace GhostNetwork.Profiles.MongoDb
{
    public class SecuritySettingsEntity
    {
        [BsonId]
        public Guid UserId { get; set; }

        [BsonElement("friends")]
        public SecuritySettingsSectionEntity Friends { get; set; }

        [BsonElement("followers")]
        public SecuritySettingsSectionEntity Followers { get; set; }

        [BsonElement("comments")]
        public SecuritySettingsSectionEntity Comments { get; set; }

        [BsonElement("posts")]
        public SecuritySettingsSectionEntity Posts { get; set; }

        [BsonElement("profilePhoto")]
        public SecuritySettingsSectionEntity ProfilePhoto { get; set; }
    }
}
