using GameServer.Model.IoC;
using GameServer.Model.Prototype;
using Microsoft.AspNetCore.Mvc;

namespace GameServer.Presenter;

[ApiController]
[Route("api/proto")]
public class PrototypeController(IoCManager ioc, IConfiguration conf) : ControllerBase
{
    private readonly PrototypeSystem _proto = ioc.Resolve<PrototypeSystem>();
    private readonly IConfiguration _conf = conf;

    [HttpPost("load")]
    public async Task<IActionResult> UploadPrototype(IFormFile? file)
    {
        if (file == null || file.Length == 0)
            return BadRequest("No file uploaded");

        if (!file.FileName.EndsWith(".yml") && !file.FileName.EndsWith(".yaml"))
            return BadRequest("Only YAML files are allowed");

        try
        {
            using var reader = new StreamReader(file.OpenReadStream());
            var yamlContent = await reader.ReadToEndAsync();
            
            _proto.LoadPrototypes(yamlContent);
            
            return Ok(new { message = "Prototype loaded successfully", fileName = file.FileName });
        }
        catch (Exception ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }

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