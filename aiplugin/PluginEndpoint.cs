using System.Net;
using System.Text.Json;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Extensions.OpenApi.Extensions;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using sk_researcher.common;

public class PluginEndpoint
{
    private readonly ILogger _logger;
    private readonly IResearchService _researchService;

    public PluginEndpoint(IResearchService researchService, ILoggerFactory loggerFactory)
    {
        _researchService = researchService;
        _logger = loggerFactory.CreateLogger<PluginEndpoint>();
    }

    [Function("WellKnownAIPlugin")]
    public async Task<HttpResponseData> WellKnownAIPlugin(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = ".well-known/ai-plugin.json")] HttpRequestData req)
    {
        var toReturn = new AIPlugin();
        toReturn.Api.Url = $"{req.Url.Scheme}://{req.Url.Host}:{req.Url.Port}/swagger.json";

        var r = req.CreateResponse(HttpStatusCode.OK);
        await r.WriteAsJsonAsync(toReturn);
        return r;
    }

    [OpenApiOperation(operationId: "ResearchTopic", tags: new[] { "ResearchTopicFunction" }, Description = "Researches the given topic.")]
    [OpenApiParameter(name: "Topic", Description = "The topic to research", Required = true, In = ParameterLocation.Query)]
    [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "text/plain", bodyType: typeof(string), Description = "Returns a single research result.")]
    [OpenApiResponseWithBody(statusCode: HttpStatusCode.BadRequest, contentType: "application/json", bodyType: typeof(string), Description = "Returns the error of the input.")]
    [Function("ResearchTopic")]
    public async Task<HttpResponseData> ResearchTopic([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "research")] HttpRequestData req)
    {
        var topic = req.Query("Topic").FirstOrDefault();

        if (topic == null)
        {
            return req.CreateResponse(HttpStatusCode.BadRequest);
        }

        var result = await _researchService.ResearchAsync(topic);

        var r = req.CreateResponse(HttpStatusCode.OK);
        r.Headers.Add("Content-Type", "text/plain");
        await r.WriteStringAsync(result.Content);
        return r;
    }
}
