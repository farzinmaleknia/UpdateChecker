using System;
using PuppeteerSharp;

namespace Api.Services.Browser;

public class BrowserService
{
    private readonly Dictionary<string, (IBrowser Browser, IPage Page)> _sessions = new();

    public async Task<string> CreateSessionAsync()
    {        
        var currentDirectory = Directory.GetCurrentDirectory();
        var chromePath = Path.Combine(currentDirectory, @"bin\Debug\net8.0\Chrome\Win64-132.0.6834.83\chrome-win64\chrome.exe");

        if (!File.Exists(chromePath))
        {
            var _browserFetcher = new BrowserFetcher();
            await _browserFetcher.DownloadAsync();
        }

        var options = new LaunchOptions
        {
            Headless = true,
            ExecutablePath = chromePath
        };

        var browser = await Puppeteer.LaunchAsync(options);
        var page = await browser.NewPageAsync();

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
