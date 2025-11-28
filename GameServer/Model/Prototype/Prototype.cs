namespace GameServer.Model.Prototype;


public sealed class EntityPrototype
{
    public string Id { get; set; } = string.Empty;
    public List<ComponentPrototype> Components { get; set; } = [];
    
    internal List<ResolvedComponentData> ResolvedComponents { get; set; } = [];
}

public sealed class ComponentPrototype
{
    public string Type { get; set; } = string.Empty;
    public Dictionary<string, object>? Data { get; set; }
}

internal sealed class ResolvedComponentData
{
    public Type ComponentType { get; set; } = null!;
    public Dictionary<string, object>? Data { get; set; }
}