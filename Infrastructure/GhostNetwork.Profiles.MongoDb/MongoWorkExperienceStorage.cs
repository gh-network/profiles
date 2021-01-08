using GhostNetwork.Profiles.WorkExperiences;
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

        public async Task<WorkExperience> FindByIdAsync(Guid id)
        {
            var filter = Builders<WorkExperienceEntity>.Filter.Eq(p => p.Id, id);
            var entity = await context.WorkExperience.Find(filter).FirstOrDefaultAsync();

            return entity == null ? null : ToDomain(entity);
        }

        public async Task<IEnumerable<WorkExperience>> GetAllExperienceByProfileIdAsync(Guid profileId)
        {
            var filter = Builders<WorkExperienceEntity>.Filter.Eq(x => x.ProfileId, profileId);

            var entities = await context.WorkExperience.Find(filter).ToListAsync();

            return (entities.Select(ToDomain).ToList());
        }

        public async Task<Guid> InsertAsync(WorkExperience workExperience)
        {
            var entity = new WorkExperienceEntity
            {
                CompanyName = workExperience.CompanyName,
                Description = workExperience.Description,
                StartWork = workExperience.StartWork?.ToUnixTimeMilliseconds(),
                FinishWork = workExperience.FinishWork?.ToUnixTimeMilliseconds(),
                ProfileId = workExperience.ProfileId
            };

            await context.WorkExperience.InsertOneAsync(entity);
            return entity.Id;
        }

        public async Task UpdateAsync(WorkExperience workExperience)
        {
            var filter = Builders<WorkExperienceEntity>.Filter.Eq(p => p.Id, workExperience.Id);

            var update = Builders<WorkExperienceEntity>.Update.Set(s => s.CompanyName, workExperience.CompanyName)
                .Set(s => s.Description, workExperience.Description)
                .Set(s => s.StartWork, workExperience.StartWork?.ToUnixTimeMilliseconds())
                .Set(s => s.FinishWork, workExperience.FinishWork?.ToUnixTimeMilliseconds());

            await context.WorkExperience.UpdateOneAsync(filter, update);
        }

        public async Task DeleteAllExperienceInProfileAsync(Guid profileId)
        {
            await context.WorkExperience.DeleteManyAsync(s => s.ProfileId == profileId);
        }

        public async Task DeleteAsync(Guid id)
        {
            var filter = Builders<WorkExperienceEntity>.Filter.Eq(p => p.Id, id);

            await context.WorkExperience.DeleteOneAsync(filter);
        }

        private static WorkExperience ToDomain(WorkExperienceEntity entity)
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
                entity.Id,
                entity.CompanyName,
                entity.Description,
                startWork,
                finishWork,
                entity.ProfileId
                );
        }
    }
}
