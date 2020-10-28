using Microsoft.EntityFrameworkCore;

namespace GhostNetwork.Profiles.MsSQL
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<ProfileEntity> Profiles { get; set; }

        public DbSet<WorkExperienceEntity> WorkExperience { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }
    }
}
