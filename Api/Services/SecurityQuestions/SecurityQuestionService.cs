
namespace Api.Services.SecurityQuestions;

public class SecurityQuestionService : ISecurityQuestionService
{

  public SecurityQuestionService()
  {
  }
  public async Task<ResultClass<List<SecurityQuestion>>> GetQuestions()
  {
    var result = new ResultClass<List<SecurityQuestion>>
    {
      MessageKey = new List<string>(),
    };


    return result;
  }
}
