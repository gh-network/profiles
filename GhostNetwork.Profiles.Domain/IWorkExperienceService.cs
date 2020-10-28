using System;
using System.Threading;
using System.Threading.Tasks;
using Domain;

namespace GhostNetwork.Profiles.Domain
{
    public interface IWorkExperienceService
    {
        Task<WorkExperience> GetByIdAsync(long id);

        Task<(DomainResult, long)> CreateAsync(string companyName, DateTime startWork, DateTime? finishWork, long profileId);

        Task<DomainResult> UpdateAsync(long id, string companyName, DateTime startWork, DateTime? finishWork, long profileId);

        Task DeleteAsync(long id);
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

        public async Task<(DomainResult, long)> CreateAsync(string companyName, DateTime startWork, DateTime? finishWork, long profileId)
        {
            var result = validator.Validate(new WorkExperienceContext(companyName, startWork, finishWork));
            if (!result.Successed)
            {
                return (result, -1);
            }

            if (await profileStorage.FindByIdAsync(profileId) == null)
            {
                return (DomainResult.Error("Profile not found."), -1);
            }

            var workExperience = new WorkExperience(0, profileId, finishWork, startWork, companyName);
            await experienceStorage.InsertAsync(workExperience);
            return (DomainResult.Success(), workExperience.Id);
        }

        public async Task DeleteAsync(long id)
        {
            await experienceStorage.DeleteAsync(id);
        }

        public async Task<WorkExperience> GetByIdAsync(long id)
        {
            return await experienceStorage.FindByIdAsync(id);
        }

        public async Task<DomainResult> UpdateAsync(long id,string companyName, DateTime startWork, DateTime? finishWork, long profileId)
        {
            var result = validator.Validate(new WorkExperienceContext(companyName, startWork.Date, finishWork));
            if (!result.Successed)
            {
                return result;
            }

            if (await profileStorage.FindByIdAsync(profileId) == null)
            {
                return DomainResult.Error("Profile not found.");
            }

            if (await profileStorage.FindByIdAsync(id) == null)
            {
                return DomainResult.Error("Work experience not found.");
            }

            var workExperience = new WorkExperience(id, profileId, finishWork, startWork, companyName);
            await experienceStorage.UpdateAsync(id, workExperience);
            return DomainResult.Success();
        }
    }
}
