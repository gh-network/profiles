using System.Threading.Tasks;

namespace GhostNetwork.Profiles
{
    public interface IProfileStorage
    {
        Task<Profile> FindByIdAsync(string id);

        Task<string> InsertAsync(Profile profile);

        Task UpdateAsync(string id, Profile updatedProfile);

        Task DeleteAsync(string id);
    }
}
