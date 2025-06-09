using System;
using System.Diagnostics;
using PuppeteerSharp;

namespace Api.Services.Browser;

public class BrowserService
{
  private readonly Dictionary<string, (IBrowser Browser, IPage Page)> _sessions = new();

  public async Task<string> CreateSessionAsync()
  {
    var chromePath = @"C:\Program Files\Google\Chrome\Application\chrome.exe";
    var userDataDir = @"C:\ChromeAutomationProfile";

    var startInfo = new ProcessStartInfo
    {
      FileName = chromePath,
      Arguments = $"--remote-debugging-port=9222 --user-data-dir=\"{userDataDir}\"",
      UseShellExecute = false,
      CreateNoWindow = false // set to true if you don’t want a console window
    };

    Process.Start(startInfo);


    var browser = await Puppeteer.ConnectAsync(new ConnectOptions
    {
      BrowserURL = "http://localhost:9222"
    });

    var page = await browser.NewPageAsync();
    await page.SetUserAgentAsync("Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/114.0.0.0 Safari/537.36");

    var sessionId = Guid.NewGuid().ToString();
    _sessions[sessionId] = (browser, page);

    return sessionId;
  }

  public (IBrowser, IPage)? GetSession(string sessionId)
  {
    if (_sessions.TryGetValue(sessionId, out var session))
      return session;
    return null;
  }
  public void SetPage(string sessionId, IPage newPage)
  {
    if (_sessions.TryGetValue(sessionId, out var session))
    {
      _sessions[sessionId] = (session.Browser, newPage);
    }
  }


  public async Task CloseSessionAsync(string sessionId)
  {
    if (_sessions.TryGetValue(sessionId, out var session))
    {
      await session.Page.CloseAsync();
      await session.Browser.CloseAsync();
      _sessions.Remove(sessionId);
    }
  }

}
