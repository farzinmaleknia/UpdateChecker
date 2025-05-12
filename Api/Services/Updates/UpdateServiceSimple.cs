using PuppeteerSharp;
using Api.Models.ResultClass;
using Microsoft.AspNetCore.ResponseCaching;

namespace Api.Services.Updates;

public class UpdateServiceSimple 
{
    private BrowserFetcher? _browserFetcher {get; set;} 

    public async Task<ResultClass<Update>> FetchAllUpdates()
    {
        var result = new ResultClass<Update>(){Data = new Update()};

        if(_browserFetcher == null)
        {
            _browserFetcher = new BrowserFetcher();
            await _browserFetcher.DownloadAsync();
        }

        using var browser = await Puppeteer.LaunchAsync(new LaunchOptions { Headless = true });
        using var page = await browser.NewPageAsync();
        await page.GoToAsync("https://www.canada.ca/en.html");

        var title = await page.GetTitleAsync();

        var elementHandle = await page.EvaluateFunctionHandleAsync(@"() => {
            return [...document.querySelectorAll('a')]
                .find(el => el.textContent.includes('Sign in to an account'));
            }");

        var jsHandle = elementHandle as ElementHandle;

        if (jsHandle != null)
        {
            await jsHandle.ClickAsync();
            await page.WaitForNavigationAsync(new NavigationOptions { WaitUntil = new[] { WaitUntilNavigation.Load } });

            var secondElementHandle = await page.EvaluateFunctionHandleAsync(@"() => {
                return [...document.querySelectorAll('a')]
                    .find(el => el.textContent.includes('Sign in to your IRCC account'));
                }");

            var secondjsHandle = secondElementHandle as ElementHandle;

            if (secondjsHandle != null)
            {
                await secondjsHandle.ClickAsync();
                await page.WaitForNavigationAsync(new NavigationOptions { WaitUntil = new[] { WaitUntilNavigation.Load } });

                var thirdElementHandle = await page.EvaluateFunctionHandleAsync(@"() => {
                    return [...document.querySelectorAll('button')]
                        .find(el => el.textContent.includes('Sign in to your existing account'));
                    }");


                var thirdjsHandle = thirdElementHandle as ElementHandle;

                if (thirdjsHandle != null)
                {
                    await thirdjsHandle.ClickAsync();
                    await page.WaitForFunctionAsync(@"() => {
                        const buttons = Array.from(document.querySelectorAll('button'));
                        return buttons.some(btn => btn.innerText.includes('Visitor visa')); 
                    }");

                    var fourthElementHandle = await page.EvaluateFunctionHandleAsync(@"() => {
                        return [...document.querySelectorAll('button')]
                            .find(el => el.textContent.includes('Visitor visa'));
                        }");


                    var fourthjsHandle = fourthElementHandle as ElementHandle;

                    if (fourthjsHandle != null)
                    {
                        await fourthjsHandle.ClickAsync();
                        await page.WaitForFunctionAsync(@"() => {
                            const links = Array.from(document.querySelectorAll('a'));
                            return links.some(a => a.innerText.includes('IRCC secure account (GCKey or Sign-In Partner)')); 
                        }");

                        var fifthElementHandle = await page.EvaluateFunctionHandleAsync(@"() => {
                            return [...document.querySelectorAll('a')]
                                .find(el => el.textContent.includes('IRCC secure account (GCKey or Sign-In Partner)'));
                        }");


                        var fifthjsHandle = fifthElementHandle as ElementHandle;

                        if (fifthjsHandle != null)
                        {
                            var href = await ( await fifthjsHandle.GetPropertyAsync("href")).JsonValueAsync<string>();
                            await page.GoToAsync(href);
                            await browser.WaitForTargetAsync(
                                target => target.Type == TargetType.Page && target.Url != page.Url
                            ).ContinueWith(async t => await t.Result.PageAsync());

                            var sixthElementHandle = await page.XPathAsync("//a[span[span[text()='GCKey username and password']]]");
                            
                            var sixthjsHandle = sixthElementHandle[0] as ElementHandle;

                            if (sixthjsHandle != null)
                            {
                                var sixthHref = await ( await sixthjsHandle.GetPropertyAsync("href")).JsonValueAsync<string>();
                                await page.GoToAsync(sixthHref);
                                await page.WaitForFunctionAsync(@"() => {
                                    return document.querySelector('#token1');
                                }");

                                //await page.WaitForNavigationAsync(new NavigationOptions { WaitUntil = new[] { WaitUntilNavigation.Load } });

                                var usernameInput = await page.EvaluateFunctionHandleAsync(@"() => {
                                    return document.querySelector('#token1')
                                }");

                                var usernamePlaceHolder = await page.EvaluateFunctionAsync<string>("el => el.getAttribute('placeholder')", usernameInput);

                                var passInput = await page.EvaluateFunctionHandleAsync(@"() => {
                                    return document.querySelector('#token2')
                                }");
                                
                                var passPlaceHolder = await page.EvaluateFunctionAsync<string>("el => el.getAttribute('placeholder')", passInput);

                                //result.Data = new Update() {WebContent = usernamePlaceHolder};
                                result.IsSuccess = true;
                                return result;

                            }
                            else
                            {
                                result.MessageKey = "sixth Element not found or not clickable.";
                                return result;
                            }  
                        }
                        else
                        {
                            result.MessageKey = "fifth Element not found or not clickable.";
                            return result;
                        }   
                    }
                    else
                    {
                        result.MessageKey = "fourth Element not found or not clickable.";
                        return result;
                    }   
                }
                else
                {
                    result.MessageKey = "third Element not found or not clickable.";
                    return result;
                }
            }
            else
            {
                result.MessageKey = "Second Element not found or not clickable.";
                return result;
            }
        }
        else
        {
            result.MessageKey = "First Element not found or not clickable.";
            return result;
        }

    }
}
