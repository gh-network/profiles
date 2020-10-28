using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GhostNetwork.Profiles.Domain
{
    public interface IWorkExperienceStorage
    {
        Task<WorkExperience> FindByIdAsync(long id);

        Task<long> InsertAsync(WorkExperience workExperience);

        Task UpdateAsync(long id, WorkExperience workExperience);

        Task DeleteAsync(long id);
    }
}
