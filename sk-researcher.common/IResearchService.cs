namespace sk_researcher.common;

public interface IResearchService
{
    Task<Dossier> ResearchAsync(string topic);
}
