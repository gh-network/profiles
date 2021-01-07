using System;
using System.Threading.Tasks;

namespace GhostNetwork.Profiles.MsSQL
{
    public class MssqlProfileStorage : IProfileStorage
    {
        private readonly ApplicationDbContext context;

        public MssqlProfileStorage(ApplicationDbContext context)
        {
            this.context = context;
        }

        public async Task<Profile> FindByIdAsync(string id)
        {
            if (!Guid.TryParse(id, out var gId))
            {
                return null;
            }

            var profile = await context.Profiles.FindAsync(gId);

            return profile == null
                ? null
                : ToDomain(profile);
        }

        public async Task<string> InsertAsync(Profile profile)
        {
            long? dateBirthday = null;
            if (DateTimeOffset.TryParse(profile.DateOfBirth.ToString(), out var dateB))
            {
                dateBirthday = dateB.ToUnixTimeMilliseconds();
            }

            var profileEntity = new ProfileEntity
            {
                City = profile.City,
                FirstName = profile.FirstName,
                LastName = profile.LastName,
                DateOfBirth = dateBirthday,
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
                return;
            }

            long? dateBirthday = null;
            if (DateTimeOffset.TryParse(updatedProfile.DateOfBirth.ToString(), out var dateB))
            {
                dateBirthday = dateB.ToUnixTimeMilliseconds();
            }

            var profileEntity = await context.Profiles.FindAsync(gId);

            profileEntity.City = updatedProfile.City;
            profileEntity.FirstName = updatedProfile.FirstName;
            profileEntity.LastName = updatedProfile.LastName;
            profileEntity.Gender = updatedProfile.Gender;
            profileEntity.DateOfBirth = dateBirthday;

            context.Profiles.Update(profileEntity);
            await context.SaveChangesAsync();
        }

        public async Task DeleteAsync(string id)
        {
            if (!Guid.TryParse(id, out var gId))
            {
                return;
            }

            var profileEntity = await context.Profiles.FindAsync(gId);
            context.Profiles.Remove(profileEntity);
            await context.SaveChangesAsync();
        }

        private static Profile ToDomain(ProfileEntity entity)
        {
            if (entity == null)
            {
                return null;
            }

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
