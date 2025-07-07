using Api.Services.SecurityQuestions;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("[controller]")]
public class SecurityQuestionController : ControllerBase
{
  private readonly ISecurityQuestionService _securityQuestionService;

  public SecurityQuestionController(ISecurityQuestionService securityQuestionService)
  {
    _securityQuestionService = securityQuestionService;
  }

  [HttpGet]
  public async Task<ActionResult<ResultClass<List<SecurityQuestion>>>> GetQuestions()
  {
    var result = await _securityQuestionService.GetQuestions();

    return Ok(result);
  }
}
