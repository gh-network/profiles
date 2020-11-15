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
            if (!Guid.TryParse(id, out var lId))
            {
                return;
            }

            var experience = await context.WorkExperience.FindAsync(lId);
            context.WorkExperience.Remove(experience);
            await context.SaveChangesAsync();
        }

        public async Task<WorkExperience> FindByIdAsync(string id)
        {
            if (!Guid.TryParse(id, out var lId))
            {
                return null;
            }

            var workExperience = await context.WorkExperience.FindAsync(lId);
            return ToDomain(workExperience);
        }

        public async Task DeleteAllExperienceInProfileAsync(string profileId)
        {
            if (!Guid.TryParse(profileId, out var lProfileId))
            {
                return;
            }

            if (await context.Profiles.FindAsync(profileId) != null)
            {
                var workExperience = context.WorkExperience
                    .Where(x => x.ProfileId == lProfileId.ToString())
                    .ToList();

                context.WorkExperience.RemoveRange(workExperience);
                await context.SaveChangesAsync();
            }
        }

        public async Task<string> InsertAsync(WorkExperience workExperience)
        {
            if (!Guid.TryParse(workExperience.ProfileId, out var lProfileId))
            {
                return null;
            }

            long? startWork = null;
            long? finishWork = null;
            if (workExperience.StartWork.HasValue)
            {
                DateTimeOffset start = workExperience.StartWork.Value;
                startWork = start.ToUnixTimeMilliseconds();
            }

            if (workExperience.FinishWork.HasValue)
            {
                DateTimeOffset finish = workExperience.FinishWork.Value;
                finishWork = finish.ToUnixTimeMilliseconds();
            }

            var newWorkExperience = new WorkExperienceEntity
            {
                ProfileId = lProfileId.ToString(),
                Description = workExperience.Description,
                StartWork = startWork,
                FinishWork = finishWork,
                CompanyName = workExperience.CompanyName
            };

            await context.WorkExperience.AddAsync(newWorkExperience);
            await context.SaveChangesAsync();

            return newWorkExperience.Id.ToString();
        }

        public async Task UpdateAsync(WorkExperience workExperience)
        {
            if (!Guid.TryParse(workExperience.Id, out var lId))
            {
                return;
            }

            long? startWork = null;
            long? finishWork = null;
            if (workExperience.StartWork.HasValue)
            {
                DateTimeOffset start = workExperience.StartWork.Value;
                startWork = start.ToUnixTimeMilliseconds();
            }

            if (workExperience.FinishWork.HasValue)
            {
                DateTimeOffset finish = workExperience.FinishWork.Value;
                finishWork = finish.ToUnixTimeMilliseconds();
            }

            var experience = await context.WorkExperience.FindAsync(lId);
            experience.CompanyName = workExperience.CompanyName;
            experience.Description = workExperience.Description;
            experience.FinishWork = finishWork;
            experience.StartWork = startWork;

            context.WorkExperience.Update(experience);

            await context.SaveChangesAsync();
        }

        public async Task<IList<WorkExperience>> GetAllExperienceByProfileIdAsync(string profileId)
        {
            if (!Guid.TryParse(profileId, out var lProfileId))
            {
                return new List<WorkExperience>();
            }

            var workExperience = context.WorkExperience.Where(x => x.ProfileId == lProfileId.ToString());
            return workExperience.AsEnumerable().Select(ToDomain).ToList();
        }

        private static WorkExperience ToDomain(WorkExperienceEntity entity)
        {
            if (entity == null)
            {
                return null;
            }

            DateTimeOffset? startWork = null;
            DateTimeOffset? finishWork = null;
            if (entity.StartWork.HasValue)
            {
                startWork = DateTimeOffset.FromUnixTimeMilliseconds(entity.StartWork.Value);
            }

            if (entity.FinishWork.HasValue)
            {
                finishWork = DateTimeOffset.FromUnixTimeMilliseconds(entity.FinishWork.Value);
            }

            return new WorkExperience(
                entity.Id.ToString(),
                entity.ProfileId,
                entity.CompanyName,
                entity.Description,
                startWork,
                finishWork);
        }
    }
}
