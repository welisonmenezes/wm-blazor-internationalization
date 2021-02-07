using System;
using System.Threading.Tasks;
using System.Collections.Generic;

public interface IWMBI
{
    Task SetLanguage(string lang);
    string GetTranslation(string key);
    string GetCurrentLang();
    void SetCallback(Action callback);
    bool IsLoaded();
    bool HasError();
}