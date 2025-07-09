using PuppeteerSharp;
using Api.Services.Browser;

public class UpdateLogin : IUpdateLogin
{
  private readonly BrowserService _browserService;
  private readonly IPuppeteerSharpUtilities _puppeteerSharpUtilities;

  public UpdateLogin(BrowserService browserService, IPuppeteerSharpUtilities puppeteerSharpUtilities)
  {
    _browserService = browserService;
    _puppeteerSharpUtilities = puppeteerSharpUtilities;
  }

  public async Task<ResultClass<Update>> LoginForUpdate(LoginForUpdateDTO request)
  {
    var result = new ResultClass<Update>() { Data = new Update(), MessageKey = new List<string>() };

    var sessionId = await _browserService.CreateSessionAsync();

    try
    {

      var session = _browserService.GetSession(sessionId);
      var browser = session.Value.Item1;
      var page = session.Value.Item2;

      int requestCount = 0;

      page.Request += (sender, e) =>
      {
        requestCount++;
        Console.WriteLine($"Request #{requestCount}: {e.Request.Url}");
      };


      await page.GoToAsync("https://www.canada.ca/en/immigration-refugees-citizenship/services/application/account.html#alerts", new NavigationOptions
      {
        WaitUntil = new[] { WaitUntilNavigation.Load }
      });

      var FirstPageElementHandle = await page.XPathAsync("//a[span[span[text()='GCKey username and password']]]");

      var FirstPagejsHandle = FirstPageElementHandle[0] as ElementHandle;

      if (FirstPagejsHandle == null)
      {
        throw new Exception(nameof(Messages.OpeningPageUnsuccessful));
      }

      var FirstPageHref = await (await FirstPagejsHandle.GetPropertyAsync("href")).JsonValueAsync<string>();
      await page.GoToAsync(FirstPageHref, new NavigationOptions
      {
        WaitUntil = new[] { WaitUntilNavigation.Load }
      });

      await _puppeteerSharpUtilities.TypeAndContinue(page, "button", "token1", request.Username, "token2", request.Password);


      var errorSpans = await page.EvaluateFunctionAsync<string[]>(@"() => {
        return [...document.querySelectorAll('span.inputError')].map(el => el.innerText);
      }");

      if (errorSpans.Count() != 0)
      {
        throw new Exception(nameof(Messages.WrongUsernamePassword));
      }

      var yourLastSignedInPhrase = await page.EvaluateFunctionAsync<string>(@"() => {
        const el = [...document.querySelectorAll('p')]
          .find(el => el.innerText.includes('You last signed in'));
        return el ? el.innerText : '';
      }");

      if (yourLastSignedInPhrase == null)
      {
        throw new Exception(nameof(Messages.OpeningPageUnsuccessful));
      }


      await _puppeteerSharpUtilities.TypeAndContinue(page, "continue");


      var codeInput = await page.QuerySelectorAsync("#code");
      var codeContinueButton = await page.QuerySelectorAsync("#continue-btn");

      if (!(codeInput != null && codeContinueButton != null))
      {
        throw new Exception(nameof(Messages.OpeningPageUnsuccessful));
      }

      result.Data = new Update()
      {
        UpdateStep = Api.Services.Enums.UpdateSteps.WaitingForVerificationCode,
        SessionId = sessionId,
      };
      result.IsSuccess = true;

      _browserService.SetPage(sessionId, page);

    }
    catch (Exception ex)
    {
      _browserService.CloseSessionAsync(sessionId);

      result.MessageKey.Add(ex.Message);
    }

    return result;
  }
}