using System;
using System.Collections.Generic;
using System.Text;
using Spectre.Console.Cli;

namespace MyGit.Core.Middleware
{
    public class TypeResolver : ITypeResolver, IDisposable
    {
        private readonly IServiceProvider _provider;

        public TypeResolver(IServiceProvider provider) => this._provider = provider;

        public object? Resolve(Type? type) => type is null ? null : this._provider.GetService(type);

        public void Dispose()
        {
            if (this._provider is IDisposable disposable)
                 disposable.Dispose();
        }
    }
}
