using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GhostNetwork.Profiles.WorkExperiences
{
    public interface IWorkExperienceStorage
    {
        Task<WorkExperience?> FindByIdAsync(Guid id);

        Task<Guid> InsertAsync(WorkExperience workExperience);

        Task UpdateAsync(WorkExperience workExperience);

        Task DeleteAllExperienceInProfileAsync(Guid profileId);

        Task<IEnumerable<WorkExperience>> GetAllExperienceByProfileIdAsync(Guid profileId);

        Task DeleteAsync(Guid id);
    }
}
