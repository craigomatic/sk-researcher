using Microsoft.SemanticKernel;
using Microsoft.Extensions.Configuration;
using sk_researcher.common;

var configBuilder = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
            .AddJsonFile("appsettings.development.json", optional: true, reloadOnChange: true)
            .Build();

var embeddingConfig = configBuilder.GetRequiredSection("EmbeddingConfig").Get<ModelConfig>();
var completionConfig = configBuilder.GetRequiredSection("CompletionConfig").Get<ModelConfig>();
var appConfig = configBuilder.GetRequiredSection("AppConfig").Get<AppConfig>();

var sk = Kernel.Builder.WithLoggerFactory(ConsoleLogger.LogFactory).Configure(embeddingConfig, completionConfig);
await sk.LoadResearchPluginsAsync(appConfig!);

var topic = "Help me understand the current status of the superconductivity/LK-99 paper. Have they discovered something that will change the world? ";

var researchService = new ResearchService(sk);
var result = await researchService.ResearchAsync(topic);

Console.WriteLine(result.Content);