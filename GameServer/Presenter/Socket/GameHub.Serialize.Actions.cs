using GameServer.Model.Action.Components;
using GameServer.Model.Action.Systems;
using GameServer.Model.Games;
using GameServer.Presenter.Socket.DTO;

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
        
        HighlightedLayer[] layers = [];
        if (_comp.TryGetComponent<ActionHighlightComponent>(ent, out var highlight))
            layers = highlight.Layers;

        return new ActionDto
        {
            Id = id,

            Name = ent.Component.Name,
            Icon = ent.Component.Icon,

            TargetType = ent.Component.TargetType,
            TargetFilter = ent.Component.TargetFilter,

            HighlightLayers = layers,
        };
    }
}