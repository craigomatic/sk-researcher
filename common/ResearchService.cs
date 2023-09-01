using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Planning;
using Microsoft.SemanticKernel.SkillDefinition;
using System.Reflection;

namespace sk_researcher.common;

public class ResearchService : IResearchService
{
    private readonly IKernel _kernel;
    private readonly int _maxTokens;

    public ResearchService(IKernel kernel, int maxTokens = 1024)
    {
        _kernel = kernel;
        _maxTokens = maxTokens;
    }

    public async Task<Dossier> ResearchAsync(string topic)
    {
        var dossier = new Dossier{ Topic = topic };

        var researchRequest = "You are a diligent researcher, and you have been asked to write a detailed report on the following topic:" +
            "[TOPIC]" +
            $"{topic}" +
            "[END TOPIC]" +
            "You must provide both detail and context, and you must provide references to your sources." +
            "If you do a web search, don't just take the summary from the search results, scrape each linked page for best context." +
            "While the opinion of a journalist is interesting, bias towards original sources where possible.";

        var researchContext = _kernel.CreateNewContext();

        var stepwisePlanner = new StepwisePlanner(
            _kernel, 
            new Microsoft.SemanticKernel.Planning.Stepwise.StepwisePlannerConfig 
            { 
                MaxTokens = _maxTokens 
            });

        var plan = stepwisePlanner.CreatePlan(researchRequest);
        
        var result = await plan.InvokeAsync(researchContext);
        
        if (result.ErrorOccurred)
        {
            throw result.LastException!;
        }

        dossier.Content = result.Result;

        return dossier;
    }
}