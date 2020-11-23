using System.IO;
using System.Threading.Tasks;

namespace GhostNetwork.Profiles.Avatars
{
    public interface IAvatarStorage
    {
        Task UploadAsync(Stream stream, string fileName, string profileId);

        Task DeleteAsync(string profileId);
    }
}
