using System;
using System.Threading.Tasks;
using MongoDB.Bson;
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

        public async Task<Profile> FindByIdAsync(string id)
        {
            if (!ObjectId.TryParse(id, out var oId))
            {
                return null;
            }

            var filter = Builders<ProfileEntity>.Filter.Eq(p => p.Id, oId);
            var entity = await context.Profiles.Find(filter).FirstOrDefaultAsync();

            return entity == null ? null : ToDomain(entity);
        }

        public async Task<string> InsertAsync(Profile profile)
        {
            long? dateBirthday = null;

            if (DateTimeOffset.TryParse(profile.DateOfBirth.ToString(), out var dateB))
            {
                dateBirthday = dateB.ToUnixTimeMilliseconds();
            }

            var entity = new ProfileEntity
            {
                FirstName = profile.FirstName,
                LastName = profile.LastName,
                Gender = profile.Gender,
                DateOfBirth = dateBirthday,
                City = profile.City
            };

            await context.Profiles.InsertOneAsync(entity);

            return entity.Id.ToString();
        }

        public async Task UpdateAsync(string id, Profile updatedProfile)
        {
            if (!ObjectId.TryParse(updatedProfile.Id, out var oId))
            {
                return;
            }

            var filter = Builders<ProfileEntity>.Filter.Eq(p => p.Id, oId);

            var update = Builders<ProfileEntity>.Update.Set(s => s.FirstName, updatedProfile.FirstName)
                .Set(s => s.LastName, updatedProfile.LastName)
                .Set(s => s.Gender, updatedProfile.Gender)
                .Set(s => DateTimeOffset.FromUnixTimeMilliseconds(s.DateOfBirth.Value), updatedProfile.DateOfBirth)
                .Set(s => s.City, updatedProfile.City);

            await context.Profiles.UpdateOneAsync(filter, update);
        }

        public async Task DeleteAsync(string id)
        {
            if (!ObjectId.TryParse(id, out var oId))
            {
                return;
            }

            var filter = Builders<ProfileEntity>.Filter.Eq(p => p.Id, oId);

            await context.Profiles.DeleteOneAsync(filter);
        }

        public static Profile ToDomain(ProfileEntity entity)
        {
            DateTimeOffset? dateOfBirth = null;
            if (entity.DateOfBirth.HasValue)
            {
                dateOfBirth = DateTimeOffset.FromUnixTimeMilliseconds(entity.DateOfBirth.Value);
            }

            return new Profile(
                entity.Id.ToString(),
                entity.FirstName,
                entity.LastName,
                entity.Gender,
                dateOfBirth,
                entity.City);
        }
    }
}
