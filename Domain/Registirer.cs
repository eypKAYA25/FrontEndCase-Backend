using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Registration;

namespace Domain;

public class Registirer : IRegistration
{
    public IServiceCollection Register(IServiceCollection services)
    {
       
        services.AddAutoMapper(typeof(MapperProfile));
        services.AddMediatR(AppDomain.CurrentDomain.GetAssemblies());
        
        return services;
    }
}