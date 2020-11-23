using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Azure.Storage.Blobs;
using Domain;
using GhostNetwork.Profiles.Avatars;

namespace GhostNetwork.Profiles.MsSQL
{
    public class AvatarStorage : IAvatarStorage
    {
        private readonly BlobServiceClient blobServiceClient;
        private readonly string containerName;
        private readonly ApplicationDbContext context;

        public AvatarStorage(BlobServiceClient blobServiceClient, string containerName, ApplicationDbContext context)
        {
            this.blobServiceClient = blobServiceClient;
            this.containerName = containerName;
            this.context = context;
        }

        public async Task UploadAsync(Stream stream, string fileName, string profileId)
        {
            if (!Guid.TryParse(profileId, out var gId))
            {
                return;
            }


            var containerClient = blobServiceClient.GetBlobContainerClient(containerName);
            var blobClient = containerClient.GetBlobClient(fileName);
            await blobClient.UploadAsync(stream);
            var profile = await context.Profiles.FindAsync(gId);
            if (profile.AvatarUrl != null)
            {
                await DeleteAsync(profileId);
            }
            profile.AvatarUrl = containerClient.Uri.AbsoluteUri + "/" + fileName;
            context.Profiles.Update(profile);
            await context.SaveChangesAsync();
        }

        public async Task DeleteAsync(string profileId)
        {
            if (!Guid.TryParse(profileId, out var gId))
            {
                return;
            }

            var profile = await context.Profiles.FindAsync(gId);
            var containerClient = blobServiceClient.GetBlobContainerClient(containerName);
            var arr = profile.AvatarUrl.Split(new char[] { '/' });
            var result = await containerClient.DeleteBlobIfExistsAsync(arr[arr.Length-1]);
            profile.AvatarUrl = null;
            await context.SaveChangesAsync();
        }
    }
}
