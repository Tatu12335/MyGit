using Microsoft.Extensions.DependencyInjection;
using MyGit.Core.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using Spectre.Console.Cli;
namespace MyGit.Core.Middleware
{
    public class TypeRegistrar : ITypeRegistrar
    {
        private readonly IServiceCollection _services;

        public TypeRegistrar(IServiceCollection services) => _services = services;

        public ITypeResolver Build() => new TypeResolver(_services.BuildServiceProvider());

        public void Register(Type service, Type implementation) =>
            _services.AddSingleton(service, implementation);

        public void RegisterInstance(Type service, object implementation) =>
            _services.AddSingleton(service, implementation);

        public void RegisterLazy(Type service, Func<object> factory)
        {
            if (factory is null) throw new ArgumentNullException(nameof(factory));
            _services.AddSingleton(service, _ => factory());
        }
    }
}
