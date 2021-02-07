using System;
using System.Threading.Tasks;
using System.Collections.Generic;

public interface IWMBI
{
    Task Start(string baseUri);
    Task SetLanguage(string lang);
    string GetTranslation(string key);
    string GetCurrentLang();
    void SetCallback(Action callback);
    bool IsLoaded();
    bool HasError();
}