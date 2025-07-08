using PuppeteerSharp;
using Api.Services.Browser;
using PuppeteerSharp.Input;
using Api.Services.SecurityQuestions;

public class UpdateVerification : IUpdateVerification
{
  private readonly BrowserService _browserService;
  private readonly ISecurityQuestionService _securityQuestionService;

  public UpdateVerification(BrowserService browserService, ISecurityQuestionService securityQuestionService)
  {
    _browserService = browserService;
    _securityQuestionService = securityQuestionService;
  }

  public async Task<ResultClass<Update>> VerificationForUpdate(VerificationForUpdateDTO request)
  {
    var result = new ResultClass<Update>() { Data = new Update(), MessageKey = new List<string>() };
    var session = _browserService.GetSession(request.SessionId);

    try
    {
      var browser = session.Value.Item1;
      var page = session.Value.Item2;

      var codeInput = await page.QuerySelectorAsync("#code");
      var codeContinueButton = await page.QuerySelectorAsync("#continue-btn");

      if (!(codeInput != null && codeContinueButton != null))
      {
        throw new Exception(nameof(Messages.OpeningPageUnsuccessful));
      }

      await codeInput.TypeAsync(request.VerificationCode, new TypeOptions { Delay = 150 });

      await page.Mouse.MoveAsync(300, 500);
      await page.Mouse.DownAsync();
      await page.Mouse.UpAsync();

      await codeContinueButton.ClickAsync();
      await page.WaitForNavigationAsync();

      //////// set the false verification responce here
      if (false)
      {
        throw new Exception(nameof(Messages.OpeningPageUnsuccessful));
      }

      var codeContinueButton2 = await page.QuerySelectorAsync("#continue-btn");

      if (codeContinueButton2 == null)
      {
        throw new Exception(nameof(Messages.OpeningPageUnsuccessful));
      }

      // var cookiesBefore = await page.GetCookiesAsync();
      // foreach (var cookie in cookiesBefore)
      // {
      //   Console.WriteLine($"COOKIES {cookie.Name} = {cookie.Value}");
      // }

      await Task.WhenAll(
          page.WaitForNavigationAsync(new NavigationOptions { WaitUntil = new[] { WaitUntilNavigation.Networkidle0 } }),
          codeContinueButton2.ClickAsync()
      );

      var _continueButton = await page.QuerySelectorAsync("#_continue");

      if (_continueButton == null)
      {
        throw new Exception(nameof(Messages.OpeningPageUnsuccessful));
      }

      await Task.WhenAll(
        page.WaitForNavigationAsync(new NavigationOptions { WaitUntil = new[] { WaitUntilNavigation.Networkidle0 } }),
        _continueButton.ClickAsync()
      );

      var securityQuestion = await page.EvaluateFunctionAsync<string>(@"() => {
        const el = [...document.querySelectorAll('strong')]
          .find(el => el.innerText.includes('My favorite'));
        return el ? el.innerText : '';
      }");

      var securityQuestionInput = await page.QuerySelectorAsync("#answer");
      var securityQuestionContinueButton = await page.QuerySelectorAsync("#_continue");

      if (!(securityQuestion != null && securityQuestionInput != null && securityQuestionContinueButton != null))
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

      await securityQuestionInput.TypeAsync(question.Answer, new TypeOptions { Delay = 150 });

      await page.Mouse.MoveAsync(300, 500);
      await page.Mouse.DownAsync();
      await page.Mouse.UpAsync();

      await securityQuestionContinueButton.ClickAsync();
      await page.WaitForNavigationAsync();

      //////// Check for false security Question answer

      result.Data = new Update()
      {
        UpdateStep = Api.Services.Enums.UpdateSteps.Completed,
        SessionId = request.SessionId,
      };
      result.IsSuccess = true;

      _browserService.SetPage(request.SessionId, page);

    }
    catch (System.Exception ex)
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