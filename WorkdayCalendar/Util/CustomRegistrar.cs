using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Spectre.Console.Cli;

namespace WorkdayCalendar.Util;

/// <summary>
/// Custom registrar used for wrapping the hostbuilderI into a ITypeRegistrar that can be used 
/// with Specre.Console.Cli to allow dependency injection.
/// </summary>
internal sealed class CustomRegistrar : ITypeRegistrar
{
    private readonly IHostBuilder hostBuilder;

    public CustomRegistrar(IHostBuilder builder)
    {
        hostBuilder = builder;
    }

    public ITypeResolver Build()
    {
        return new TypeResolver(hostBuilder.Build());
    }

    public void Register(Type service, Type implementation)
    {
        hostBuilder.ConfigureServices((_, services) => services.AddSingleton(service, implementation));
    }

    public void RegisterInstance(Type service, object implementation)
    {
        hostBuilder.ConfigureServices((_, services) => services.AddSingleton(service, implementation));
    }

    public void RegisterLazy(Type service, Func<object> func)
    {
        if (func is null)
            throw new ArgumentNullException(nameof(func));

        hostBuilder.ConfigureServices((_, services) => services.AddSingleton(service, _ => func()));
    }
}