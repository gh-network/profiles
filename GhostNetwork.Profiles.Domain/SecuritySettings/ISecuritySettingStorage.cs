using System;
using System.Threading.Tasks;

namespace GhostNetwork.Profiles.SecuritySettings
{
    public interface ISecuritySettingStorage
    {
        Task UpsertAsync(SecuritySetting updatedSettings);

        Task<SecuritySetting> FindByUserIdAsync(Guid userId);
    }
}
