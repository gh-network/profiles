using Azure.Storage.Blobs;
using GhostNetwork.Profiles.Avatars;
using GhostNetwork.Profiles.Azure;
using GhostNetwork.Profiles.MsSQL;
using GhostNetwork.Profiles.WorkExperiences;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;

namespace GhostNetwork.Profiles.Api
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Profiles API V1",
                    Version = "1.0.0"
                });
            });
            services.AddSwaggerGen();
            services.AddControllers();
            services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(Configuration.GetConnectionString("ProfilesConnection")));
            services.AddScoped<IProfileService, ProfileService>();
            services.AddScoped<IProfileStorage, ProfileStorage>();
            services.AddScoped<IValidator<ProfileContext>, ProfileValidator>();
            services.AddScoped<IWorkExperienceService, WorkExperienceService>();
            services.AddScoped<IWorkExperienceStorage, WorkExperienceStorage>();
            services.AddScoped<IValidator<WorkExperienceContext>, WorkExperienceValidator>();

            services.AddScoped<IAvatarStorage>(provider => new AvatarStorage(
                new BlobServiceClient(
                    Configuration.GetValue<string>("AzureBlobStorageConnectionString")), 
                    Configuration.GetValue<string>("Container")));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseSwagger().UseSwaggerUI(config =>
                {
                    config.SwaggerEndpoint("/swagger/v1/swagger.json", "Profiles Api V1");
                });
            }

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
