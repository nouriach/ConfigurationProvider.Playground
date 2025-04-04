using ConfigurationProvider.Playground.Configuration;

namespace ConfigurationProvider.Playground.Extensions;

public static class ApiExtensions
{
    public static ApiConfigurationSource AddApiConfiguration(
        this IConfigurationBuilder builder, Action<ApiConfigurationSource> configure)
    {
        var source = new ApiConfigurationSource();
        
        configure(source);
  
        builder.Add(source);
        return source;
    } 
}