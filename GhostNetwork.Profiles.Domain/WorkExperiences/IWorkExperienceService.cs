using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain;

namespace GhostNetwork.Profiles.WorkExperiences
{
    public interface IWorkExperienceService
    {
        Task<WorkExperience> GetByIdAsync(string id);

        Task<(DomainResult, string)> CreateAsync(string companyName, string description, DateTimeOffset? startWork, DateTimeOffset? finishWork, string profileId);

        Task<DomainResult> UpdateAsync(string id, string companyName, string description, DateTimeOffset? startWork, DateTimeOffset? finishWork);

        Task DeleteAsync(string id);

        Task<IList<WorkExperience>> FindByProfileId(string profileId);
    }

    public class WorkExperienceService : IWorkExperienceService
    {
        private readonly IWorkExperienceStorage experienceStorage;
        private readonly IProfileStorage profileStorage;
        private readonly IValidator<WorkExperienceContext> validator;
        private readonly ISort<WorkExperience> workExperienceSort;

        public WorkExperienceService(IWorkExperienceStorage experienceStorage, IProfileStorage profileStorage, IValidator<WorkExperienceContext> validator, ISort<WorkExperience> workExperienceSort)
        {
            this.experienceStorage = experienceStorage;
            this.profileStorage = profileStorage;
            this.validator = validator;
            this.workExperienceSort = workExperienceSort;
        }

        public async Task<(DomainResult, string)> CreateAsync(string companyName, string description, DateTimeOffset? startWork, DateTimeOffset? finishWork, string profileId)
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

            var workExperience = new WorkExperience(default, profileId, companyName, description, startWork, finishWork);
            var id = await experienceStorage.InsertAsync(workExperience);
            return (DomainResult.Success(), id);
        }

        public async Task DeleteAsync(string id)
        {
            await experienceStorage.DeleteAsync(id);
        }

        public async Task<IList<WorkExperience>> FindByProfileId(string profileId)
        {
            var workExperiences = await experienceStorage.GetAllExperienceByProfileIdAsync(profileId);
            return workExperienceSort.Sort(workExperiences);
        }

        public async Task<WorkExperience> GetByIdAsync(string id)
        {
            return await experienceStorage.FindByIdAsync(id);
        }

        public async Task<DomainResult> UpdateAsync(string id, string companyName, string description, DateTimeOffset? startWork, DateTimeOffset? finishWork)
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
