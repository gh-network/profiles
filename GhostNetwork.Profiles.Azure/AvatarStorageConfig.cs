using Azure.Storage.Blobs;

namespace GhostNetwork.Profiles.Azure
{
    public class AvatarStorageConfig
    {
        public AvatarStorageConfig(BlobServiceClient blobServiceClient, string containerName)
        {
            BlobServiceClient = blobServiceClient;
            ContainerName = containerName;
        }

        public BlobServiceClient BlobServiceClient { get; }

        public string ContainerName { get; }
    }
}
