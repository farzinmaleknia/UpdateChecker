using PuppeteerSharp;
using Api.Services.Browser;
using PuppeteerSharp.Input;
using Api.Services.SecurityQuestions;
using Microsoft.EntityFrameworkCore.Infrastructure.Internal;

public class UpdateVerification : IUpdateVerification
{
  private readonly BrowserService _browserService;
  private readonly ISecurityQuestionService _securityQuestionService;
  private readonly IPuppeteerSharpUtilities _puppeteerSharpUtilities;

  public UpdateVerification(BrowserService browserService, ISecurityQuestionService securityQuestionService, IPuppeteerSharpUtilities puppeteerSharpUtilities)
  {
    _browserService = browserService;
    _securityQuestionService = securityQuestionService;
    _puppeteerSharpUtilities = puppeteerSharpUtilities;
  }

  public async Task<ResultClass<Update>> VerificationForUpdate(VerificationForUpdateDTO request)
  {
    var result = new ResultClass<Update>() { Data = new Update(), MessageKey = new List<string>() };
    var session = _browserService.GetSession(request.SessionId);

    try
    {
      var browser = session.Value.Item1;
      var page = session.Value.Item2;

      await _puppeteerSharpUtilities.TypeAndContinue(page, "continue-btn", "code", request.VerificationCode);

      //////// set the false verification responce here
      if (false)
      {
        throw new Exception(nameof(Messages.OpeningPageUnsuccessful));
      }

      await _puppeteerSharpUtilities.TypeAndContinue(page, "continue-btn");

      await _puppeteerSharpUtilities.TypeAndContinue(page, "_continue");


      var securityQuestion = await page.EvaluateFunctionAsync<string>(@"() => {
        const el = [...document.querySelectorAll('strong')]
          .find(el => el.innerText.includes('My favorite'));
        return el ? el.innerText : '';
      }");

      if (securityQuestion == null )
      {
        throw new Exception(nameof(Messages.OpeningPageUnsuccessful));
      }

      var filteredSQ = securityQuestion.Replace("\"", "").Replace("/\"", "");

      var sqResult = await _securityQuestionService.GetQuestions();

      if (!sqResult.IsSuccess)
      {
        throw new Exception(message: sqResult.MessageKey[0]);
      }

      var questions = sqResult.Data;

      var question = questions.FirstOrDefault(q => q.Question == filteredSQ);

      if (!(question != null && !string.IsNullOrEmpty(question.Answer)))
      {
        throw new Exception(message: string.Format(Messages.ThatQuestionOrAnswerEmpty, filteredSQ));
      }


      await _puppeteerSharpUtilities.TypeAndContinue(page, "_continue", "answer", question.Answer);

      //////// Check for false security Question answer

      result.Data = new Update()
      {
        UpdateStep = Api.Services.Enums.UpdateSteps.Completed,
        SessionId = request.SessionId,
      };
      result.IsSuccess = true;

      _browserService.SetPage(request.SessionId, page);

    }
    catch (Exception ex)
    {
      _browserService.CloseSessionAsync(request.SessionId);
      result.Data = new Update()
      {
        UpdateStep = Api.Services.Enums.UpdateSteps.WaitingForVerificationCode,
        SessionId = request.SessionId,
      };
      result.MessageKey.Add(ex.Message);
    }

    return result;
  }


}