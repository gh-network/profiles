using System;
using System.Threading.Tasks;
using GhostNetwork.Profiles.SecuritySettings;
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

        public async Task UpdateAsync(SecuritySetting updatedSettings)
        {
            var filter = Builders<SecuritySettingsEntity>.Filter.Eq(x => x.UserId, updatedSettings.UserId);
            var update = Builders<SecuritySettingsEntity>.Update.Set(s => s.AccessToPosts, updatedSettings.AccessToPosts)
                .Set(s => s.CertainUsersForPosts, updatedSettings.CertainUsersForPosts)
                .Set(s => s.AccessToFriends, updatedSettings.AccessToFriends)
                .Set(s => s.CertainUsersForFriends, updatedSettings.CertainUsersForFriends);
            
            await context.SecuritySettings.UpdateOneAsync(filter, update, new UpdateOptions() {IsUpsert = true});
        }

        public async Task<SecuritySetting> FindByUserIdAsync(Guid userId)
        {
            var filter = Builders<SecuritySettingsEntity>.Filter.Eq(x => x.UserId, userId);
            var settings = await context.SecuritySettings.Find(filter).FirstOrDefaultAsync();
            return settings == null ? null : ToDomain(settings);
        }

        public static SecuritySetting ToDomain(SecuritySettingsEntity entity)
        {
            return new SecuritySetting(
                    entity.UserId,
                    entity.AccessToPosts,
                    entity.AccessToFriends,
                    entity.CertainUsersForPosts,
                    entity.CertainUsersForFriends);
        }
    }
}
