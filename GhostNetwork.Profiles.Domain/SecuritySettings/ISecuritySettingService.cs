﻿using System;
using System.Threading.Tasks;
using Domain;

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
    }
}
