namespace GameServer.Model.Terrains;


/// <summary>
/// A base for cell's terrain
/// </summary>
public interface ITerrain
{
    bool PassableForUnit { get; }
    bool PassableForBullet { get;  }
}

public struct Ground : ITerrain
{
    public bool PassableForUnit => true;
    public bool PassableForBullet => true;
}

public struct Wall : ITerrain
{
    public bool PassableForUnit => false;
    public bool PassableForBullet => false;
}

public struct Water : ITerrain
{
    public bool PassableForUnit => false;
    public bool PassableForBullet => true;
}
