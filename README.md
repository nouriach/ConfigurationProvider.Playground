# Dynamic CORS Configuration in .NET Using Configuration Provider 📬
## Retrieve and update allowed client domains dynamically from an external API.

A Configuration Provider in .NET enables flexible, extensible application setting
by allowing you to load configuration data from custom sources like APIs, databases,
or files. It supports dynamic configuration, centralized management, and separation
of concerns, enhancing maintainability and adaptability across different environments
without hardcoding values into the application.

## Read the article 📰

> [Dynamic CORS Configuration in .NET Using Custom Configuration Provider](https://medium.com/the-tech-collective/dynamic-cors-configuration-in-net-using-a-custom-configuration-provider-707adbbc3f94)

## User Instructions 🔖

To interact with this repo you will need to have .NET installed on your machine. You can download the latest .NET version [here]("https://dotnet.microsoft.com/en-us/download").
This app is using .NET 8.

To monitor the code flow, run the app in debug mode and set breakpoints at key points of interest. The console logs will offer additional context.

Once the app starts, the user can validate that the config variables have been set by hitting this endpoint: /allowedOrigins.

## Dev Notes 🗒️

When building a new custom Configuration Provider, there are two main class: `IConfigurationSource` & `ConfigurationProvider` that need to be implemented.

### `IConfigurationSource`

`IConfigurationSource` defines a contract for creating `IConfigurationProvider` instances. It allows custom configuration sources to be added to the configuration 
system, enabling flexible data retrieval from various sources. In this repo, our source is an external API. The external API's Request Uri is stored in this class.
If the external source provider was a database, then key database connection details would be stored here.

### `ConfigurationProvider`

`ConfigurationProvider` is an abstract base class in .NET for loading key-value configuration data. It manages the data store and provides methods to load, set, and
retrieve configuration values efficiently. In this example, we leverage the `Load` and `OnReload` methods, as well as the `Data` field to store our retrieved config
data globally.

## Considerations 🤔

The Load method in ConfigurationProvider must run synchronously, as it's called during configuration initialization. Because of this, even if fetching data is asynchronous,
`Load` cannot be declared or executed asynchronously. As a result, the code does look a bit laborious:

```
var response = client.GetAsync(url).ConfigureAwait(false).GetAwaiter().GetResult();
if (!response.IsSuccessStatusCode)
    CheckOptional();

var responseContent = response.Content.ReadAsStringAsync().ConfigureAwait(false).GetAwaiter().GetResult();
```

Using a configuration provider can tightly couple your app to an external data source at startup, potentially delaying or preventing 
startup if the source is slow, unreachable, or misconfigured. It's important to handle exceptions gracefully to ensure the app fails 
safely or falls back to default configuration.

## Resources 📚

- [Implement a Custom Configuration Provider in .NET 7](https://medium.com/@gokerakce/implement-a-custom-configuration-provider-in-net-7-c0a195dcd05f)
- [Adding RESTful API Configuration Provider For Our ASP.NET Core Application](https://www.csharp.com/article/adding-restful-api-configuration-provider-for-our-asp-net-core-application/)
- [IConfigurationSource Interface](https://learn.microsoft.com/en-us/dotnet/api/microsoft.extensions.configuration.iconfigurationsource?view=net-9.0-pp)
- [Custom Configuration Provider in .NET: Step-by-Step Guide](https://medium.com/@vosarat1995/custom-configuration-provider-in-net-step-by-step-guide-3d8a3a8f7203)
- [SQL Server as a Configuration Provider | IConfiguration with a Database](https://www.youtube.com/watch?v=gq4wR0lCz8g)