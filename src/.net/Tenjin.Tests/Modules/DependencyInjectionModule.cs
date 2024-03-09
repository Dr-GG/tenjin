using Microsoft.Extensions.DependencyInjection;
using Tenjin.Extensions;
using Tenjin.Interfaces.DependencyInjection;

namespace Tenjin.Tests.Modules;

public class DependencyInjectionModule : IDependencyInjectionModule
{
    public void Register(IServiceCollection services)
    {
        services.RegisterMappers(typeof(DependencyInjectionModule).Assembly);
    }
}
