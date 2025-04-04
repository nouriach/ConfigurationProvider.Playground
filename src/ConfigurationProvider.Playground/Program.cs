using ConfigurationProvider.Playground.Extensions;

var builder = WebApplication.CreateBuilder();

// configure your app

var uri = builder.Configuration.GetValue<string>("BreweryApi:Uri");
builder.Configuration.AddApiConfiguration(c =>
{
    c.RequestBaseUrl = uri ?? string.Empty;
    c.Optional = false;
});

// var apiProvider = apiConfigSource.Build(builder.Configuration) as ApiConfigurationProvider;
// apiProvider?.Load(); 

var corsPolicy = builder.Configuration.GetValue<string>("Cors:Policy"); 
var allowedOrigins = builder.Configuration.GetValue<string[]>("AllowedOrigins");

builder.Services.AddCors(options =>
{
    var domains = new List<string>();
    var index = 0;
    
    while (true)
    {
        var key = $"AllowedOrigins__{index}";
        var value = builder.Configuration[key];
        if (value is null) break;
        
        domains.Add(value);
        index++;
    }

    options.AddPolicy(corsPolicy ?? string.Empty, policy =>
    {
        policy.WithOrigins(domains.ToArray())
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

var app = builder.Build();

// build your app

app.UseCors(corsPolicy ?? string.Empty);

app.MapGet("/allowedOrigins", () =>
{
    var response = new
    {
        ConfigResultOne = builder.Configuration["AllowedOrigins__0"],
        ConfigResultTwo = builder.Configuration["AllowedOrigins__1"],
        ConfigResultThree = builder.Configuration["AllowedOrigins__2"],
        ConfigResultFour = builder.Configuration["AllowedOrigins__3"]
    };

    return Results.Ok(response);
});

app.Run();