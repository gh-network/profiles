using System;
using System.Collections.Generic;
using GhostNetwork.Profiles.SecuritySettings;
using MongoDB.Bson.Serialization.Attributes;

namespace GhostNetwork.Profiles.MongoDb
{
    public class SecuritySettingsSectionEntity
    {
        [BsonElement("access")]
        public Access Access { get; set; }

        [BsonElement("certainUsers")]
        public IEnumerable<Guid> CertainUsers { get; set; }

        public static explicit operator SecuritySettingsSection(SecuritySettingsSectionEntity entity)
        {
            if (entity == null)
            {
                return null;
            }

            return new SecuritySettingsSection(entity.Access, entity.CertainUsers);
        }

        public static explicit operator SecuritySettingsSectionEntity(SecuritySettingsSection settings)
        {
            if (settings == null)
            {
                return null;
            }

            return new SecuritySettingsSectionEntity
            {
                Access = settings.Access,
                CertainUsers = settings.CertainUsers
            };
        }
    }
}