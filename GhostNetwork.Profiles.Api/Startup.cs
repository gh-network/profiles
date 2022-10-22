using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using GhostNetwork.EventBus;
using GhostNetwork.EventBus.AzureServiceBus;
using GhostNetwork.EventBus.RabbitMq;
using GhostNetwork.Profiles.Api.Helpers;
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
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using MongoDB.Driver;
using MongoDB.Driver.Core.Events;
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
                options.SwaggerDoc("api", new OpenApiInfo
                {
                    Title = "GhostNetwork.Profiles",
                    Version = "1.4.2"
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

            services.AddSingleton(provider =>
            {
                var connectionString = Configuration["MONGO_CONNECTION"];
                var mongoUrl = MongoUrl.Create(connectionString);
                var settings = MongoClientSettings.FromUrl(mongoUrl);
                settings.ClusterConfigurator = cb =>
                {
                    cb.Subscribe<CommandStartedEvent>(_ =>
                    {
                        var logger = provider.GetRequiredService<ILogger<MongoDbContext>>();
                        using var scope = logger.BeginScope(new Dictionary<string, object>
                        {
                            ["type"] = "outgoing:mongodb"
                        });

                        logger.LogInformation("Mongodb query started");
                    });

                    cb.Subscribe<CommandSucceededEvent>(e =>
                    {
                        var logger = provider.GetRequiredService<ILogger<MongoDbContext>>();
                        using var scope = logger.BeginScope(new Dictionary<string, object>
                        {
                            ["type"] = "outgoing:mongodb",
                            ["elapsedMilliseconds"] = e.Duration.Milliseconds
                        });

                        logger.LogInformation("Mongodb query finished");
                    });

                    cb.Subscribe<CommandFailedEvent>(e =>
                    {
                        var logger = provider.GetRequiredService<ILogger<MongoDbContext>>();
                        using var scope = logger.BeginScope(new Dictionary<string, object>
                        {
                            ["type"] = "outgoing:mongodb",
                            ["elapsedMilliseconds"] = e.Duration.Milliseconds
                        });

                        logger.LogInformation("Mongodb query failed");
                    });
                };
                return new MongoClient(settings);
            });
            services.AddScoped(provider =>
            {
                var mongoUrl = MongoUrl.Create(Configuration["MONGO_CONNECTION"]);
                return new MongoDbContext(provider.GetRequiredService<MongoClient>().GetDatabase(mongoUrl.DatabaseName ?? DefaultDbName));
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

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IServiceProvider provider, IHostApplicationLifetime hostApplicationLifetime)
        {
            app.UseMiddleware<LoggingMiddleware>();

            if (env.IsDevelopment())
            {
                app
                    .UseSwagger()
                    .UseSwaggerUI(config => { config.SwaggerEndpoint("/swagger/api/swagger.json", "Profiles Api V1"); });

                app.UseCors(builder => builder.AllowAnyHeader()
                    .AllowAnyMethod()
                    .AllowAnyOrigin());
            }

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            hostApplicationLifetime.ApplicationStarted.Register(() =>
            {
                var scope = app.ApplicationServices.CreateScope();
                var mongoDb = scope.ServiceProvider.GetService<MongoDbContext>();
                mongoDb?.ConfigureAsync();
            });
        }
    }
}
