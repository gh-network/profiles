using System.Collections.Generic;
using System.Threading.Tasks;

namespace GhostNetwork.Profiles.WorkExperiences
{
    public interface IWorkExperienceStorage
    {
        Task<WorkExperience> FindByIdAsync(long id);

        Task<long> InsertAsync(WorkExperience workExperience);

        Task UpdateAsync(long id, WorkExperience workExperience);

        Task DeleteAllExperienceInProfile(long profileId);

        Task<IEnumerable<WorkExperience>> GetAllExperienceByProfileId(long profileId);

        Task DeleteAsync(long id);
    }
}
