using System.Diagnostics;
using GameServer.Model.Components;
using GameServer.Model.Entities;
using GameServer.Model.Map;
using GameServer.Model.Games;
using GameServer.Presenter.Socket.DTO;

namespace GameServer.Presenter.Socket;


public sealed partial class GameHub
{
    private readonly ComponentSystem _comp = ioc.Resolve<ComponentSystem>();
    private readonly EntitySystem _entity = ioc.Resolve<EntitySystem>();
    
    public async Task<GameStateDto> SerializeGameState(Game game)
    {
        var units = SerializeUnits(game);
        var players = SerializePlayers(game);
        var map = await SerializeMap(game);
        
        
        return new GameStateDto
        {
            Map = map,
            Units = units,
            Players = players,
        };
    }

    private async Task<MapStateDto?> SerializeMap(Game game)
    {
        var mapComp = _comp.GetComponentOrDefault<MapComponent>(game.Map);
        if (mapComp == null)
            return null;

        var terrain = mapComp.Terrain;
        if (terrain is null)
            throw new NullReferenceException("Terrain is null");

        Debug.Assert(terrain.Width == mapComp.Width);
        Debug.Assert(terrain.Height == mapComp.Height);

        var terrainDto = new uint[terrain.Width][];
        for (uint i = 0; i < terrain.Width; ++i)
            terrainDto[i]  = new uint[terrain.Height];
        
        await terrain.ForEachAsync((x, y, val) =>
        {
            terrainDto[x][y] = val;
            return Task.CompletedTask;
        });
        
        return new MapStateDto
        {
            MapName = mapComp.MapName,
            Width = mapComp.Width,
            Height = mapComp.Height,
            Terrain = terrainDto
        };
    }
}