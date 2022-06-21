using System;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using GhostNetwork.EventBus;
using GhostNetwork.EventBus.AzureServiceBus;
using GhostNetwork.EventBus.RabbitMq;
using GhostNetwork.Profiles.Api.Helpers.OpenApi;
using GhostNetwork.Profiles.Friends;
using GhostNetwork.Profiles.MongoDb;
using GhostNetwork.Profiles.SecuritySettings;
using GhostNetwork.Profiles.WorkExperiences;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using MongoDB.Driver;
using RabbitMQ.Client;
using Swashbuckle.AspNetCore.Filters;

namespace GhostNetwork.Profiles.Api
{
    public class Startup
    {
        private const string DefaultDbName = "profiles";

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        private IConfiguration Configuration { get; }

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
                options.DocumentFilter<EnumDocumentFilter<Access>>();

                options.IncludeXmlComments(XmlPathProvider.XmlPath);
            });

            switch (Configuration["EVENTHUB_TYPE"]?.ToLower())
            {
                case "rabbit":
                    services.AddSingleton<IEventBus>(provider => new RabbitMqEventBus(
                        new ConnectionFactory { Uri = new Uri(Configuration["RABBIT_CONNECTION"]) },
                        new EventBus.RabbitMq.HandlerProvider(provider)));
                    break;
                case "servicebus":
                    services.AddSingleton<IEventBus>(provider => new AzureServiceEventBus(
                        Configuration["SERVICEBUS_CONNECTION"],
                        new EventBus.AzureServiceBus.HandlerProvider(provider)));
                    break;
                default:
                    services.AddSingleton<IEventBus, NullEventBus>();
                    break;
            }

            services.AddScoped(_ =>
            {
                var connectionString = Configuration["MONGO_CONNECTION"];
                var mongoUrl = MongoUrl.Create(connectionString);
                var client = new MongoClient(mongoUrl);
                return new MongoDbContext(client.GetDatabase(mongoUrl.DatabaseName ?? DefaultDbName));
            });

            services.AddScoped<IProfileStorage, MongoProfileStorage>();
            services.AddScoped<IProfileService, ProfileService>();
            services.AddScoped<IValidator<ProfileContext>, ProfileValidator>();

            services.AddScoped<IWorkExperienceStorage, MongoWorkExperienceStorage>();
            services.AddScoped<IWorkExperienceService, WorkExperienceService>();
            services.AddScoped<IValidator<WorkExperienceContext>, WorkExperienceValidator>();

            services.AddScoped<ISecuritySettingStorage, SecuritySettingsStorage>();
            services.AddScoped<ISecuritySettingService, SecuritySettingsService>();

            services.AddScoped<IRelationsService, MongoRelationsStorage>();
            services.AddScoped<IAccessResolver, AccessResolver>();

            services.AddControllers()
                .AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
                });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IServiceProvider provider)
        {
            if (env.IsDevelopment())
            {
                app
                    .UseSwagger()
                    .UseSwaggerUI(config => { config.SwaggerEndpoint("/swagger/v1/swagger.json", "Profiles Api V1"); });

                app.UseCors(builder => builder.AllowAnyHeader()
                    .AllowAnyMethod()
                    .AllowAnyOrigin());

                var profileStorage = provider.GetRequiredService<IProfileStorage>();
                SeedAsync(profileStorage).GetAwaiter().GetResult();
            }

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

        private async Task SeedAsync(IProfileStorage profileStorage)
        {
            var alice = Guid.Parse("3fa85f64-5717-4562-b3fc-2c963f66af76");
            if (await profileStorage.FindByIdAsync(alice) == null)
            {
                await profileStorage.InsertAsync(new Profile(alice, "Alice", "Alice", "Female", null, null, null));
            }

            var bob = Guid.Parse("3fa85f64-5717-4562-b3fc-2c963f66af77");
            if (await profileStorage.FindByIdAsync(bob) == null)
            {
                await profileStorage.InsertAsync(new Profile(bob, "Bob", "Bob", "Male", null, null, null));
            }
        }
    }
}
