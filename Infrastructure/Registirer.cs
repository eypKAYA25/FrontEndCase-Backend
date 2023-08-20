using System.IdentityModel.Tokens.Jwt;
using Domain.Base.Provider;
using Infrastructure.Entities.Identity;
using Infrastructure.Provider;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.ValueGeneration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Registration;

namespace Infrastructure;

public class Registirer : IRegistration
{
    private readonly IConfiguration _configuration;

    public Registirer(IConfiguration configuration)
    {
        this._configuration = configuration;
    }
    public IServiceCollection Register(IServiceCollection services)
    {
        services.AddScoped<IIdentityProvider, IdentityProvider>();
        services.AddSingleton<PasswordHashProvider>();
        services.AddTransient<SequentialGuidValueGenerator>();
        
        
        services.AddDefaultIdentity<AspUser>(options =>
            {
                // options.User.RequireUniqueEmail = true;
                // options.SignIn.RequireConfirmedPhoneNumber = false;
                // options.SignIn.RequireConfirmedEmail = false;
                // options.SignIn.RequireConfirmedAccount = false;
                // // if user will activating later, you should true here for passwordless user creation
                // options.Lockout.AllowedForNewUsers = false;  
                // options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
                // options.Lockout.MaxFailedAccessAttempts = 3;
                
                options.User.RequireUniqueEmail = true;
                options.SignIn.RequireConfirmedPhoneNumber = false;
                options.SignIn.RequireConfirmedEmail = false;
                options.SignIn.RequireConfirmedAccount = false;
                options.Lockout.AllowedForNewUsers = false;
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
                options.Lockout.MaxFailedAccessAttempts = 3;
                
                options.Password.RequireDigit = true;
                options.Password.RequiredLength = 8;
                options.Password.RequireLowercase = true;
                options.Password.RequireUppercase = true;
                options.Password.RequireNonAlphanumeric = true;
            })
            .AddRoles<Infrastructure.Entities.Identity.Role>()
            .AddEntityFrameworkStores<PostgreSqlDbContext>()
            .AddDefaultTokenProviders();
        
        services.AddSingleton<PasswordHashProvider>();
        services.AddSingleton<JwtSecurityTokenHandler>();
        services.AddSingleton<SymmetricSecurityKey>(provider =>
        {
            PasswordHashProvider passwordHashProvider = provider.GetRequiredService<PasswordHashProvider>();
            SymmetricSecurityKey symmetricSecurityKey = new SymmetricSecurityKey(PasswordHashProvider.Key);
            return symmetricSecurityKey;
        });
        
        services.AddSingleton<SigningCredentials>((provider) =>
        {
            SymmetricSecurityKey symmetricSecurityKey = provider.GetRequiredService<SymmetricSecurityKey>();
            SigningCredentials signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256Signature);
            return signingCredentials;
        });
        
        services.AddAuthentication(options =>
            {
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultSignInScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultSignOutScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer((JwtBearerOptions options) =>
            {
                SymmetricSecurityKey securityKey = new SymmetricSecurityKey(PasswordHashProvider.Key);
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = Strings.ISSUER,
                    ValidAudience = Strings.ISSUER,
                    IssuerSigningKey = securityKey,
                    TokenDecryptionKey = securityKey
                };
            });
        
        services.AddAutoMapper(typeof(MapperProfile));
        services.AddScoped<IPostgresSqlDbProvider, PostgresSqlDbProvider>();
        
        new Domain.Registirer().Register(services);
        return services;
    }
}