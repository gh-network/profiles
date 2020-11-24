using System;
using System.IO;
using System.Threading.Tasks;
using Domain;
using GhostNetwork.Profiles.Avatars;
using GhostNetwork.Profiles.WorkExperiences;

namespace GhostNetwork.Profiles
{
    public interface IProfileService
    {
        Task<Profile> GetByIdAsync(string id);

        Task<(DomainResult, string)> CreateAsync(string firstName, string lastName, string gender, DateTimeOffset? dateOfBirth, string city);

        Task<DomainResult> UpdateAsync(string id, string firstName, string lastName, string gender, DateTimeOffset? dateOfBirth, string city);

        Task DeleteAsync(string id);

        Task<DomainResult> DeleteAvatarAsync(string profileId);

        Task<DomainResult> UpdateAvatarAsync(string profileId, Stream stream, string extension);
    }

    public class ProfileService : IProfileService
    {
        private readonly IProfileStorage profileStorage;
        private readonly IValidator<ProfileContext> profileValidator;
        private readonly IWorkExperienceStorage workExperienceStorage;
        private readonly IAvatarStorage avatarStorage;

        public ProfileService(IProfileStorage profileStorage, IValidator<ProfileContext> profileValidator, IWorkExperienceStorage workExperienceStorage, IAvatarStorage avatarStorage)
        {
            this.profileStorage = profileStorage;
            this.profileValidator = profileValidator;
            this.workExperienceStorage = workExperienceStorage;
            this.avatarStorage = avatarStorage;
        }

        public async Task<(DomainResult, string)> CreateAsync(string firstName, string lastName, string gender, DateTimeOffset? dateOfBirth, string city)
        {
            var result = profileValidator.Validate(new ProfileContext(firstName, lastName, city, dateOfBirth, gender));

            if (!result.Successed)
            {
                return (result, default);
            }

            var profile = new Profile(default, firstName, lastName, gender, dateOfBirth, city, null);

            var profileId = await profileStorage.InsertAsync(profile);

            return (DomainResult.Success(), profileId);
        }

        public async Task DeleteAsync(string id)
        {
            await workExperienceStorage.DeleteAllExperienceInProfileAsync(id);
            await profileStorage.DeleteAsync(id);
        }

        public async Task<DomainResult> DeleteAvatarAsync(string profileId)
        {
            var profile = await profileStorage.FindByIdAsync(profileId);
            if (profile == null)
            {
                return DomainResult.Error("Profile not found.");
            }

            if (profile.AvatarUrl == null)
            {
                return DomainResult.Error("Avatar not found");
            }

            await avatarStorage.DeleteAsync(profileId);
            await profileStorage.DeleteAvatarAsync(profileId);
            return DomainResult.Success();
        }

        public async Task<DomainResult> UpdateAvatarAsync(string profileId, Stream stream, string extension)
        {
            var profile = await profileStorage.FindByIdAsync(profileId);
            if (profile == null)
            {
                return DomainResult.Error("Profile not found.");
            }

            var fileName = Guid.NewGuid() + extension;
            stream.Position = 0;
            var avatarUrl = await avatarStorage.UploadAsync(stream, fileName);
            if (profile.AvatarUrl != null)
            {

                await avatarStorage.DeleteAsync(profile.AvatarUrl);
            }

            await profileStorage.UpdateAvatarAsync(profileId, avatarUrl);
            return DomainResult.Success();
        }

        public async Task<Profile> GetByIdAsync(string id)
        {
            return await profileStorage.FindByIdAsync(id);
        }

        public async Task<DomainResult> UpdateAsync(string id, string firstName, string lastName, string gender, DateTimeOffset? dateOfBirth, string city)
        {
            var result = profileValidator.Validate(new ProfileContext(firstName, lastName, city, dateOfBirth, gender));

            if (result.Successed)
            {
                var profile = await profileStorage.FindByIdAsync(id);

                if (profile == null)
                {
                    return DomainResult.Error("Profile not found.");
                }

                profile.Update(firstName, lastName, gender, dateOfBirth, city);

                await profileStorage.UpdateAsync(id, profile);
                return DomainResult.Success();
            }

            return result;
        }
    }
}
