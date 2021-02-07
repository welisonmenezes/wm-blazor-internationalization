using System;
using System.Web;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.JSInterop;

public sealed class WMBIStrategy: IWMBI
{
    private readonly IJSRuntime jsRuntime;
    private readonly HttpClient httpClient;
    private Dictionary<string, string> translations;
    private Action callback;
    private bool isLoaded = false;
    private bool hasError = false;
    private string currentLang;
    private string fileName;
    private string filePath;

    public WMBIStrategy(IJSRuntime jsRuntime, HttpClient httpClient)
    {
        this.jsRuntime = jsRuntime;
        this.httpClient = httpClient;
        this.translations = new Dictionary<string, string>();
        System.Console.WriteLine("WMBIStrategy");
    }

    public async void Configure(
        string defaultLanguage,
        string defaultFileName,
        string defaultFilePath)
    {
        System.Console.WriteLine("Configure Start");
        this.fileName = defaultFileName;
        this.filePath = defaultFilePath;
        await SetLanguage(defaultLanguage);
        System.Console.WriteLine("Configure End");
    }

    private async Task<Dictionary<string, string>> SetLanguageDicitonary()
    {
        this.isLoaded = false;
        this.hasError = false;
        var httpRes = await this.httpClient.GetAsync(
            $"{this.httpClient.BaseAddress}{this.filePath}{this.fileName}.{this.currentLang}.json"
        );
        if (httpRes.StatusCode != HttpStatusCode.NotFound) 
        {
            var contentBytes = await httpRes.Content.ReadAsByteArrayAsync();
            this.translations = JsonSerializer.Deserialize<Dictionary<string, string>>(contentBytes);
            System.Console.WriteLine("SetLanguageDicitonary");
            this.isLoaded = true;
            this.RunCallback();
            return this.translations;
        }
        this.isLoaded = true;
        this.hasError = true;
        this.RunCallback();
        return this.translations;
    }

    public async Task SetLanguage(string lang)
    {
        System.Console.WriteLine("SetLanguage: " + lang);
        this.currentLang = lang;
        await SetLanguageDicitonary();
    }

    public string GetTranslation(string key)
    {
        System.Console.WriteLine("GetTranslation");
        string value;
        if (translations.TryGetValue(key, out value))
        {
            return value;
        }
        return null;
    }

    private void RunCallback()
    {
        System.Console.WriteLine("RunCallback");
        this.callback();
    }

    public void SetCallback(Action callback)
    {
        System.Console.WriteLine("SetCallback");
        this.callback = callback;
    }

    public string GetCurrentLang()
    {
        return this.currentLang;
    }

    public bool IsLoaded()
    {
        return this.isLoaded;
    }

    public bool HasError()
    {
        return this.hasError;
    }
}
