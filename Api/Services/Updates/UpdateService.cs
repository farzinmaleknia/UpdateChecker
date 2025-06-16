using PuppeteerSharp;
using Api.Models.ResultClass;
using Microsoft.AspNetCore.ResponseCaching;
using Api.Services.Browser;
using Microsoft.Extensions.Localization;
using System.Runtime.Versioning;
using PuppeteerSharp.Input;
using System.Security.Cryptography;

namespace Api.Services.Updates;

public class UpdateService : IUpdateService
{
  private readonly BrowserService _browserService;

  public UpdateService(BrowserService browserService)
  {
    _browserService = browserService;
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


      await page.GoToAsync("https://www.canada.ca/en/immigration-refugees-citizenship/services/application/account.html#alerts");
      await browser.WaitForTargetAsync(
        target => target.Type == TargetType.Page && target.Url != page.Url
      ).ContinueWith(async t => await t.Result.PageAsync());

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

      var usernameInput = await page.QuerySelectorAsync("#token1");

      var passInput = await page.QuerySelectorAsync("#token2");

      var submitButton = await page.QuerySelectorAsync("#button");

      if (!(usernameInput != null && passInput != null && submitButton != null))
      {
        var serverDown = await page.EvaluateFunctionAsync<string>(@"() => {
          const el = [...document.querySelectorAll('strong')]
            .find(el => el.innerText.includes('Our server is down'));
          return el ? el.innerText : '';
        }");

        if (String.IsNullOrEmpty(serverDown))
        {
          throw new Exception(nameof(Messages.OpeningPageUnsuccessful));

        }

        throw new Exception(nameof(Messages.ServerDown));
      }

      await page.Mouse.MoveAsync(300, 500);
      await page.Mouse.DownAsync();
      await page.Mouse.UpAsync();

      await usernameInput.TypeAsync(request.Username, new TypeOptions { Delay = 150 });
      await passInput.TypeAsync(request.Password, new TypeOptions { Delay = 150 });


      await submitButton.ClickAsync();
      await page.WaitForNavigationAsync();

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

      var continueButton = await page.QuerySelectorAsync("#continue");

      if (!(yourLastSignedInPhrase != null && continueButton != null))
      {
        throw new Exception(nameof(Messages.OpeningPageUnsuccessful));
      }

      await continueButton.ClickAsync();
      await page.WaitForNavigationAsync();

      var codeInput = await page.QuerySelectorAsync("#code");
      var codeContinueButton = await page.QuerySelectorAsync("#continue-btn");

      if (!(codeInput != null && codeContinueButton != null))
      {
        throw new Exception(nameof(Messages.OpeningPageUnsuccessful));
      }

      result.Data = new Update()
      {
        UpdateStep = Enums.UpdateSteps.WaitingForVerificationCode,
        SessionId = sessionId,
      };
      result.IsSuccess = true;

      _browserService.SetPage(sessionId, page);

      return result;

    }
    catch (System.Exception ex)
    {
      _browserService.CloseSessionAsync(sessionId);

      result.MessageKey.Add(ex.Message);
      return result;
    }
  }
}
