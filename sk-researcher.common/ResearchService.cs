using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Planning;

namespace sk_researcher.common;

public class ResearchService : IResearchService
{
    private readonly IKernel _kernel;

    public ResearchService(IKernel kernel)
    {
        _kernel = kernel;        
    }

    public async Task<Dossier> ResearchAsync(string topic)
    {
        var dossier = new Dossier{ Topic = topic };

        var researchRequest = $"I would like to learn about {topic}" +
            "The more detail you are able to provide the better. " +
            "If you do a web search, don't just take the summary from the search results, scrape each linked page and summarise for best context." +
            "While the opinion of a journalist is interesting, bias towards original sources where possible.";

        var researchContext = _kernel.CreateNewContext();

        var stepwisePlanner = new StepwisePlanner(_kernel);
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