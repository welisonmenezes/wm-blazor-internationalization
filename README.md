# WM Blazor Internationalization

## Summary

This package is an another way to localize text and html blocks in your Blazor Web App!
(internationalization, localization, translation, ...)

- Version: 5.0
- It works on both Blazor Server and Blazor WebAssembly.
- Your translations come from a json file.

## NuGet Package
https://www.nuget.org/packages/BlazorUniversalAnalytics/

## GitHub Repository
https://github.com/welisonmenezes/wm-blazor-internationalization

## Configuration

First, import the namespaces in `_Imports.razor`

```
@using WMBlazorInternationalization
@using WMBlazorInternationalization.WMBI
```

Then, add the `WMBInternationalization` component to wrap all the components in your `App.razor`.<br/>
You can do it by wrapping the Router component of the aplication.
Note that the `ChildContent` must wrap the app content, the `ChildLoading` must wrap the content shown when the localization is loading and the `ChildError` must wrap ther error content when the localization is fail. 

```
<WMBInternationalization>
    <ChildContent>
        <Router AppAssembly="@typeof(Program).Assembly">
            <Found Context="routeData">
                <RouteView RouteData="@routeData" DefaultLayout="@typeof(MainLayout)" />
            </Found>
            <NotFound>
                <LayoutView Layout="@typeof(MainLayout)">
                    <p>Sorry, there's nothing at this address.</p>
                </LayoutView>
            </NotFound>
        </Router>
    </ChildContent>
    <ChildLoading>
        <p>Loading your localization data.</p>
    </ChildLoading>
    <ChildError>
        <p>An error was occurred while loading your localization.</p>
    </ChildError>
</WMBInternationalization>
```

## Setting up the Component

Inside your main `Startup`/`Program`, call `AddWMBlazorInternationalization`. This will configure the app.

Blazor Wasm:
```
builder.Services.AddWMBlazorInternationalization();
```

Blazor Server:
```
services.AddWMBlazorInternationalization();
```

### Parameters

You can also customize some things by passing parameters

```
builder.Services.AddWMBlazorInternationalization(defaultLanguage, fileName, filePath, storageType);
```

- `defaultLanguage`: The default is `en`, but you can choose anyone or `null`;
- `fileName`: The default is `Locale`, but you can choose anyone or `null`;
- `filePath`: The default is `i18ntext/`, but you can choose anyone or `null`;
- `storageType`: The default is `localStorage`, but you can choose anyone or `null`;

When parameters are `null`, the default values will be assumed.

## Setting up the translations

Inside the `wwwroot` directory, create a folder called `i18ntext` and, inside that, create a json file for each language you want.
That is importante to name this files as following:

`Locale.[language-code].json`

Examples: 
```
Locale.en.json
Locale.pt.json
Locale.pt-br.json
Locale.es.json
```

Note that `i18next` and `Locale` values can be changed as shown above.

## Create localized text source files as JSON

Example:
```
{
    "Key1": "Localized text 1",
    "Key2": "Localized text 2",
    ...
}

## Using in pages and components

Inside your component, get the WMBInternationalization functionalities by the CascadingParameter named `CascadeWMBI`

```
@code {
    [CascadingParameter(Name = "CascadeWMBI")]
    protected WMBInternationalization WMBI { get; set; }
}
```

Now you has access to 3 methods: `GetTranslation`, `SetLanguage` and `GetCurrentLang`.

## GetTranslation

By `GetTranslation` you can read any key property from the json file regarding current language.

Example: 
```
<h1>@WMBI.GetTranslation("HelloWorld")</h1>
```

## SetLanguage

By `SetLanguage` you can choose another language. 

Example: 
```
WMBI.SetLanguage("pt");
```

## GetCurrentLang

By `GetCurrentLang` you can see the current language code.

Example: 
```
@WMBI.GetCurrentLang()
```

By this method you can even choose show some html blocks depending on the selected language.

Example: 
```
@if(@WMBI.GetCurrentLang() == "en")
{
    <img src="img/reino-unido.png">
}

@if(@WMBI.GetCurrentLang() == "pt")
{
    <img src="img/brasil.png">
}
```

### For live demo, download it from github and run, either BlazorServerDemo or BlazorWasmDemo project.