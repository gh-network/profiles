using System;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace GhostNetwork.Profiles.SecuritySettings
{
    public interface ISecuritySettingStorage
    {
        Task<SecuritySetting> FindByUserIdAsync(Guid userId);

        Task<SecuritySettingsSection> FindSectionByUserIdAsync(Guid userId, string sectionName);

        ValueTask<bool> ContainsInCertainUsers(Guid userId, Guid ifUserId, string sectionName);

        Task UpsertAsync(SecuritySetting updatedSettings);
    }
}
