
using Api.Data;
using Microsoft.EntityFrameworkCore;
using Api.Resources;

namespace Api.Services.SecurityQuestions;

public class SecurityQuestionService : ISecurityQuestionService
{
  private readonly DataContext _dataContext;

  public SecurityQuestionService(DataContext dataContext)
  {
    _dataContext = dataContext;
  }
  public async Task<ResultClass<List<SecurityQuestion>>> GetQuestions()
  {
    var result = new ResultClass<List<SecurityQuestion>>
    {
      MessageKey = new List<string>(),
    };

    try
    {
      var questions = await _dataContext.SecurityQuestions.ToListAsync();

      if (questions is null)
      {
        throw new Exception(message: nameof(Messages.SomethingWrong));
      }

      result.Data = questions;
      result.IsSuccess = true;
      return result;
    }
    catch (Exception ex)
    {
      result.IsSuccess = false;
      result.MessageKey.Add(ex.Message);

      return result;
    }
  }
}
