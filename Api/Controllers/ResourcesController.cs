
using Api.Services.Resources;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("[controller]")]
public class ResourcesController : ControllerBase
{
    private readonly ResourcesService _ResourcesService;

    public ResourcesController(ResourcesService resourcesService)
    {
        _ResourcesService = resourcesService;
    }

    [HttpPost]
    public async Task<ActionResult<ResultClass<string>>> GetResources()
    {
        var result = await _ResourcesService.GetResources();

        return Ok(result);
    }
}
