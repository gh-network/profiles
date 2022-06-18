using System;
using System.Threading.Tasks;

namespace GhostNetwork.Profiles.SecuritySettings
{
    public interface ISecuritySettingStorage
    {
        Task<SecuritySetting> FindByUserIdAsync(Guid userId);

        Task<SecuritySettingsSection> FindSectionByUserIdAsync(Guid userId, string sectionName);

        Task<bool> ContainsInCertainUsersAsync(Guid userId, Guid ofUserId, string sectionName);

        Task UpsertAsync(SecuritySetting updatedSettings);
    }
}
