using GameServer.Model.Components;

namespace GameServer.Model.Health;


/// <summary>
/// Add the ability to track health points
/// </summary>
public sealed class HealthComponent : Component
{
    public uint MaxHealth { get; set; } = 10;
    public uint CurrentHealth = 0; // If still 0 after proto spawned - automatically set MaxHealth
}