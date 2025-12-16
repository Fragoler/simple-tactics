using GameServer.Model.Action.Components;
using GameServer.Model.Action.Effects;
using GameServer.Model.Action.Systems;
using GameServer.Model.EventBus;
using GameServer.Model.Phases;
using GameServer.Presenter.Socket.DTO;

namespace GameServer.Presenter.Socket;


public sealed partial class GameHub
{
    private readonly ActionSystem _action;
    private readonly PhasesSystem _phases;
    private readonly EventBusSystem _event;
    
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

            TargetType = ent.Component.Effect is ICellTargetActionEffect
                ? "Cell" : "None",
            TargetFilter =  new TargetFilterDto(ent.Component.TargetFilter),

            HighlightLayers = layers.ToArray(),
        };
    }
    
}