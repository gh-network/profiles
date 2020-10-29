using System.Threading.Tasks;
using GhostNetwork.Profiles.Domain;
using Microsoft.Extensions.DependencyInjection;

namespace GhostNetwork.Profiles.MsSQL
{
    public class ProfileStorage : IProfileStorage
    {
        private readonly ApplicationDbContext context;

        public ProfileStorage(ApplicationDbContext context)
        {
            this.context = context;
        }

        public async Task<Profile> FindByIdAsync(long id)
        {
            var profile = await context.Profiles.FindAsync(id);
            if (profile == null)
            {
                return null;
            }

            return ToDomain(profile);
        }

        public async Task<long> InsertAsync(Profile profile)
        {
            var profileEntity = new ProfileEntity
            {
                Id = profile.Id,
                City = profile.City,
                DateOfBirth = profile.DateOfBirth,
                FirstName = profile.FirstName,
                LastName = profile.LastName,
                Gender = profile.Gender
            };

            await context.AddAsync(profileEntity);
            await context.SaveChangesAsync();

            return profileEntity.Id;
        }

        public async Task UpdateAsync(long id, Profile updatedProfile)
        {
            var profileEntity = await context.Profiles.FindAsync(id);
            profileEntity.City = updatedProfile.City;
            profileEntity.FirstName = updatedProfile.FirstName;
            profileEntity.LastName = updatedProfile.LastName;
            profileEntity.Gender = updatedProfile.Gender;
            profileEntity.DateOfBirth = updatedProfile.DateOfBirth;
            context.Profiles.Update(profileEntity);
            await context.SaveChangesAsync();
        }

        public async Task DeleteAsync(long id)
        {
            var profileEntity = await context.Profiles.FindAsync(id);
            context.Profiles.Remove(profileEntity);
            await context.SaveChangesAsync();
        }

        private static Profile ToDomain(ProfileEntity entity)
        {
            return new Profile(
                entity.Id,
                entity.FirstName,
                entity.LastName,
                entity.Gender,
                entity.DateOfBirth,
                entity.City);
        }
    }
}
