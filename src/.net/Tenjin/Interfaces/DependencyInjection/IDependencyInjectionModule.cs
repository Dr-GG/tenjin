using Microsoft.Extensions.DependencyInjection;

namespace Tenjin.Interfaces.DependencyInjection;

/// <summary>
/// The interface that acts as a module for dependency injection.
/// </summary>
public interface IDependencyInjectionModule
{
    /// <summary>
    /// Registers all dependencies of the module.
    /// </summary>
    public void Register(IServiceCollection services);
}
