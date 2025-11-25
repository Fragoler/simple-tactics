using GameServer.Model.Entities;
using GameServer.Model.IoC;
using GameServer.Model.Systems;

namespace GameServer.Model.Contexts;


public class Context
{
    public Dictionary<Guid, Game> Games = [];
    public IoCManager IoC = new();
    
    private static Context? _сontextValue;
    
    public static Context Get()
    {
        _сontextValue ??= new Context();
        return _сontextValue;
    }
    

}