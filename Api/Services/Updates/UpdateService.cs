using PuppeteerSharp;
using Api.Models.ResultClass;
using Microsoft.AspNetCore.ResponseCaching;
using Api.Services.Browser;
using Microsoft.Extensions.Localization;
using System.Runtime.Versioning;

namespace Api.Services.Updates;

public class UpdateService : IUpdateService
{
    private readonly BrowserService _browserService;

    public UpdateService(BrowserService browserService){
        _browserService = browserService;
    }

    public async Task<ResultClass<Update>> LoginForUpdate(LoginForUpdateDTO request)
    {
        var result = new ResultClass<Update>(){Data = new Update()};

        try
        {
            var sessionId = await _browserService.CreateSessionAsync();

            var session = _browserService.GetSession(sessionId);
            var browser = session.Value.Item1;
            var page = session.Value.Item2;

            await page.GoToAsync("https://www.canada.ca/en/immigration-refugees-citizenship/services/application/account.html#alerts");
            await browser.WaitForTargetAsync(
                target => target.Type == TargetType.Page && target.Url != page.Url
            ).ContinueWith(async t => await t.Result.PageAsync());

            var FirstPageElementHandle = await page.XPathAsync("//a[span[span[text()='GCKey username and password']]]");
            
            var FirstPagejsHandle = FirstPageElementHandle[0] as ElementHandle;

            if (FirstPagejsHandle != null)
            {
                var FirstPageHref = await ( await FirstPagejsHandle.GetPropertyAsync("href")).JsonValueAsync<string>();
                await page.GoToAsync(FirstPageHref);
                await page.WaitForFunctionAsync(@"() => {
                    return document.querySelector('#token1');
                }");

                var usernameInput = await page.EvaluateFunctionHandleAsync(@"() => {
                    return document.querySelector('#token1')
                }");


                var passInput = await page.EvaluateFunctionHandleAsync(@"() => {
                    return document.querySelector('#token2')
                }");

                if (usernameInput != null && passInput != null)
                {
                    
                    var usernamePlaceHolder = await page.EvaluateFunctionAsync<string>("el => el.getAttribute('placeholder')", usernameInput);
                    var passPlaceHolder = await page.EvaluateFunctionAsync<string>("el => el.getAttribute('placeholder')", passInput);

                    result.Data = new Update() {
                        UpdateStep = Enums.UpdateSteps.WaitingForCredentials,
                        SessionId = sessionId,
                    };
                    result.IsSuccess = true;
                    return result;

                }
                else
                {
                    result.MessageKey = nameof(Messages.PleaseTryAgainOrCall);
                    return result;
                }  
                }
            else
            {
                result.MessageKey = nameof(Messages.PleaseTryAgainOrCall);
                return result;
            } 
        }
        catch (System.Exception ex)
        {
            result.MessageKey = ex.Message;
            return result;
        }

 

    }
}
