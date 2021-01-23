using System;
using System.Threading.Tasks;

namespace GhostNetwork.Profiles.SecuritySettings
{
    public interface ISecuritySettingStorage
    {
        Task<SecuritySetting> FindByUserIdAsync(Guid userId);

        Task UpsertAsync(SecuritySetting updatedSettings);
    }
}
