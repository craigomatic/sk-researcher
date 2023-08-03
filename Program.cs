using Microsoft.SemanticKernel;
using Microsoft.Extensions.Configuration;
using Microsoft.SemanticKernel.Planning;

var configBuilder = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
            .AddJsonFile("appsettings.development.json", optional: true, reloadOnChange: true)
            .Build();

var embeddingConfig = configBuilder.GetRequiredSection("EmbeddingConfig").Get<ModelConfig>();
var completionConfig = configBuilder.GetRequiredSection("CompletionConfig").Get<ModelConfig>();
var appConfig = configBuilder.GetRequiredSection("AppConfig").Get<AppConfig>();

var sk = Kernel.Builder.Configure(embeddingConfig, completionConfig);
await sk.LoadResearchPluginsAsync(appConfig!);

var researchRequest = "I would like to learn about superconductivity at room temperature and LK-99. Please help me out with an entry level understanding of why this potential discovery may be significant. The more detail you are able to provide the better.";
var researchContext = sk.CreateNewContext();

var stepwisePlanner = new StepwisePlanner(sk);
var plan = stepwisePlanner.CreatePlan(researchRequest);
 
var result = await plan.InvokeAsync(researchContext);
Console.WriteLine(result);