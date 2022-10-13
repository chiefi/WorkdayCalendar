using Microsoft.Extensions.Hosting;
using Spectre.Console.Cli;

namespace WorkdayCalendar.Util;

/// <summary>
/// Custom TypeResolver to allow resolving of services in Spectre.Console.Cli
/// </summary>
internal sealed class TypeResolver : ITypeResolver, IDisposable
{
    private readonly IHost host;

    public TypeResolver(IHost provider)
    {
        host = provider ?? throw new ArgumentNullException(nameof(provider));
    }

    public object? Resolve(Type? type)
    {
        return type != null ? host.Services.GetService(type) : null;
    }

    public void Dispose()
    {
        host.Dispose();
    }
}