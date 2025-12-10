using GameServer.Model.Action.Components;
using GameServer.Model.Action.Systems;
using GameServer.Model.Entities;
using GameServer.Model.Games;
using GameServer.Model.Players.Components;
using GameServer.Model.Transform;
using GameServer.Presenter.Socket.DTO;
using Microsoft.AspNetCore.SignalR;

namespace GameServer.Presenter.Socket;


public sealed partial class GameHub
{
    private readonly ActionSystem _action = ioc.Resolve<ActionSystem>();
    
    private ActionDto[] SerializeActions()
    {
        return _action.GetAllActionIds().Select(SerializeAction).ToArray();
    }

    private ActionDto SerializeAction(string id)
    {
        var ent = _action.GetAction(id);
        
        List<HighlightedLayerDto> layers = [];
        if (_comp.TryGetComponent<ActionHighlightComponent>(ent, out var highlight)) 
            layers.AddRange(highlight.Layers.Select(layer => new HighlightedLayerDto(layer)));


        return new ActionDto
        {
            Id = id,

            Name = ent.Component.Name,
            Icon = ent.Component.Icon,

            TargetType = ent.Component.TargetType.ToString(),
            TargetFilter = new TargetFilterDto(ent.Component.TargetFilter),

            HighlightLayers = layers.ToArray(),
        };
    }

    private async Task HandlePlayerSchedule(Entity<PlayerComponent> player, ScheduledActionDto[] actionsDto)
    {
        var actionsN = actionsDto.Select(
            a => (_entity.GetEntity(a.UnitId, player.Ent.Game), a.ActionId, a.Target)).ToArray();

        if (actionsN.Any(a => a.Item1 is null))
        {
            _logger.LogError("Invalid entity received");
            await Clients.Caller.SendAsync("error", "Invalid entity received");
            return;
        }

        var actions = actionsN.Select(a => ((Entity, string, Coordinates?)) (a with { Item1 = a.Item1!.Value})! ).ToArray();
        if (!_action.TrySchedulePlayerActions(player, actions))
        {
            _logger.LogError("Cannot schedule player actions");
            await Clients.Caller.SendAsync("error", "Cannot schedule player actions");
            return;
        }

        player.Component.IsReady = true;
        await SendPlayersStateToAll(player.Ent.Game);
    }
}