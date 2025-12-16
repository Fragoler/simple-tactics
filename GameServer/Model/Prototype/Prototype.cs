using GameServer.Model.Components;

namespace GameServer.Model.Prototype;


public sealed class EntityPrototype
{
    public string Id { get; set; } = string.Empty;
    public List<Component> Components { get; set; } = [];
}