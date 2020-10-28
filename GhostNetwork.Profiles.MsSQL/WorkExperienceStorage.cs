using System.Threading.Tasks;
using GhostNetwork.Profiles.Domain;

namespace GhostNetwork.Profiles.MsSQL
{
    public class WorkExperienceStorage : IWorkExperienceStorage
    {
        private readonly ApplicationDbContext context;

        public WorkExperienceStorage(ApplicationDbContext context)
        {
            this.context = context;
        }

        public async Task DeleteAsync(long id)
        {
            var experience = await context.WorkExperience.FindAsync(id);
            context.WorkExperience.Remove(experience);
            await context.SaveChangesAsync();
        }

        public async Task<WorkExperience> FindByIdAsync(long id)
        {
            var workExperience = await context.WorkExperience.FindAsync(id);
            if (workExperience == null)
            {
                return null;
            }

            return ToDomain(workExperience);
        }

        public async Task<long> InsertAsync(WorkExperience workExperience)
        {
            var newWorkExperience = new WorkExperienceEntity
            {
                Id = workExperience.Id,
                ProfileId = workExperience.ProfileId,
                FinishWork = workExperience.FinishWork,
                StartWork = workExperience.StartWork,
                CompanyName = workExperience.CompanyName
            };

            await context.WorkExperience.AddAsync(newWorkExperience);
            await context.SaveChangesAsync();

            return newWorkExperience.Id;
        }

        public async Task UpdateAsync(long id, WorkExperience workExperience)
        {
            var experience = await context.WorkExperience.FindAsync(workExperience);
            experience.CompanyName = workExperience.CompanyName;
            experience.FinishWork = workExperience.FinishWork;
            experience.StartWork = workExperience.StartWork;

            context.WorkExperience.Update(experience);

            await context.SaveChangesAsync();
        }

        private static WorkExperience ToDomain(WorkExperienceEntity entity)
        {
            return new WorkExperience(
                entity.Id,
                entity.ProfileId,
                entity.FinishWork,
                entity.StartWork,
                entity.CompanyName);
        }
    }
}
