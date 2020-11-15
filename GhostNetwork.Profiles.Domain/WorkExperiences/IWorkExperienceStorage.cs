using System.Collections.Generic;
using System.Threading.Tasks;

namespace GhostNetwork.Profiles.WorkExperiences
{
    public interface IWorkExperienceStorage
    {
        Task<WorkExperience> FindByIdAsync(string id);

        Task<string> InsertAsync(WorkExperience workExperience);

        Task UpdateAsync(WorkExperience workExperience);

        Task DeleteAllExperienceInProfileAsync(string profileId);

        Task<IList<WorkExperience>> GetAllExperienceByProfileIdAsync(string profileId);

        Task DeleteAsync(string id);
    }
}
