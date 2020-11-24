using System.IO;
using System.Threading.Tasks;

namespace GhostNetwork.Profiles.Avatars
{
    public interface IAvatarStorage
    {
        Task<string> UploadAsync(Stream stream, string fileName);

        Task<bool> DeleteAsync(string avatarUrl);
    }
}
