using System.IO;
using System.Threading.Tasks;
using Azure.Storage.Blobs;
using GhostNetwork.Profiles.Avatars;

namespace GhostNetwork.Profiles.Azure
{
    public class AvatarStorage : IAvatarStorage
    {
        private readonly BlobServiceClient blobServiceClient;
        private readonly string containerName;

        public AvatarStorage(BlobServiceClient blobServiceClient, string containerName)
        {
            this.blobServiceClient = blobServiceClient;
            this.containerName = containerName;
        }

        public async Task<string> UploadAsync(Stream stream, string fileName)
        {
            var containerClient = blobServiceClient.GetBlobContainerClient(containerName);
            var blobClient = containerClient.GetBlobClient(fileName);
            await blobClient.UploadAsync(stream);
            return containerClient.Uri.AbsoluteUri + "/" + fileName;
        }

        public async Task<bool> DeleteAsync(string avatarUrl)
        {
            var containerClient = blobServiceClient.GetBlobContainerClient(containerName);
            var arr = avatarUrl.Split(new char[] {'/'});
            var result = await containerClient.DeleteBlobIfExistsAsync(arr[arr.Length - 1]);
            if (result)
            {
                return true;
            }

            return false;
        }
    }
}
