using System;
using System.Threading.Tasks;

public interface IWMBI
{
    void Configure(string defaultLanguage, string defaultFileName, string defaultFilePath, string defaultStorageType);
    Task Start(string baseUri);
    Task SetLanguage(string lang);
    string GetTranslation(string key);
    string GetCurrentLang();
    void SetCallback(Action callback);
    bool IsLoaded();
    bool HasError();
}
