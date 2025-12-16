using GameServer.Model.IoC;
using GameServer.Model.Prototype;
using Microsoft.AspNetCore.Mvc;

namespace GameServer.Presenter.API;


[ApiController]
[Route("api/proto")]
public class PrototypeController(IoCManager ioc, IConfiguration conf) : ControllerBase
{
    private readonly PrototypeSystem _proto = ioc.Resolve<PrototypeSystem>();
    private readonly IConfiguration _conf = conf;

    [HttpGet("list")]
    public IActionResult ListPrototypes()
    {
        var prototypes = _proto.GetAllPrototypeIds();
        return Ok(prototypes);
    }

    [HttpGet("{id}")]
    public IActionResult GetPrototype(string id)
    {
        var prototype = _proto.GetPrototype(id);
        if (prototype == null)
            return NotFound($"Prototype '{id}' not found");

        return Ok(prototype);
    }
}