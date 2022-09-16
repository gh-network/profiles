using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GhostNetwork.Profiles
{
    public interface IProfileStorage
    {
        Task<IEnumerable<Profile>> SearchByIdsAsync(IEnumerable<Guid> ids);

        Task<(IEnumerable<Profile>, long)> SearchByIdsAsync(int skip, int take);

        Task<Profile> FindByIdAsync(Guid id);

        Task<Guid> InsertAsync(Profile profile);

        Task UpdateAsync(Profile updatedProfile);

        Task UpdateAvatarAsync(Guid id, string avatarUrl);

        Task DeleteAsync(Guid id);
    }
}
