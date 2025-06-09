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

  [HttpPost("LoginForUpdate")]
  public async Task<ActionResult<ResultClass<Update>>> LoginForUpdate([FromBody] LoginForUpdateDTO request)
  {
    var result = await _updateService.LoginForUpdate(request);

    return Ok(result);
  }
}
