namespace ConfigurationProvider.Playground.Configuration;

public class ApiConfigurationSource : IConfigurationSource
{
    public string RequestBaseUrl { get; set; } = null!;
    public bool Optional { get; set; }

    public IConfigurationProvider Build(IConfigurationBuilder builder)
    {
        return new ApiConfigurationProvider(this);  
    }
}