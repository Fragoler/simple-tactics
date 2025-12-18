using GameServer.Model.Entities;
using GameServer.Model.Prototype;
using GameServer.Model.Transform;

namespace GameServer.Model.Action.Effects;


/// <summary>
/// Do absolutely nothing
/// </summary>
[YamlType("None")]
public sealed class DoNothingEffect : INoneTargetActionEffect
{
    public void Execute(Entity<TransformComponent> executor)
    {

    }
}