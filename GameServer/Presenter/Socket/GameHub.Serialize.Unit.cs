using System.Runtime.Serialization;
using GameServer.Model.Action.Components;
using GameServer.Model.Entities;
using GameServer.Model.Games;
using GameServer.Model.Health;
using GameServer.Model.Players;
using GameServer.Model.Sprite;
using GameServer.Model.Transform;
using GameServer.Presenter.Socket.DTO;

namespace GameServer.Presenter.Socket;


public sealed partial class GameHub
{
    private UnitDto[] SerializeUnits(Game game)
    {
        var units = new List<UnitDto>();
        
        
        foreach (var entity in _entity.GetAllEntity(game))
        {
            if (!_comp.TryGetComponent<TransformComponent>(entity, out var transform))
                continue;

            var dto = SerializeUnit((entity, transform));
            units.Add(dto);
        }

        return units.ToArray();
    }

    private UnitDto SerializeUnit(Entity<TransformComponent> entity)
    {
        var controlled = _comp.GetComponentOrDefault<ControlledComponent>(entity);

        // Player ID
        uint? playerId = null;
        if (controlled?.Player != null)
            playerId = _players.GetPlayerId(controlled.Player.Value);
        //
        
        // Health
        uint? curHealth = null;
        uint? maxHealth = null;
        if (_comp.TryGetComponent<HealthComponent>(entity, out var health))
        {
            curHealth = health.CurrentHealth;
            maxHealth = health.MaxHealth;
        }
        //
        
        // Sprite 
        if (!_comp.TryGetComponent<SpriteComponent>(entity, out var sprite))
            throw new SerializationException("Cannot find SpriteComponent");
        // 
        
        // Actions
        string[] actionIds = [];
        if (_comp.TryGetComponent<ActionsContainerComponent>(entity, out var actions))
            actionIds = actions.ActionPrototypes;
        //
        
        return new UnitDto
        {
            UnitId = entity.Ent.Info.Id,
            PlayerId = playerId,
            
            Coords = new PositionDto
            {
                X = entity.Component.Coords.X,
                Y = entity.Component.Coords.Y,
            },
            
            Sprite = sprite.Type,
            ActionIds = actionIds,
            
            CurHealth = curHealth,
            MaxHealth = maxHealth
        };
    }
}