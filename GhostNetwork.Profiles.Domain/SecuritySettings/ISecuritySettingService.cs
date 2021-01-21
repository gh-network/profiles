using System;
using System.Linq;
using System.Threading.Tasks;
using Domain;

namespace GhostNetwork.Profiles.SecuritySettings
{
    public interface ISecuritySettingService
    {
        Task<DomainResult> UpsertAsync(Guid profileId, Access accessToPosts, Guid[] certainUsersForPosts, Access accessToFriends, Guid[] certainForFriends);

        Task<(SecuritySetting, AccessProperties)> GetByUserIdAsync(Guid profileId);
    }

    public class SecuritySettingsService : ISecuritySettingService
    {
        private readonly ISecuritySettingStorage securitySettingsStorage;
        private readonly IProfileStorage profileStorage;

        public SecuritySettingsService(ISecuritySettingStorage securitySettingsStorage, IProfileStorage profileStorage)
        {
            this.securitySettingsStorage = securitySettingsStorage;
            this.profileStorage = profileStorage;
        }

        public async Task<DomainResult> UpsertAsync(Guid profileId, Access accessToPosts, Guid[] certainUsersForPosts, Access accessToFriends, Guid[] certainUsersForFriends)
        {
            var profile = await profileStorage.FindByIdAsync(profileId);
            if (profile == null)
            {
                return DomainResult.Error("Profile not found");
            }

            var (securitySettings, accessProperties) = await securitySettingsStorage.FindByUserIdAsync(profileId);
            if (securitySettings == null)
            {
                securitySettings = new SecuritySetting(
                    profileId,
                    certainUsersForPosts.ToList(),
                    certainUsersForFriends.ToList());
                accessProperties = new AccessProperties(accessToPosts, accessToFriends);
            }
            else
            {
                securitySettings.Update(certainUsersForPosts.ToList(), certainUsersForFriends.ToList());
                accessProperties.Update(accessToPosts, accessToFriends);
            }

            await securitySettingsStorage.UpsertAsync(securitySettings, accessProperties);
            return DomainResult.Success();
        }

        public async Task<(SecuritySetting, AccessProperties)> GetByUserIdAsync(Guid profileId)
        {
            var profile = await profileStorage.FindByIdAsync(profileId);
            if (profile == null)
            {
                return (null, null);
            }

            var (securitySettings, accessProperties) = await securitySettingsStorage.FindByUserIdAsync(profileId);
            return securitySettings == null ? DefaultSecuritySetting.GetDefaultSecuritySetting() : (securitySettings, accessProperties);
        }
    }
}
