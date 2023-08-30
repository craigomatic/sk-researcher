public class Dossier
{
    public string Topic { get; set; } = string.Empty;

    public string Content { get; set; } = string.Empty;

    public List<string> Thoughts { get; set; } = new();

    public List<string> Citations { get; set; } = new();
}
