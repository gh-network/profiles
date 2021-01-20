using System;
using System.Threading.Tasks;

namespace GhostNetwork.Profiles.SecuritySettings
{
    public interface ISecuritySettingStorage
    {
        Task UpdateAsync(SecuritySetting updatedSettings);

        Task<SecuritySetting> FindByUserIdAsync(Guid userId);
    }
}
