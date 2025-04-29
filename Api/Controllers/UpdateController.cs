
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("[controller]")]
public class UpdateController : ControllerBase
{

    public UpdateController()
    {
    }

    [HttpGet]
    public async Task<ActionResult<Update>> FetchAllUpdates()
    {
        var result = new Update(){WebContent = "new contetn"};

        return Ok(result);
    }
}
