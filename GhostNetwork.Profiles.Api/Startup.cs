using System;
using System.Text.Json.Serialization;
using GhostNetwork.Profiles.Api.Helpers.OpenApi;
using GhostNetwork.Profiles.MongoDb;
using GhostNetwork.Profiles.MsSQL;
using GhostNetwork.Profiles.WorkExperiences;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using MongoDB.Driver;
using Swashbuckle.AspNetCore.Filters;

namespace GhostNetwork.Profiles.Api
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
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Profiles API V1",
                    Version = "1.0.0"
                });

                options.OperationFilter<OperationIdFilter>();
                options.OperationFilter<AddResponseHeadersFilter>();
            });

            services.AddScoped(provider =>
            {
                var client = new MongoClient($"mongodb://{Configuration["MONGO_ADDRESS"]}/profiles");
                return new MongoDbContext(client.GetDatabase("profiles"));
            });

            services.AddScoped<IProfileStorage, MongoProfileStorage>();
            services.AddScoped<IProfileService, ProfileService>();
            services.AddScoped<IValidator<ProfileContext>, ProfileValidator>();

            services.AddScoped<IWorkExperienceStorage, MongoWorkExperienceStorage>();
            services.AddScoped<IWorkExperienceService, WorkExperienceService>();
            services.AddScoped<IValidator<WorkExperienceContext>, WorkExperienceValidator>();

            services.AddControllers()
                .AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
                });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app
                    .UseSwagger()
                    .UseSwaggerUI(config =>
                    {
                        config.SwaggerEndpoint("/swagger/v1/swagger.json", "Profiles Api V1");
                    });

                app.UseCors(builder => builder.AllowAnyHeader()
                    .AllowAnyMethod()
                    .AllowAnyOrigin());
            }

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
