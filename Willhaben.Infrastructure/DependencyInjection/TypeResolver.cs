using System;
using Spectre.Console.Cli;

namespace Willhaben.Infrastructure.DependencyInjection;

public sealed class TypeResolver : ITypeResolver
{
    private readonly IServiceProvider _provider;

    public TypeResolver(IServiceProvider provider)
    {
        _provider = provider;
    }

    public object Resolve(Type type)
    {
        return _provider.GetService(type);
    }
}