using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Driver;

namespace GhostNetwork.Profiles.MongoDb
{
    public class MongoProfileStorage : IProfileStorage
    {
        private readonly MongoDbContext context;

        public MongoProfileStorage(MongoDbContext context)
        {
            this.context = context;
        }

        public async Task<IEnumerable<Profile>> SearchByIdsAsync(IEnumerable<Guid> ids)
        {
            var filter = Builders<ProfileEntity>.Filter.In(x => x.Id, ids);
            
            var profiles = await context.Profiles.Aggregate()
                .Match(filter)
                .ToListAsync();

            return profiles.Select(ToDomain);
        }

        public async Task<Profile> FindByIdAsync(Guid id)
        {
            var filter = Builders<ProfileEntity>.Filter.Eq(p => p.Id, id);
            var entity = await context.Profiles.Find(filter).FirstOrDefaultAsync();

            return entity == null ? null : ToDomain(entity);
        }

        public async Task<Guid> InsertAsync(Profile profile)
        {
            var entity = new ProfileEntity
            {
                Id = profile.Id,
                FirstName = profile.FirstName,
                LastName = profile.LastName,
                Gender = profile.Gender,
                DateOfBirth = profile.DateOfBirth?.ToUnixTimeMilliseconds(),
                City = profile.City
            };

            await context.Profiles.InsertOneAsync(entity);

            return entity.Id;
        }

        public async Task UpdateAsync(Guid id, Profile updatedProfile)
        {
            var filter = Builders<ProfileEntity>.Filter.Eq(p => p.Id, updatedProfile.Id);

            var update = Builders<ProfileEntity>.Update
                .Set(s => s.FirstName, updatedProfile.FirstName)
                .Set(s => s.LastName, updatedProfile.LastName)
                .Set(s => s.Gender, updatedProfile.Gender)
                .Set(s => s.DateOfBirth, updatedProfile.DateOfBirth?.ToUnixTimeMilliseconds())
                .Set(s => s.City, updatedProfile.City);

            await context.Profiles.UpdateOneAsync(filter, update);
        }

        public async Task DeleteAsync(Guid id)
        {
            var filter = Builders<ProfileEntity>.Filter.Eq(p => p.Id, id);

            await context.Profiles.DeleteOneAsync(filter);
        }

        private static Profile ToDomain(ProfileEntity entity)
        {
            DateTimeOffset? dateOfBirth = null;
            if (entity.DateOfBirth.HasValue)
            {
                dateOfBirth = DateTimeOffset.FromUnixTimeMilliseconds(entity.DateOfBirth.Value);
            }

            return new Profile(
                entity.Id,
                entity.FirstName,
                entity.LastName,
                entity.Gender,
                dateOfBirth,
                entity.City);
        }
    }
}
