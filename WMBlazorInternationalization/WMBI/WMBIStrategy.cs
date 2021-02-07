using System;
using System.Web;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.JSInterop;

public sealed class WMBIStrategy : IWMBI
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
    private string storageType;

    private Task<IJSObjectReference> _module;
    private Task<IJSObjectReference> Module => _module ??= jsRuntime.InvokeAsync<IJSObjectReference>("import", "./_content/WMBlazorInternationalization/wm-blazor.interntationalization.js").AsTask();

    public WMBIStrategy(IJSRuntime jsRuntime, HttpClient httpClient)
    {
        this.jsRuntime = jsRuntime;
        this.httpClient = httpClient;
        this.translations = new Dictionary<string, string>();
    }

    public async void Configure(
        string defaultLanguage,
        string defaultFileName,
        string defaultFilePath,
        string defaultStorageType)
    {
        this.currentLang = defaultLanguage;
        this.fileName = defaultFileName;
        this.filePath = defaultFilePath;
        this.storageType = defaultStorageType;
        await StartDefaultLanguage();
    }

    private async Task<Dictionary<string, string>> SetLanguageDicitonary()
    {
        this.isLoaded = false;
        this.hasError = false;
        var httpRes = await this.httpClient.GetAsync($"{this.httpClient.BaseAddress}{this.filePath}{this.fileName}.{this.currentLang}.json");
        if (httpRes.StatusCode != HttpStatusCode.NotFound)
        {
            var contentBytes = await httpRes.Content.ReadAsByteArrayAsync();
            this.translations = JsonSerializer.Deserialize<Dictionary<string, string>>(contentBytes);
            this.isLoaded = true;
            this.RunCallback();
            return this.translations;
        }
        else
        {
            this.isLoaded = true;
            this.hasError = true;
            this.RunCallback();
            return this.translations;
        }
    }

    private async Task StartDefaultLanguage()
    {
        string theLang;
        if (!String.IsNullOrEmpty(this.storageType))
        {
            string persistedLang = await GetPersistedLanguage();
            theLang = (!String.IsNullOrEmpty(persistedLang)) ? persistedLang : this.currentLang;
        }
        else
        {
            theLang = this.currentLang;
        }
        await SetLanguage(theLang);
    }

    private void RunCallback()
    {
        this.callback();
    }

    private async Task PersistLanguage()
    {
        var module = await this.Module;
        await module.InvokeAsync<string>("WMBISetCurrentLanguage", this.currentLang, this.storageType);
    }

    private async Task<string> GetPersistedLanguage()
    {
        var module = await this.Module;
        return await module.InvokeAsync<string>("WMBIGetCurrentLanguage", this.storageType);
    }

    public async Task SetLanguage(string lang)
    {
        this.currentLang = lang;
        if (!String.IsNullOrEmpty(this.storageType))
        {
            await PersistLanguage();
        }
        await SetLanguageDicitonary();
    }

    public string GetTranslation(string key)
    {
        try 
        {
            return this.translations[key];
        }
        catch (Exception e)
        {
            System.Console.WriteLine(e.Message);
            return e.Message;
        }
    }

    public void SetCallback(Action callback)
    {
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
