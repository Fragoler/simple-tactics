using GameServer.Model.Effects;
using GameServer.Model.Entities;
using GameServer.Model.Phases.Events;
using GameServer.Model.Players.Components;
using GameServer.Model.Transform;
using GameServer.Presenter.Socket.DTO;
using Microsoft.AspNetCore.SignalR;

namespace GameServer.Presenter.Socket;


public sealed partial class GameHub
{
    private readonly EffectSystem _effects;
    
    private async Task HandlePlayerSchedule(Entity<PlayerComponent> player, ScheduledActionDto[] actionsDto)
    {
        
        // Before checks
        var ev1 = new CanSchedulePlayerActionsEvent{ Game = player.Ent.Game, Player = player};
        _event.RaiseGlobal(ev1);
        if (ev1.Cancelled)
        {
            await Clients.Caller.SendAsync("error", "Cannot schedule player actions");
            return;
        }
        //
        
        // Formatting
        var actionsN = actionsDto.Select(
            a => (_entity.GetEntity(a.UnitId, player.Ent.Game), a.ActionId, a.Target)).ToArray();

        if (actionsN.Any(a => a.Item1 is null))
        {
            _logger.LogError("Invalid entity received");
            await Clients.Caller.SendAsync("error", "Invalid entity received");
            return;
        }

        var actions = actionsN.Select(a => ((Entity, string, Coordinates?)) (a with { Item1 = a.Item1!.Value})! ).ToArray();
        //
        
        // Scheduling
        if (!_action.TrySchedulePlayerActions(player, actions))
        {
            _logger.LogError("Cannot schedule player actions");
            await Clients.Caller.SendAsync("error", "Cannot schedule player actions");
            return;
        }
        //

        await SendPlayersStateToAll(player.Ent.Game);
        
        // try start calculating actions
        var ev2 = new CanEndPlaningPhaseEvent{ Game = player.Ent.Game };
        _event.RaiseGlobal(ev2);
        if (ev2.Cancelled)
            return;
        
        _phases.StartCalculating(ev2.Game);
        await Clients.Group(ev2.Game.Token).SendAsync("actionResults", 
            _effects.RetrieveAllEffects(ev2.Game).Select(u => u.ToDto()));
        

        //
    }

}