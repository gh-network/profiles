using System;
using System.Collections.Generic;
using System.Linq;
using GhostNetwork.Profiles.SecuritySettings;
using MongoDB.Bson.Serialization.Attributes;

namespace GhostNetwork.Profiles.MongoDb
{
    public class SecuritySettingsSectionEntity
    {
        [BsonElement("access")]
        public Access Access { get; set; }

        [BsonElement("certainUsers")]
        public IEnumerable<Guid> CertainUsers { get; set; } = Enumerable.Empty<Guid>();

        public static explicit operator SecuritySettingsSection(SecuritySettingsSectionEntity entity)
        {
            return new SecuritySettingsSection(entity.Access, entity.CertainUsers);
        }

        public static explicit operator SecuritySettingsSectionEntity(SecuritySettingsSection settings)
        {
            return new SecuritySettingsSectionEntity
            {
                Access = settings.Access,
                CertainUsers = settings.CertainUsers
            };
        }
    }
}