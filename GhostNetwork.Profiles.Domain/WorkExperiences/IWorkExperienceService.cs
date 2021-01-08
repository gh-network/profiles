using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain;

namespace GhostNetwork.Profiles.WorkExperiences
{
    public interface IWorkExperienceService
    {
        Task<WorkExperience> GetByIdAsync(Guid id);

        Task<(DomainResult, Guid)> CreateAsync(string companyName, string description, DateTimeOffset? startWork, DateTimeOffset? finishWork, Guid profileId);

        Task<DomainResult> UpdateAsync(Guid id, string companyName, string description, DateTimeOffset? startWork, DateTimeOffset? finishWork);

        Task DeleteAsync(Guid id);

        Task<IEnumerable<WorkExperience>> FindByProfileId(Guid profileId);
    }

    public class WorkExperienceService : IWorkExperienceService
    {
        private readonly IWorkExperienceStorage experienceStorage;
        private readonly IProfileStorage profileStorage;
        private readonly IValidator<WorkExperienceContext> validator;

        public WorkExperienceService(IWorkExperienceStorage experienceStorage, IProfileStorage profileStorage, IValidator<WorkExperienceContext> validator)
        {
            this.experienceStorage = experienceStorage;
            this.profileStorage = profileStorage;
            this.validator = validator;
        }

        public async Task<(DomainResult, Guid)> CreateAsync(string companyName,
            string description, DateTimeOffset? startWork, DateTimeOffset? finishWork, Guid profileId)
        {
            var result = validator.Validate(new WorkExperienceContext(companyName, description, startWork, finishWork));
            if (!result.Successed)
            {
                return (result, default);
            }

            if (await profileStorage.FindByIdAsync(profileId) == null)
            {
                return (DomainResult.Error("Profile not found."), default);
            }

            var workExperience = new WorkExperience(default, companyName, description, startWork, finishWork, profileId);
            var id = await experienceStorage.InsertAsync(workExperience);
            return (DomainResult.Success(), id);
        }

        public async Task DeleteAsync(Guid id)
        {
            await experienceStorage.DeleteAsync(id);
        }

        public async Task<IEnumerable<WorkExperience>> FindByProfileId(Guid profileId)
        {
            var workExperiences = await experienceStorage.GetAllExperienceByProfileIdAsync(profileId);
            return workExperiences.OrderByDescending(x => x.StartWork.HasValue).ThenBy(x => x.StartWork).ToList();
        }

        public async Task<WorkExperience> GetByIdAsync(Guid id)
        {
            return await experienceStorage.FindByIdAsync(id);
        }

        public async Task<DomainResult> UpdateAsync(Guid id,
            string companyName, string description, DateTimeOffset? startWork, DateTimeOffset? finishWork)
        {
            var result = validator.Validate(new WorkExperienceContext(companyName, description, startWork, finishWork));
            if (!result.Successed)
            {
                return result;
            }

            var workExp = await experienceStorage.FindByIdAsync(id);

            if (workExp == null)
            {
                return DomainResult.Error("Work experience not found.");
            }

            workExp.Update(companyName, description, startWork, finishWork);
            await experienceStorage.UpdateAsync(workExp);
            return DomainResult.Success();
        }
    }
}
