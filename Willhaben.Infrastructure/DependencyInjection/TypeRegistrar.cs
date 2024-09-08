
using System;
using Microsoft.Extensions.DependencyInjection;
using Spectre.Console.Cli;

namespace Willhaben.Infrastructure.DependencyInjection;
public sealed class TypeRegistrar : ITypeRegistrar
{
    private readonly IServiceCollection _services;

    public TypeRegistrar(IServiceCollection services)
    {
        _services = services;
    }

    public void RegisterLazy(Type service, Func<object> factory)
    {
        _services.AddSingleton(service, provider => factory());
    }

    public ITypeResolver Build()
    {
        return new TypeResolver(_services.BuildServiceProvider());
    }

    public void Register(Type service, Type implementation)
    {
        _services.AddSingleton(service, implementation);
    }

    public void RegisterInstance(Type service, object implementation)
    {
        _services.AddSingleton(service, implementation);
    }

    public void Register(Type service, Func<object> factory)
    {
        _services.AddSingleton(service, _ => factory());
    }

    // Add RegisterLazy method
    public void RegisterLazy(Type service, Func<ITypeResolver, object> factory)
    {
        _services.AddSingleton(service, provider =>
        {
            var resolver = new TypeResolver(provider);
            return factory(resolver);
        });
    }
}