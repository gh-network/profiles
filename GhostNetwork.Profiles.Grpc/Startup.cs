using GhostNetwork.Profiles.Grpc.Services;
using GhostNetwork.Profiles.MongoDb;
using GhostNetwork.Profiles.WorkExperiences;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MongoDB.Driver;

namespace GhostNetwork.Profiles.Grpc
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddScoped(provider =>
            {
                var client = new MongoClient($"mongodb://{Configuration["MONGO_ADDRESS"]}/gprofiles");
                return new MongoDbContext(client.GetDatabase("gprofiles"));
            });

            services.AddScoped<IProfileStorage, MongoProfileStorage>();
            services.AddScoped<IProfileService, ProfileService>();
            services.AddScoped<IValidator<ProfileContext>, ProfileValidator>();

            services.AddScoped<IWorkExperienceStorage, MongoWorkExperienceStorage>();
            services.AddScoped<IWorkExperienceService, WorkExperienceService>();
            services.AddScoped<IValidator<WorkExperienceContext>, WorkExperienceValidator>();

            services.AddGrpc();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGrpcService<ProfilesService>();

                endpoints.MapGet("/",
                    async context =>
                    {
                        await context.Response.WriteAsync(
                            "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");
                    });
            });
        }
    }
}