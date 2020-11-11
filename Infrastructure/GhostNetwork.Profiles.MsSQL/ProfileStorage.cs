using System;
using System.Threading.Tasks;

namespace GhostNetwork.Profiles.MsSQL
{
    public class ProfileStorage : IProfileStorage
    {
        private readonly ApplicationDbContext context;

        public ProfileStorage(ApplicationDbContext context)
        {
            this.context = context;
        }

        public async Task<Profile> FindByIdAsync(string id)
        {
            if (!Guid.TryParse(id, out var gId))
            {
                throw new ArgumentException(nameof(id));
            }

            var profile = await context.Profiles.FindAsync(gId);

            return profile == null
                ? null
                : ToDomain(profile);
        }

        public async Task<string> InsertAsync(Profile profile)
        {
            var profileEntity = new ProfileEntity
            {
                City = profile.City,
                DateOfBirth = profile.DateOfBirth,
                FirstName = profile.FirstName,
                LastName = profile.LastName,
                Gender = profile.Gender
            };

            await context.AddAsync(profileEntity);
            await context.SaveChangesAsync();

            return profileEntity.Id.ToString();
        }

        public async Task UpdateAsync(string id, Profile updatedProfile)
        {
            if (!Guid.TryParse(id, out var gId))
            {
                throw new ArgumentException(nameof(id));
            }

            var profileEntity = await context.Profiles.FindAsync(gId);

            profileEntity.City = updatedProfile.City;
            profileEntity.FirstName = updatedProfile.FirstName;
            profileEntity.LastName = updatedProfile.LastName;
            profileEntity.Gender = updatedProfile.Gender;
            profileEntity.DateOfBirth = updatedProfile.DateOfBirth;

            context.Profiles.Update(profileEntity);
            await context.SaveChangesAsync();
        }

        public async Task DeleteAsync(string id)
        {
            if (!Guid.TryParse(id, out var gId))
            {
                throw new ArgumentException(nameof(id));
            }

            var profileEntity = await context.Profiles.FindAsync(gId);
            context.Profiles.Remove(profileEntity);
            await context.SaveChangesAsync();
        }

        private static Profile ToDomain(ProfileEntity entity)
        {
            return new Profile(
                entity.Id.ToString(),
                entity.FirstName,
                entity.LastName,
                entity.Gender,
                entity.DateOfBirth,
                entity.City);
        }
    }
}
