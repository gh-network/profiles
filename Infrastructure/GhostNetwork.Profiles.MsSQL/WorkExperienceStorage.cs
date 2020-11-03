using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GhostNetwork.Profiles.WorkExperiences;
using Microsoft.EntityFrameworkCore;

namespace GhostNetwork.Profiles.MsSQL
{
    public class WorkExperienceStorage : IWorkExperienceStorage
    {
        private readonly ApplicationDbContext context;

        public WorkExperienceStorage(ApplicationDbContext context)
        {
            this.context = context;
        }

        public async Task DeleteAsync(string id)
        {
            var experience = await context.WorkExperience.FindAsync(id);
            context.WorkExperience.Remove(experience);
            await context.SaveChangesAsync();
        }

        public async Task<WorkExperience> FindByIdAsync(string id)
        {
            var workExperience = await context.WorkExperience.FindAsync(id);
            if (workExperience == null)
            {
                return null;
            }

            return ToDomain(workExperience);
        }

        public async Task DeleteAllExperienceInProfile(string profileId)
        {
            if (long.TryParse(profileId, out var lProfileId))
            {
                return;
            }

            if (await context.Profiles.FindAsync(profileId) != null)
            {
                var workExperience = context.WorkExperience
                    .Where(x => x.ProfileId == lProfileId)
                    .ToList();

                context.WorkExperience.RemoveRange(workExperience);
                await context.SaveChangesAsync();
            }
        }

        public async Task<string> InsertAsync(WorkExperience workExperience)
        {
            if (long.TryParse(workExperience.ProfileId, out var lProfileId))
            {
                throw new AggregateException(nameof(workExperience.ProfileId));
            }
            
            var newWorkExperience = new WorkExperienceEntity
            {
                ProfileId = lProfileId,
                FinishWork = workExperience.FinishWork,
                StartWork = workExperience.StartWork,
                CompanyName = workExperience.CompanyName
            };

            await context.WorkExperience.AddAsync(newWorkExperience);
            await context.SaveChangesAsync();

            return newWorkExperience.Id.ToString();
        }

        public async Task UpdateAsync(string id, WorkExperience workExperience)
        {
            var experience = await context.WorkExperience.FindAsync(id);
            experience.CompanyName = workExperience.CompanyName;
            experience.FinishWork = workExperience.FinishWork;
            experience.StartWork = workExperience.StartWork;

            context.WorkExperience.Update(experience);

            await context.SaveChangesAsync();
        }

        public async Task<IEnumerable<WorkExperience>> GetAllExperienceByProfileId(string profileId)
        {
            if (long.TryParse(profileId, out var lProfileId))
            {
                return Enumerable.Empty<WorkExperience>();
            }

            return await context.WorkExperience
                .Where(x => x.ProfileId == lProfileId)
                .Select(x => ToDomain(x))
                .ToListAsync();
        }

        private static WorkExperience ToDomain(WorkExperienceEntity entity)
        {
            return new WorkExperience(
                entity.Id.ToString(),
                entity.ProfileId.ToString(),
                entity.FinishWork,
                entity.StartWork,
                entity.CompanyName);
        }
    }
}
