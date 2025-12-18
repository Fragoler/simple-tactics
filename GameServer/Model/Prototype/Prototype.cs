using GameServer.Model.Components;

namespace GameServer.Model.Prototype;


/// <summary>
/// Prototype of entity
/// Used as template for entity creation
/// </summary>
public sealed class EntityPrototype
{
    public string Id { get; set; } = string.Empty;
    public List<Component> Components { get; set; } = [];
}