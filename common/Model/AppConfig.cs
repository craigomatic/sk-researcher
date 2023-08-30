public class AppConfig
{
    public List<PluginConfig> Plugins { get; set; } = new();
}

public class PluginConfig
{
    public string Name { get; set; } = string.Empty;

    public string Url { get; set; } = string.Empty;
}