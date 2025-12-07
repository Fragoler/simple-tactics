using GameServer.Model.Action.Effects;

namespace GameServer.Model.Action.Systems;


public sealed partial class ActionSystem
{
    private Dictionary<string, IActionEffect> _effects = new()
    {
        { "None", new DoNothingEffect() },
        { "Move", new MoveEffect()},
        { "Melee", new MeleeEffect() },
        { "Shoot", new ShootEffect() },
        { "SelfDetonation", new SelfDetonation()}
    };
    
    private IActionEffect GetEffect(string effectId) =>  _effects[effectId];
        
}