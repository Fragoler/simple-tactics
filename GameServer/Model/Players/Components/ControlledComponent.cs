using GameServer.Model.Components;
using GameServer.Model.Entities;

namespace GameServer.Model.Players.Components;



/// <summary>
/// Mark entity as controlled by the current player
/// </summary>
public sealed class ControlledComponent : Component
{
    public Entity<PlayerComponent>? Player;
}