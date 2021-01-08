using System;
using System.Threading.Tasks;

namespace GhostNetwork.Profiles
{
    public interface IProfileStorage
    {
        Task<Profile> FindByIdAsync(Guid id);

        Task<Guid> InsertAsync(Profile profile);

        Task UpdateAsync(Guid id, Profile updatedProfile);

        Task DeleteAsync(Guid id);
    }
}
