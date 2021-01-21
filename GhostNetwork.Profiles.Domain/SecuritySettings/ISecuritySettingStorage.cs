using System;
using System.Threading.Tasks;

namespace GhostNetwork.Profiles.SecuritySettings
{
    public interface ISecuritySettingStorage
    {
        Task UpsertAsync(SecuritySetting updatedSettings, AccessProperties accessProperties);

        Task<(SecuritySetting, AccessProperties)> FindByUserIdAsync(Guid userId);
    }
}
