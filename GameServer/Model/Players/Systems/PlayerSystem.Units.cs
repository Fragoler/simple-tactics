using GameServer.Model.Entities;
using GameServer.Model.Players.Components;

namespace GameServer.Model.Players.Systems;


public sealed partial class PlayersSystem
{
    public Entity<ControlledComponent> AssignUnitToPlayer(Entity<PlayerComponent> player, Entity entity)
    {
        var controlled = _comp.EnsureComponent<ControlledComponent>(entity);
        controlled.Player = player;
        
        return (entity, controlled);
    }
}