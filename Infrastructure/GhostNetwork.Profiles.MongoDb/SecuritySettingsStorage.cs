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

        public async Task UpsertAsync(SecuritySetting updatedSettings, AccessProperties accessProperties)
        {
            var filter = Builders<SecuritySettingsEntity>.Filter.Eq(x => x.UserId, updatedSettings.UserId);
            var update = Builders<SecuritySettingsEntity>.Update.Set(s => s.AccessToPosts, accessProperties.AccessToPosts)
                .Set(s => s.CertainUsersForPosts, updatedSettings.CertainUsersForPosts)
                .Set(s => s.AccessToFriends, accessProperties.AccessToFriends)
                .Set(s => s.CertainUsersForFriends, updatedSettings.CertainUsersForFriends);
            
            await context.SecuritySettings.UpdateOneAsync(filter, update, new UpdateOptions() {IsUpsert = true});
        }

        public async Task<(SecuritySetting, AccessProperties)> FindByUserIdAsync(Guid userId)
        {
            var filter = Builders<SecuritySettingsEntity>.Filter.Eq(x => x.UserId, userId);
            var settings = await context.SecuritySettings.Find(filter).FirstOrDefaultAsync();
            return (settings == null
                ? (null, null)
                : (ToDomain(settings), new AccessProperties(settings.AccessToPosts, settings.AccessToFriends)));
        }

        public static SecuritySetting ToDomain(SecuritySettingsEntity entity)
        {
            return new SecuritySetting(
                    entity.UserId,
                    entity.CertainUsersForPosts,
                    entity.CertainUsersForFriends);
        }
    }
}
