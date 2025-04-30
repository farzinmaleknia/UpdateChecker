
using Api.Models.ResultClass;
using Api.Services.Updates;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("[controller]")]
public class UpdateController : ControllerBase
{
    private readonly IUpdateService _updateService;

    public UpdateController(IUpdateService updateService)
    {
        _updateService = updateService;
    }

    [HttpGet]
    public async Task<ActionResult<ResultClass<Update>>> FetchAllUpdates()
    {
        var result = await _updateService.FetchAllUpdates();

        return Ok(result);
    }
}
