using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain;

namespace GhostNetwork.Profiles.SecuritySettings
{
    public interface ISecuritySettingService
    {
        Task<SecuritySetting?> GetByUserIdAsync(Guid userId);

        Task<IEnumerable<SecuritySetting>> FindManyByUserIdsAsync(IEnumerable<Guid> userIds);

        Task<DomainResult> UpsertAsync(
            Guid userId,
            SecuritySettingsSection publications,
            SecuritySettingsSection friends);
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

        public async Task<SecuritySetting?> GetByUserIdAsync(Guid userId)
        {
            if (await profileStorage.FindByIdAsync(userId) == null)
            {
                return null;
            }

            var securitySettings = await securitySettingsStorage.FindByUserIdAsync(userId);

            return securitySettings ?? SecuritySetting.DefaultForUser(userId);
        }

        public async Task<IEnumerable<SecuritySetting>> FindManyByUserIdsAsync(IEnumerable<Guid> userIds)
        {
            var existedUsers = await profileStorage.SearchByIdsAsync(userIds);
            if (!existedUsers.Any())
            {
                return Enumerable.Empty<SecuritySetting>();
            }

            return await securitySettingsStorage.FindManyByUserIdsAsync(existedUsers.Select(u => u.Id));
        }

        public async Task<DomainResult> UpsertAsync(
            Guid userId,
            SecuritySettingsSection publications,
            SecuritySettingsSection friends)
        {
            var profile = await profileStorage.FindByIdAsync(userId);
            if (profile == null)
            {
                return DomainResult.Error("User not found");
            }

            var securitySettings = await securitySettingsStorage.FindByUserIdAsync(userId);
            if (securitySettings == null)
            {
                securitySettings = new SecuritySetting(userId, publications, friends);
            }
            else
            {
                securitySettings.Update(publications, friends);
            }

            await securitySettingsStorage.UpsertAsync(securitySettings);

            return DomainResult.Success();
        }
    }
}
