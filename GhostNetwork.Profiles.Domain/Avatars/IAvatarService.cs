using System;
using System.IO;
using System.Threading.Tasks;
using Azure.Storage.Blobs;
using Domain;

namespace GhostNetwork.Profiles.Avatars
{
    public interface IAvatarService
    {
        Task<DomainResult> UploadAsync(byte[] array, string extension, string profileId);

        Task<DomainResult> DeleteAsync(string profileId);
    }

    public class AvatarService : IAvatarService
    {
        private readonly BlobServiceClient blobServiceClient;
        private readonly string containerName;
        private readonly IProfileService profileService;

        public AvatarService(BlobServiceClient blobServiceClient, string containerName, IProfileService profileService)
        {
            this.blobServiceClient = blobServiceClient;
            this.containerName = containerName;
            this.profileService = profileService;
        }

        public async Task<DomainResult> UploadAsync(byte[] array, string extension, string profileId)
        {
            var profile = await profileService.GetByIdAsync(profileId);
            if (profile == null)
            {
                return DomainResult.Error("Profile not found.");
            }

            var containerClient = blobServiceClient.GetBlobContainerClient(containerName);
            var name = Guid.NewGuid() + extension;
            var blobClient = containerClient.GetBlobClient(name);
            await blobClient.UploadAsync(new MemoryStream(array));
            await profileService.UpdateAsync(profile.Id, profile.FirstName, profile.FirstName, profile.Gender, profile.DateOfBirth, profile.City, containerClient.Uri.AbsoluteUri + "/" + name);
            return DomainResult.Success();
        }

        public async Task<DomainResult> DeleteAsync(string profileId)
        {
            var profile = await profileService.GetByIdAsync(profileId);
            if (profile == null)
            {
                return DomainResult.Error("Profile not found.");
            }


            var containerClient = blobServiceClient.GetBlobContainerClient(containerName);
            var result = await containerClient.DeleteBlobIfExistsAsync(profile.AvatarUrl);
            if (result.Value)
            {
                return DomainResult.Success();
            }

            return DomainResult.Error("Avatar not found.");
        }
    }
}
