using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Skills.Web;
using Microsoft.SemanticKernel.Skills.Web.Bing;
using Microsoft.SemanticKernel.Skills.OpenAPI.Extensions;

public static class PluginExtensions
{
    public static async Task LoadResearchPluginsAsync(this IKernel kernel, AppConfig appConfig)
    {
        kernel.ImportSkill(new WebSearchEngineSkill(new BingConnector(appConfig.BingApiKey)), "WebSearch");

        foreach (var plugin in appConfig.Plugins)
        {
            await kernel.ImportAIPluginAsync(
                plugin.Name,
                new Uri(plugin.Url),
                executionParameters: new OpenApiSkillExecutionParameters { IgnoreNonCompliantErrors = true });
        }
        
    }
}