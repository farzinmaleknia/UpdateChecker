
namespace Api.Services.SecurityQuestions;

public interface ISecurityQuestionService
{
  public Task<ResultClass<List<SecurityQuestion>>> GetQuestions();
}
