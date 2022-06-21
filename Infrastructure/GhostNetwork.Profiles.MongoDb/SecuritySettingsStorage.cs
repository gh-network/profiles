using System;
using System.Linq;
using System.Threading.Tasks;
using GhostNetwork.Profiles.SecuritySettings;
using MongoDB.Bson;
using MongoDB.Driver;

namespace GhostNetwork.Profiles.MongoDb
{
    public class SecuritySettingsStorage : ISecuritySettingStorage
    {
        private readonly MongoDbContext context;

        public SecuritySettingsStorage(MongoDbContext context)
        {
            this.context = context;
        }

        public async Task<SecuritySetting> FindByUserIdAsync(Guid userId)
        {
            var filter = Builders<SecuritySettingsEntity>.Filter.Eq(x => x.UserId, userId);
            var settings = await context.SecuritySettings.Find(filter).FirstOrDefaultAsync();

            return settings == null
                ? null
                : ToDomain(settings);
        }

        public async Task<Access> GetSectionAccessAsync(Guid userId, string sectionName)
        {
            var filter = Builders<SecuritySettingsEntity>.Filter.Eq(x => x.UserId, userId);

            var project = new BsonDocument
            {
                { "_id", 0 },
                { "access", $"${sectionName}.access" }
            };

            var section = await context.SecuritySettings
                .Aggregate()
                .Match(filter)
                .Project<SecuritySettingsSectionEntity>(project)
                .FirstOrDefaultAsync();

            return new SecuritySettingsSection(section.Access, null!).Access;
        }

        public async Task<bool> ContainsInCertainUsersAsync(Guid userId, Guid ofUserId, string sectionName)
        {
            var filter = Builders<SecuritySettingsEntity>.Filter.Eq(x => x.UserId, ofUserId) &
                         Builders<SecuritySettingsEntity>.Filter.In($"{sectionName}.certainUsers", new[] { userId });

            return await context.SecuritySettings
                .Find(filter)
                .AnyAsync();
        }

        public async Task UpsertAsync(SecuritySetting updatedSettings)
        {
            var filter = Builders<SecuritySettingsEntity>.Filter.Eq(x => x.UserId, updatedSettings.UserId);
            var update = Builders<SecuritySettingsEntity>.Update
                .Set(s => s.Friends, (SecuritySettingsSectionEntity)updatedSettings.Friends)
                .Set(s => s.Followers, (SecuritySettingsSectionEntity)updatedSettings.Followers)
                .Set(s => s.Posts, (SecuritySettingsSectionEntity)updatedSettings.Posts)
                .Set(s => s.Comments, (SecuritySettingsSectionEntity)updatedSettings.Comments)
                .Set(s => s.ProfilePhoto, (SecuritySettingsSectionEntity)updatedSettings.ProfilePhoto);

            await context.SecuritySettings
                .UpdateOneAsync(filter, update, new UpdateOptions { IsUpsert = true });
        }

        private static SecuritySetting ToDomain(SecuritySettingsEntity entity)
        {
            return entity == null
                ? null
                : new SecuritySetting(
                    entity.UserId,
                    (SecuritySettingsSection)entity.Friends ?? new SecuritySettingsSection(Access.Everyone, Enumerable.Empty<Guid>()),
                    (SecuritySettingsSection)entity.Followers ?? new SecuritySettingsSection(Access.Everyone, Enumerable.Empty<Guid>()),
                    (SecuritySettingsSection)entity.Posts ?? new SecuritySettingsSection(Access.Everyone, Enumerable.Empty<Guid>()),
                    (SecuritySettingsSection)entity.Comments ?? new SecuritySettingsSection(Access.Everyone, Enumerable.Empty<Guid>()),
                    (SecuritySettingsSection)entity.ProfilePhoto ?? new SecuritySettingsSection(Access.Everyone, Enumerable.Empty<Guid>()));
        }
    }
}
