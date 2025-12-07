namespace GameServer.Model.Terrains;



public interface ITerrain
{
    // TODO: Add some effects for terrain
}

public struct Ground : ITerrain {}
public struct Wall : ITerrain {}