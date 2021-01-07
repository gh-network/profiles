using GhostNetwork.Profiles.WorkExperiences;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GhostNetwork.Profiles.MongoDb
{
    public class MongoWorkExperienceStorage : IWorkExperienceStorage
    {
        private readonly MongoDbContext context;

        public MongoWorkExperienceStorage(MongoDbContext context)
        {
            this.context = context;
        }

        public async Task<WorkExperience> FindByIdAsync(string id)
        {
            if (!ObjectId.TryParse(id, out var oId))
            {
                return null;
            }

            var filter = Builders<WorkExperienceEntity>.Filter.Eq(p => p.Id, oId);
            var entity = await context.WorkExperience.Find(filter).FirstOrDefaultAsync();

            return entity == null ? null : ToDomain(entity);
        }

        public async Task<IEnumerable<WorkExperience>> GetAllExperienceByProfileIdAsync(string profileId)
        {
            var filter = Builders<WorkExperienceEntity>.Filter.Eq(x => x.ProfileId, profileId);

            var entities = await context.WorkExperience.Find(filter).ToListAsync();

            return (entities.Select(ToDomain).ToList());
        }

        public async Task<string> InsertAsync(WorkExperience workExperience)
        {
            long? startWork = null;
            long? finishWork = null;
            if (workExperience.StartWork.HasValue)
            {
                startWork = workExperience.StartWork.Value.ToUnixTimeMilliseconds();
            }
            if (workExperience.FinishWork.HasValue)
            {
                startWork = workExperience.FinishWork.Value.ToUnixTimeMilliseconds();
            }

            var entity = new WorkExperienceEntity
            {
                CompanyName = workExperience.CompanyName,
                Description = workExperience.Description,
                StartWork = startWork,
                FinishWork = finishWork,
                ProfileId = workExperience.ProfileId
            };

            await context.WorkExperience.InsertOneAsync(entity);
            return entity.Id.ToString();
        }

        public async Task UpdateAsync(WorkExperience workExperience)
        {
            if (!ObjectId.TryParse(workExperience.Id, out var oId))
            {
                return;
            }

            long? startWork = null;
            long? finishWork = null;
            if (workExperience.StartWork.HasValue)
            {
                startWork = workExperience.StartWork.Value.ToUnixTimeMilliseconds();
            }
            if (workExperience.FinishWork.HasValue)
            {
                startWork = workExperience.FinishWork.Value.ToUnixTimeMilliseconds();
            }

            var filter = Builders<WorkExperienceEntity>.Filter.Eq(p => p.Id, oId);

            var update = Builders<WorkExperienceEntity>.Update.Set(s => s.CompanyName, workExperience.CompanyName)
                .Set(s => s.Description, workExperience.Description)
                .Set(s => s.StartWork, startWork)
                .Set(s => s.FinishWork, finishWork);

            await context.WorkExperience.UpdateOneAsync(filter, update);
        }

        public async Task DeleteAllExperienceInProfileAsync(string profileId)
        {
            if (!ObjectId.TryParse(profileId, out var oId))
            {
                return;
            }

            var profile = Builders<ProfileEntity>.Filter.Eq(p => p.Id, oId);
            await context.WorkExperience.DeleteManyAsync(s => s.ProfileId == oId.ToString());
        }

        public async Task DeleteAsync(string id)
        {
            if (!ObjectId.TryParse(id, out var oId))
            {
                return;
            }

            var filter = Builders<WorkExperienceEntity>.Filter.Eq(p => p.Id, oId);

            await context.WorkExperience.DeleteOneAsync(filter);
        }

        public static WorkExperience ToDomain(WorkExperienceEntity entity)
        {
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
                entity.CompanyName,
                entity.Description,
                startWork,
                finishWork,
                entity.ProfileId
                );
        }
    }
}
