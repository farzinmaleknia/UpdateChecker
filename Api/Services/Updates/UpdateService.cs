using PuppeteerSharp;
using Api.Models.ResultClass;
using Microsoft.AspNetCore.ResponseCaching;

namespace Api.Services.Updates;

public class UpdateService : IUpdateService
{
    private BrowserFetcher? _browserFetcher {get; set;} 

    public async Task<ResultClass<Update>> FetchAllUpdates()
    {
        var result = new ResultClass<Update>();

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
            await Task.WhenAll(
                page.WaitForNavigationAsync(new NavigationOptions { WaitUntil = new[] { WaitUntilNavigation.Load } }),
                jsHandle.ClickAsync()
            );

            var secondElementHandle = await page.EvaluateFunctionHandleAsync(@"() => {
                return [...document.querySelectorAll('a')]
                    .find(el => el.textContent.includes('Sign in to your IRCC account'));
                }");

            var secondjsHandle = secondElementHandle as ElementHandle;

            if (secondjsHandle != null)
            {
                await Task.WhenAll(
                    page.WaitForNavigationAsync(new NavigationOptions { WaitUntil = new[] { WaitUntilNavigation.Load } }),
                    jsHandle.ClickAsync()
                );

                var thirdElementHandle = await page.EvaluateFunctionHandleAsync(@"() => {
                    return [...document.querySelectorAll('button')]
                        .find(el => el.textContent.includes('Sign in to your existing account'));
                    }");


                var thirdjsHandle = thirdElementHandle as ElementHandle;

                if (thirdjsHandle != null)
                {
                    await Task.WhenAll(
                        page.WaitForFunctionAsync(@"() => {
                            const buttons = Array.from(document.querySelectorAll('button'));
                            return buttons.some(btn => btn.innerText.includes('Visitor visa')); 
                        }"),
                        thirdjsHandle.ClickAsync()
                    );

                    var fourthElementHandle = await page.EvaluateFunctionHandleAsync(@"() => {
                        return [...document.querySelectorAll('button')]
                            .find(el => el.textContent.includes('Visitor visa'));
                        }");


                    var fourthjsHandle = fourthElementHandle as ElementHandle;

                    if (fourthjsHandle != null)
                    {
                        await Task.WhenAll(
                            page.WaitForFunctionAsync(@"() => {
                                const links = Array.from(document.querySelectorAll('a'));
                                return links.some(a => a.innerText.includes('IRCC secure account (GCKey or Sign-In Partner)')); 
                            }"),
                            fourthjsHandle.ClickAsync()
                        );

                        var fifthElementHandle = await page.EvaluateFunctionHandleAsync(@"() => {
                            return [...document.querySelectorAll('button')]
                                .find(el => el.textContent.includes('IRCC secure account (GCKey or Sign-In Partner)'));
                            }");


                        var fifthjsHandle = fifthElementHandle as ElementHandle;

                        if (fifthjsHandle != null)
                        {
                            await Task.WhenAll(
                                browser.WaitForTargetAsync(
                                    target => target.Type == TargetType.Page && target.Url != page.Url
                                ).ContinueWith(async t => await t.Result.PageAsync()),
                                fifthjsHandle.ClickAsync()
                            );

                            var sixthElementHandle = await page.EvaluateFunctionHandleAsync(@"() => {
                                return [...document.querySelectorAll('button')]
                                    .find(el => el.textContent.includes('IRCC secure account (GCKey or Sign-In Partner)'));
                                }");

                            var sixthjsHandle = sixthElementHandle as ElementHandle;

                            if (sixthjsHandle != null)
                            {
                                await Task.WhenAll(
                                    browser.WaitForTargetAsync(
                                        target => target.Type == TargetType.Page && target.Url != page.Url
                                    ).ContinueWith(async t => await t.Result.PageAsync()),
                                    sixthjsHandle.ClickAsync()
                                );

                                var seventhElementHandle = await page.XPathAsync("//a[span[text()='GCKey username and password']]");
                                

                                var seventhjsHandle = seventhElementHandle as ElementHandle[];

                                if (seventhjsHandle != null)
                                {
                                    await Task.WhenAll(
                                        browser.WaitForTargetAsync(
                                            target => target.Type == TargetType.Page && target.Url != page.Url
                                        ).ContinueWith(async t => await t.Result.PageAsync()),
                                        seventhjsHandle[0].ClickAsync()
                                    );

                                    // var seventhElementHandle = await page.XPathAsync("//a[span[text()='GCKey username and password']]");
                                    

                                }
                                else
                                {
                                    result.Message = "Sixth Element not found or not clickable.";
                                    return result;
                                }  



                            }
                            else
                            {
                                result.Message = "Sixth Element not found or not clickable.";
                                return result;
                            }  

                        }
                        else
                        {
                            result.Message = "fifth Element not found or not clickable.";
                            return result;
                        }   

                    }
                    else
                    {
                        result.Message = "fourth Element not found or not clickable.";
                        return result;
                    }   

                }
                else
                {
                    result.Message = "third Element not found or not clickable.";
                    return result;
                }

            }
            else
            {
                result.Message = "Second Element not found or not clickable.";
                return result;
            }

        }
        else
        {
            result.Message = "First Element not found or not clickable.";
            return result;
        }

        var res = new Update(){WebContent = title};

        result.Data = res;
        result.IsSuccess = true;

        return result;
    }
}
