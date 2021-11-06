﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Domain;
using GhostNetwork.Profiles.WorkExperiences;

namespace GhostNetwork.Profiles
{
    public interface IProfileService
    {
        Task<IEnumerable<Profile>> SearchByIdsAsync(IEnumerable<Guid> ids);

        Task<Profile> GetByIdAsync(Guid id);

        Task<(DomainResult, Profile)> CreateAsync(Guid? id, string firstName, string lastName, string gender, DateTimeOffset? dateOfBirth, string city);

        Task<DomainResult> UpdateAsync(Guid id, string firstName, string lastName, string gender, DateTimeOffset? dateOfBirth, string city);

        Task DeleteAsync(Guid id);
    }

    public class ProfileService : IProfileService
    {
        private readonly IProfileStorage profileStorage;
        private readonly IValidator<ProfileContext> profileValidator;
        private readonly IWorkExperienceStorage workExperienceStorage;

        public ProfileService(
            IProfileStorage profileStorage,
            IValidator<ProfileContext> profileValidator,
            IWorkExperienceStorage workExperienceStorage)
        {
            this.profileStorage = profileStorage;
            this.profileValidator = profileValidator;
            this.workExperienceStorage = workExperienceStorage;
        }

        public async Task<(DomainResult, Profile)> CreateAsync(Guid? id, string firstName, string lastName, string gender, DateTimeOffset? dateOfBirth, string city)
        {
            var result = profileValidator.Validate(new ProfileContext(firstName, lastName, city, dateOfBirth, gender));

            if (!result.Successed)
            {
                return (result, default);
            }

            var profile = new Profile(id ?? Guid.NewGuid(), firstName, lastName, gender, dateOfBirth, city);

            await profileStorage.InsertAsync(profile);

            return (DomainResult.Success(), profile);
        }

        public async Task DeleteAsync(Guid id)
        {
            await workExperienceStorage.DeleteAllExperienceInProfileAsync(id);
            await profileStorage.DeleteAsync(id);
        }

        public async Task<IEnumerable<Profile>> SearchByIdsAsync(IEnumerable<Guid> ids)
        {
            return await profileStorage.SearchByIdsAsync(ids);
        }

        public async Task<Profile> GetByIdAsync(Guid id)
        {
            return await profileStorage.FindByIdAsync(id);
        }

        public async Task<DomainResult> UpdateAsync(Guid id, string firstName, string lastName, string gender, DateTimeOffset? dateOfBirth, string city)
        {
            var result = profileValidator.Validate(new ProfileContext(firstName, lastName, city, dateOfBirth, gender));

            if (!result.Successed)
            {
                return result;
            }

            var profile = await profileStorage.FindByIdAsync(id);
            if (profile == null)
            {
                return DomainResult.Error("Profile not found.");
            }

            profile.Update(firstName, lastName, gender, dateOfBirth, city);
            await profileStorage.UpdateAsync(id, profile);

            return DomainResult.Success();
        }
    }
}
