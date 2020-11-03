using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Domain;

namespace GhostNetwork.Profiles.WorkExperiences
{
    public interface IWorkExperienceService
    {
        Task<WorkExperience> GetByIdAsync(string id);

        Task<(DomainResult, string)> CreateAsync(string companyName, DateTime startWork, DateTime? finishWork, string profileId);

        Task<DomainResult> UpdateAsync(string id, string companyName, DateTime startWork, DateTime? finishWork);

        Task DeleteAsync(string id);

        Task<IEnumerable<WorkExperience>> GetAllExperienceByProfileId(string profileId);
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

        public async Task<(DomainResult, string)> CreateAsync(string companyName, DateTime startWork,
            DateTime? finishWork, string profileId)
        {
            var result = validator.Validate(new WorkExperienceContext(companyName, startWork, finishWork));
            if (!result.Successed)
            {
                return (result, default);
            }

            if (await profileStorage.FindByIdAsync(profileId) == null)
            {
                return (DomainResult.Error("Profile not found."), default);
            }

            var workExperience = new WorkExperience(default, profileId, finishWork, startWork, companyName);
            await experienceStorage.InsertAsync(workExperience);
            return (DomainResult.Success(), workExperience.Id);
        }

        public async Task DeleteAsync(string id)
        {
            await experienceStorage.DeleteAsync(id);
        }

        public async Task<IEnumerable<WorkExperience>> GetAllExperienceByProfileId(string profileId)
        {
            return await experienceStorage.GetAllExperienceByProfileId(profileId);
        }

        public async Task<WorkExperience> GetByIdAsync(string id)
        {
            return await experienceStorage.FindByIdAsync(id);
        }

        public async Task<DomainResult> UpdateAsync(string id, string companyName, DateTime startWork,
            DateTime? finishWork)
        {
            var result = validator.Validate(new WorkExperienceContext(companyName, startWork.Date, finishWork));
            if (!result.Successed)
            {
                return result;
            }

            if (await experienceStorage.FindByIdAsync(id) == null)
            {
                return DomainResult.Error("Work experience not found.");
            }

            var workExperience = new WorkExperience(id, default, finishWork, startWork, companyName);
            await experienceStorage.UpdateAsync(id, workExperience);
            return DomainResult.Success();
        }
    }
}
