using GameServer.Model.Entities;
using GameServer.Model.Games;
using GameServer.Model.Transform;
using GameServer.Presenter.Socket.DTO;

namespace GameServer.Model.Effects;


public abstract class EffectArgs
{
    public required Game Game { get; init; }
    public int Duration { get; init; } = 500;

    public abstract EffectDto ToDto();
}

public sealed class MoveEffectArgs : EffectArgs
{
    public Entity Entity { get; init; }
    public Coordinates From { get; init; }
    public Coordinates To { get; init; }
    
    public override EffectDto ToDto() => new()
    {
        Type = "Move",
        UnitId = Entity.Info.Id,
        From = From,
        To = To
    };
}

public sealed class DamageEffectArgs : EffectArgs
{
    public Entity Entity { get; init; }
    public uint Amount { get; init; }
    
    public override EffectDto ToDto() => new()
    {
        Type = "Damage",
        UnitId = Entity.Info.Id,
        Duration = Duration,
        Amount = Amount,
    };
}

public sealed class ShootEffectArgs : EffectArgs
{
    public Entity Entity { get; init; }
    public Coordinates From { get; init; }
    public Coordinates To { get; init; }
    public Entity? Target { get; init; }
    
    public override EffectDto ToDto() => new()
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
    public Entity Entity { get; init; }
    public Coordinates From { get; init; }
    public Coordinates To { get; init; }
    public Entity Target { get; init; }
    
    public override EffectDto ToDto() => new()
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
    public Entity Entity { get; init; }
    public Coordinates Center { get; init; }
    public double Radius { get; init; }
    
    public override EffectDto ToDto() => new()
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
    public Entity Entity { get; init; }
    public override EffectDto ToDto() => new()
    {
        Type = "Death",
        UnitId = Entity.Info.Id,
        Duration = Duration
    };
}

public sealed class HealEffectArgs : EffectArgs
{
    public Entity Entity { get; init; }
    public Entity Target { get; init; }
    public uint Amount { get; init; }
    
    public override EffectDto ToDto() => new()
    {
        Type = "Heal",
        UnitId = Entity.Info.Id,
        Duration = Duration,
        TargetUnitId = Target.Info.Id,
        Amount = Amount
    };
}
