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

        public async Task UpdateAsync(Guid userId, SecuritySetting updatedSettings)
        {
            var filter = Builders<SecuritySettingsEntity>.Filter.Eq(x => x.UserId, userId);
            var settings = await context.SecuritySettings.Find(filter).FirstOrDefaultAsync();
            if (settings == null)
            {
                var settingsEntity = new SecuritySettingsEntity
                {
                    AccessToFriends = updatedSettings.AccessToFriends.ToString(),
                    AccessToPosts = updatedSettings.AccessToPosts.ToString(),
                    UserId = userId,
                    CertainUsersForFriends = updatedSettings.CertainUsersForFriends,
                    CertainUsersForPosts = updatedSettings.CertainUsersForPosts
                };
                await context.SecuritySettings.InsertOneAsync(settingsEntity);
                return;
            }

            var update = Builders<SecuritySettingsEntity>.Update.Set(s => s.AccessToPosts, updatedSettings.AccessToPosts.ToString())
                .Set(s => s.CertainUsersForPosts, updatedSettings.CertainUsersForPosts)
                .Set(s => s.AccessToFriends, updatedSettings.AccessToFriends.ToString())
                .Set(s => s.CertainUsersForFriends, updatedSettings.CertainUsersForFriends);
            
            await context.SecuritySettings.UpdateOneAsync(filter, update);
        }

        public async Task<SecuritySetting> FindByUserIdAsync(Guid userId)
        {
            var filter = Builders<SecuritySettingsEntity>.Filter.Eq(x => x.UserId, userId);
            var settings = await context.SecuritySettings.Find(filter).FirstOrDefaultAsync();
            return settings == null ? null : ToDomain(settings);
        }

        public static SecuritySetting ToDomain(SecuritySettingsEntity entity)
        {
            if (Enum.TryParse<Access>(entity.AccessToFriends, out var eAccessToFriends)
                && Enum.TryParse<Access>(entity.AccessToPosts, out var eAccessToPosts))
            {
                return new SecuritySetting(
                    entity.Id,
                    entity.UserId,
                    eAccessToPosts,
                    eAccessToFriends,
                    entity.CertainUsersForPosts,
                    entity.CertainUsersForFriends);
            }

            throw new InvalidCastException();
        }
    }
}
