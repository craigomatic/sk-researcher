using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Abstractions;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Configurations;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Enums;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Microsoft.SemanticKernel;
using sk_researcher.common;
using System.Text.Json;

var builtConfig = null as IConfigurationRoot;

var host = new HostBuilder()
    .ConfigureFunctionsWorkerDefaults(defaults =>
    {
        defaults.Serializer = new Azure.Core.Serialization.JsonObjectSerializer(
            new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                PropertyNameCaseInsensitive = true
            });
    })
    .ConfigureAppConfiguration(configuration =>
    {
        var config = configuration.SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("local.settings.json", optional: true, reloadOnChange: true);

        builtConfig = config.Build();

    })
    .ConfigureServices(services =>
    {
        var sk = Kernel.Builder.Configure(
                embeddingConfig: null,
                completionConfig: builtConfig.GetRequiredSection("CompletionConfig").Get<ModelConfig>());
        
        var appConfig = builtConfig.GetRequiredSection("AppConfig").Get<AppConfig>();
        var t = sk.LoadResearchPluginsAsync(appConfig!);
        t.Wait();

        services.AddScoped<IResearchService, ResearchService>(_ => new ResearchService(sk));

        services.AddSingleton<IOpenApiConfigurationOptions>(_ =>
        {
            var options = new OpenApiConfigurationOptions()
            {
                Info = new OpenApiInfo()
                {
                    Version = "1.0.0",
                    Title = "Researcher Plugin",
                    Description = "This plugin is capable of scraping webpages."
                },
                Servers = DefaultOpenApiConfigurationOptions.GetHostNames(),
                OpenApiVersion = OpenApiVersionType.V3,
                IncludeRequestingHostName = true,
                ForceHttps = false,
                ForceHttp = false,
            };

            return options;
        });
    })
    .Build();

host.Run();
