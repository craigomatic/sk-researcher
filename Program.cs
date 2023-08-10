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

var scratchPad = new ScratchPad();

var sk = Kernel.Builder.WithLogger(new ScratchPadLogger { ScratchPad = scratchPad }).Configure(embeddingConfig, completionConfig);
await sk.LoadResearchPluginsAsync(appConfig!);

scratchPad.Topic = "superconductivity at room temperature and LK-99, and why this potential discovery may be significant.";

var researchRequest = $"I would like to learn about {scratchPad.Topic}" +
    "The more detail you are able to provide the better. " +
    "If you do a web search, don't just take the summary from the search results, scrape each linked page and summarise for best context.";

var researchContext = sk.CreateNewContext();

var stepwisePlanner = new StepwisePlanner(sk);
var plan = stepwisePlanner.CreatePlan(researchRequest);
 
var result = await plan.InvokeAsync(researchContext);
Console.WriteLine(result);