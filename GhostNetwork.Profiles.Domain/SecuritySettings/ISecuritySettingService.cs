using System;
using System.Threading.Tasks;
using Domain;
using GhostNetwork.Profiles.Friends;

namespace GhostNetwork.Profiles.SecuritySettings
{
    public interface ISecuritySettingService
    {
        Task<SecuritySetting> GetByUserIdAsync(Guid userId);

        Task<DomainResult> UpsertAsync(
            Guid userId,
            SecuritySettingsSection friends,
            SecuritySettingsSection followers,
            SecuritySettingsSection publications,
            SecuritySettingsSection comments,
            SecuritySettingsSection profilePhoto);

        Task<bool> ResolveAccess(Guid userId, Guid toUserId, string sectionName);
    }

    public class SecuritySettingsService : ISecuritySettingService
    {
        private readonly ISecuritySettingStorage securitySettingsStorage;
        private readonly IRelationsService relationService;
        private readonly IProfileStorage profileStorage;

        public SecuritySettingsService(ISecuritySettingStorage securitySettingsStorage, IRelationsService relationService, IProfileStorage profileStorage)
        {
            this.securitySettingsStorage = securitySettingsStorage;
            this.relationService = relationService;
            this.profileStorage = profileStorage;
        }

        public async Task<SecuritySetting> GetByUserIdAsync(Guid userId)
        {
            if (await profileStorage.FindByIdAsync(userId) == null)
            {
                return null;
            }

            var securitySettings = await securitySettingsStorage.FindByUserIdAsync(userId);

            return securitySettings ?? SecuritySetting.DefaultForUser(userId);
        }

        public async Task<DomainResult> UpsertAsync(
            Guid userId,
            SecuritySettingsSection friends,
            SecuritySettingsSection followers,
            SecuritySettingsSection publications,
            SecuritySettingsSection comments,
            SecuritySettingsSection profilePhoto)
        {
            var profile = await profileStorage.FindByIdAsync(userId);
            if (profile == null)
            {
                return DomainResult.Error("User not found");
            }

            var securitySettings = await securitySettingsStorage.FindByUserIdAsync(userId);
            if (securitySettings == null)
            {
                securitySettings = new SecuritySetting(userId, friends, followers, publications, comments, profilePhoto);
            }
            else
            {
                securitySettings.Update(friends, followers, publications, comments, profilePhoto);
            }

            await securitySettingsStorage.UpsertAsync(securitySettings);

            return DomainResult.Success();
        }

        public async Task<bool> ResolveAccess(Guid userId, Guid toUserId, string sectionName)
        {
            if (userId == toUserId)
            {
                return true;
            }

            var section = await securitySettingsStorage.FindSectionByUserIdAsync(toUserId, sectionName);

            if (section.Access == Access.NoOne)
            {
                return false;
            }

            if (section.Access == Access.OnlyFriends)
            {
                if (!await relationService.IsFriendAsync(userId, toUserId))
                {
                    return false;
                }
            }

            if (section.Access == Access.OnlyCertainUsers)
            {
                if (!await securitySettingsStorage.ContainsInCertainUsersAsync(userId, toUserId, sectionName))
                {
                    return false;
                }
            }

            if (section.Access == Access.EveryoneExceptCertainUsers)
            {
                if (await securitySettingsStorage.ContainsInCertainUsersAsync(userId, toUserId, sectionName))
                {
                    return false;
                }
            }

            return true;
        }
    }
}
