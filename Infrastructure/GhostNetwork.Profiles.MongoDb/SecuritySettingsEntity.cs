using System;
using MongoDB.Bson.Serialization.Attributes;

namespace GhostNetwork.Profiles.MongoDb
{
    #nullable disable
    public class SecuritySettingsEntity
    {
        [BsonId]
        public Guid UserId { get; set; }

        [BsonElement("posts")]
        public SecuritySettingsSectionEntity Posts { get; set; }

        [BsonElement("friends")]
        public SecuritySettingsSectionEntity Friends { get; set; }
    }
}
