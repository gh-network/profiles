using System.Threading.Tasks;

namespace GhostNetwork.Profiles
{
    public interface IProfileStorage
    {
        Task<Profile> FindByIdAsync(long id);

        Task<long> InsertAsync(Profile profile);

        Task UpdateAsync(long id, Profile updatedProfile);

        Task DeleteAsync(long id);
    }
}
