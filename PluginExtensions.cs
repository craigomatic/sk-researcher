using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Skills.OpenAPI.Extensions;

public static class PluginExtensions
{
    public static async Task LoadResearchPluginsAsync(this IKernel kernel, AppConfig appConfig)
    {
        if (!appConfig.Plugins.Any())
        {
            throw new Exception("No plugins have been specified. Please specify at least one plugin in appsettings.json");
        }

        foreach (var plugin in appConfig.Plugins)
        {
            await kernel.ImportAIPluginAsync(
                plugin.Name,
                new Uri(plugin.Url),
                executionParameters: new OpenApiSkillExecutionParameters { IgnoreNonCompliantErrors = true });
        }
    }
}