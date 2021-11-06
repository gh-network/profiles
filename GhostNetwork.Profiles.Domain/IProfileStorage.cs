using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GhostNetwork.Profiles
{
    public interface IProfileStorage
    {
        Task<IEnumerable<Profile>> SearchByIdsAsync(IEnumerable<Guid> ids);

        Task<Profile> FindByIdAsync(Guid id);

        Task<Guid> InsertAsync(Profile profile);

        Task UpdateAsync(Guid id, Profile updatedProfile);

        Task DeleteAsync(Guid id);
    }
}
