using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GhostNetwork.Profiles.SecuritySettings
{
    public interface ISecuritySettingStorage
    {
        Task<SecuritySetting?> FindByUserIdAsync(Guid userId);

        Task<IEnumerable<SecuritySetting>> FindManyByUserIdsAsync(IEnumerable<Guid> userIds);

        Task UpsertAsync(SecuritySetting updatedSettings);
    }
}
