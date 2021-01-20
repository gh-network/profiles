using System;
using System.Linq;
using System.Threading.Tasks;
using Domain;

namespace GhostNetwork.Profiles.SecuritySettings
{
    public interface ISecuritySettingService
    {
        Task<DomainResult> UpsertAsync(Guid profileId, Access accessForPosts, Guid[] certainUsersForPosts, Access accessForFriends, Guid[] certainForFriends);

        Task<SecuritySetting> GetByUserIdAsync(Guid profileId);
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

        public async Task<DomainResult> UpsertAsync(Guid profileId, Access accessForPosts, Guid[] certainUsersForPosts, Access accessForFriends, Guid[] certainUsersForFriends)
        {
            var profile = await profileStorage.FindByIdAsync(profileId);
            if (profile == null)
            {
                return DomainResult.Error("Profile not found");
            }

            var security = await securitySettingsStorage.FindByUserIdAsync(profileId);
            if (security == null)
            {
                security = new SecuritySetting(
                    default,
                    profileId,
                    accessForPosts,
                    accessForFriends,
                    certainUsersForPosts.ToList(),
                    certainUsersForFriends.ToList());
            }
            else
            {
                security.Update(accessForPosts, accessForFriends, certainUsersForPosts.ToList(), certainUsersForFriends.ToList());
            }

            await securitySettingsStorage.UpdateAsync(profileId, security);
            return DomainResult.Success();
        }

        public async Task<SecuritySetting> GetByUserIdAsync(Guid profileId)
        {
            var profile = await profileStorage.FindByIdAsync(profileId);
            if (profile == null)
            {
                return null;
            }

            var securitySettings = await securitySettingsStorage.FindByUserIdAsync(profileId);
            return securitySettings ?? DefaultSecuritySetting.GetDefaultSecuritySetting();
        }
    }
}
