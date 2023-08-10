public class AppConfig
{
    public IEnumerable<PluginConfig> Plugins { get; set; } = Enumerable.Empty<PluginConfig>();
}

public class PluginConfig
{
    public string Name { get; set; } = string.Empty;

    public string Url { get; set; } = string.Empty;
}