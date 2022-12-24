namespace Automata.Runtime.Models;

public record PluginModel
{
    public string? Name { get; set; }

    public string? Description { get; set; }

    public string? Version { get; set; }

    public string? Author { get; set; }

    public DateTime CreationDate { get; set; }

    public IEnumerable<Block> Blocks { get; set; }

    public PluginModel()
    {
        Blocks = new List<Block>();
    }
}
