using GameServer.Model.Components;
using GameServer.Model.Entities;

namespace GameServer.Model.Players.Components;


public sealed class ControlledComponent : Component
{
    public Entity<PlayerComponent>? Player;
}