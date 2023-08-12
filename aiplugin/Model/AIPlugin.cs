using System.Text.Json.Serialization;

public class AIPlugin
{
    [JsonPropertyName("schema_version")]
    public string SchemaVersion { get; set; } = "v1";

    [JsonPropertyName("name_for_model")]
    public string NameForModel { get; set; } = "researcher";

    [JsonPropertyName("name_for_human")]
    public string NameForHuman { get; set; } = "researcher";

    [JsonPropertyName("description_for_model")]
    public string DescriptionForModel { get; set; } = "Researches a topic";

    [JsonPropertyName("description_for_human")]
    public string DescriptionForHuman { get; set; } = "Researches a topic";

    public AIPluginAuth Auth { get; set; } = new AIPluginAuth { Type = "none" };

    public AIPluginAPI Api { get; set; } = new AIPluginAPI { Type = "openapi" };

    [JsonPropertyName("contact_email")]
    public string ContactEmail { get; set; } = string.Empty;

    [JsonPropertyName("logo_url")]
    public string LogoUrl { get; set; } = string.Empty;

    [JsonPropertyName("legal_info_url")]
    public string LegalInfoUrl { get; set; } = string.Empty;
}

public class AIPluginAuth
{
    public string Type { get; set; } = string.Empty;
}

public class AIPluginAPI
{
    public string Type { get; set; } = string.Empty;

    public string Url { get; set; } = string.Empty;
}