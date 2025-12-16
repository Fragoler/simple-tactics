using GameServer.Model.Entities;
using GameServer.Model.Prototype;
using GameServer.Model.Transform;

namespace GameServer.Model.Action.Effects;


[YamlType("None")]
public sealed class DoNothingEffect : INoneTargetActionEffect
{
    public void Execute(Entity<TransformComponent> executor)
    {

    }
}