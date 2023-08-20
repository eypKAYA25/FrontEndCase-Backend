using Microsoft.Extensions.DependencyInjection;

namespace Registration;

/// <summary>
/// This interface provides you the <see cref="Register"/> method,
/// it takes <see cref="IServiceCollection"/> <paramref name="services"/> as parameter
/// so you can register your services in this method in implemented concrete class.
/// </summary>
public interface IRegistration
{
    /// <summary>
    /// Register your services
    /// </summary>
    /// <param name="services"></param>
    IServiceCollection Register(IServiceCollection services);
}