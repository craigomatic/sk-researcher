using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Memory;

public static class ConfigExtensions
{
    public static IKernel Configure(this KernelBuilder kernelBuilder, ModelConfig? embeddingConfig, ModelConfig? completionConfig)
    {
        if (embeddingConfig != null)
        {
            switch (embeddingConfig.AIService.ToUpperInvariant())
            {
                case ModelConfig.AzureOpenAI:
                    kernelBuilder = kernelBuilder.WithAzureTextEmbeddingGenerationService(embeddingConfig.DeploymentOrModelId, embeddingConfig.Endpoint, embeddingConfig.Key);
                    break;
                case ModelConfig.OpenAI:
                    kernelBuilder = kernelBuilder.WithOpenAITextEmbeddingGenerationService(embeddingConfig.DeploymentOrModelId, embeddingConfig.Key);
                    break;
                default:
                    throw new NotSupportedException("Invalid AI Service was specified for embeddings");
            }
        }

        if (completionConfig != null)
        {
            switch (completionConfig.AIService.ToUpperInvariant())
            {
                case ModelConfig.AzureOpenAI:
                    kernelBuilder = kernelBuilder.WithAzureChatCompletionService(completionConfig.DeploymentOrModelId, completionConfig.Endpoint, completionConfig.Key);
                    break;
                case ModelConfig.OpenAI:
                    kernelBuilder = kernelBuilder.WithOpenAIChatCompletionService(completionConfig.DeploymentOrModelId, completionConfig.Key);
                    break;
                default:
                    throw new NotSupportedException("Invalid AI Service was specified for completions");
            }
        }

        return kernelBuilder.
            WithMemoryStorage(new VolatileMemoryStore()).
            Build();
    }
}