using PuppeteerSharp;
using PuppeteerSharp.Input;

public class PuppeteerSharpUtilities() : IPuppeteerSharpUtilities
{
  public async Task TypeAndContinue(IPage page, string button, string? input = null, string? value = null, string? input2 = null, string? value2 = null)
  {
    var Button = await page.QuerySelectorAsync($"#{button}");
    if (Button == null)
      throw new Exception(nameof(Messages.OpeningPageUnsuccessful));

    if (input != null && value != null)
    {
      var Input = await page.QuerySelectorAsync($"#{input}");

      if (Input == null)
        throw new Exception(nameof(Messages.OpeningPageUnsuccessful));

      await Input.TypeAsync(value, new TypeOptions { Delay = 150 });

      if (input2 != null && value2 != null)
      {
        var Input2 = await page.QuerySelectorAsync($"#{input2}");

        if (Input2 == null)
          throw new Exception(nameof(Messages.OpeningPageUnsuccessful));

        await Input2.TypeAsync(value2, new TypeOptions { Delay = 150 });
      }
    }

    await page.Mouse.MoveAsync(300, 500);
    await page.Mouse.DownAsync();
    await page.Mouse.UpAsync();

    await Task.WhenAll(
      page.WaitForNavigationAsync(new NavigationOptions { WaitUntil = new[] { WaitUntilNavigation.Networkidle0 } }),
      Button.ClickAsync()
    );
  }
}