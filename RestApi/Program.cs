using Infrastructure;
using Microsoft.EntityFrameworkCore;
using RestApi.Middlewares;
using Swashbuckle.AspNetCore.SwaggerUI;



var builder = WebApplication.CreateBuilder(args);



string? environmentName = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

if (environmentName == null)
{
    throw new ApplicationException(environmentName);
}
IConfiguration initialConfiguration = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json")
    .AddJsonFile($"appsettings.{environmentName}.json")
    .Build();

builder.Services.AddSingleton<IConfiguration>(initialConfiguration);
 
// IHostBuilder serviceProviderHost = Host.CreateDefaultBuilder();
// serviceProviderHost.UseEnvironment(environmentName);
// serviceProviderHost.UseContentRoot(AppContext.BaseDirectory);
// serviceProviderHost.ConfigureAppConfiguration
// (
//     (
//         context
//         , configurationBuilder
//     ) => configurationBuilder
//         .AddJsonFile("appsettings.json")
//         .AddJsonFile($"appsettings.{environmentName}.json")
// );

builder.Services.AddDbContext<PostgreSqlDbContext>(options =>
{
    options.UseNpgsql(builder.Configuration
            .GetConnectionString(initialConfiguration.GetValue<string>("ConnectionStrings:Postgresql")), 
        builder => builder.MigrationsAssembly("Infrastructure"));
});

new RestApi.Registirer(initialConfiguration).Register(builder.Services);

WebApplication app = builder.Build();

// Configure the HTTP request pipeline.
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
        options.RoutePrefix = "swagger";
        options.ConfigObject.DocExpansion = DocExpansion.List;
        options.ConfigObject.DefaultModelExpandDepth = 1;
        options.ConfigObject.DefaultModelsExpandDepth = 1;
        options.ConfigObject.DisplayOperationId = true;
    });
}
app.UseAuthentication();

app.UseHttpsRedirection();

app.UseRouting();

app.UseCors(policyBuilder => policyBuilder
    .AllowAnyOrigin()
    .SetIsOriginAllowedToAllowWildcardSubdomains()
    .AllowAnyHeader()
    .AllowAnyMethod()
    .Build()
);

app.UseAuthorization();
app.UseMiddleware<DomainExceptionHandlerMiddleware>();
app.MapControllers();

app.Run();