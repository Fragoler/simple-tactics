using GameServer.Model.Entities;
using GameServer.Model.Games;
using GameServer.Model.Transform;
using GameServer.Presenter.Socket.DTO;

namespace GameServer.Model.Effect;


public abstract class EffectArgs
{
    public Entity Entity { get; init; }
    public int Duration { get; set; } = 500;
    
    public abstract EffectDto ToDto();
}

public sealed class MoveEffectArgs : EffectArgs
{
    public Coordinates From { get; init; }
    public Coordinates To { get; init; }
    
    public override EffectDto ToDto() => new MoveEffectDto
    {
        Type = "Move",
        UnitId = Entity.Info.Id,
        From = From,
        To = To
    };
}

public sealed class DamageEffectArgs : EffectArgs
{
    public uint Amount { get; init; }
    
    public override EffectDto ToDto() => new DamageEffectDto
    {
        Type = "Damage",
        UnitId = Entity.Info.Id,
        Duration = Duration,
        Amount = Amount,
    };
}

public sealed class ShootEffectArgs : EffectArgs
{
    public Coordinates From { get; init; }
    public Coordinates To { get; init; }
    public Entity? Target { get; init; }
    
    public override EffectDto ToDto() => new ShootEffectDto
    {
        Type = "Shoot",
        UnitId = Entity.Info.Id,
        Duration = Duration,
        From = From,
        To = To,
        TargetUnitId = Target?.Info.Id
    };
}

public sealed class MeleeEffectArgs : EffectArgs
{
    public Coordinates From { get; init; }
    public Coordinates To { get; init; }
    public Entity Target { get; init; }
    
    public override EffectDto ToDto() => new MeleeEffectDto
    {
        Type = "Melee",
        UnitId = Entity.Info.Id,
        Duration = Duration,
        From = From,
        To = To,
        TargetUnitId = Target.Info.Id
    };
}

public sealed class ExplosionEffectArgs : EffectArgs
{
    public Coordinates Center { get; init; }
    public double Radius { get; init; }
    
    public override EffectDto ToDto() => new ExplosionEffectDto
    {
        Type = "Explosion",
        UnitId = Entity.Info.Id,
        Duration = Duration,
        Center = Center,
        Radius = Radius
    };
}

public sealed class DeathEffectArgs : EffectArgs
{
    public override EffectDto ToDto() => new DeathEffectDto
    {
        Type = "Death",
        UnitId = Entity.Info.Id,
        Duration = Duration
    };
}

public sealed class HealEffectArgs : EffectArgs
{
    public Entity Target { get; init; }
    public uint Amount { get; init; }
    
    public override EffectDto ToDto() => new HealEffectDto
    {
        Type = "Heal",
        UnitId = Entity.Info.Id,
        Duration = Duration,
        TargetUnitId = Target.Info.Id,
        Amount = Amount
    };
}
