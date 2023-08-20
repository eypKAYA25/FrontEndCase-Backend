using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Models;
using Registration;
using RestApi.Middlewares;

namespace RestApi;

public class Registirer : IRegistration
{
    private readonly IConfiguration _configuration;

    public Registirer(IConfiguration configuration)
    {
        this._configuration = configuration;
    }
    public IServiceCollection Register(IServiceCollection services)
    {
        new Infrastructure.Registirer(this._configuration).Register(services);
        services.AddControllers();
        services.AddAutoMapper(typeof(MapperProfile));
        services.AddApiVersioning(options =>
        {
            options.AssumeDefaultVersionWhenUnspecified = true;
            options.DefaultApiVersion = new ApiVersion(1, 0);
            options.ReportApiVersions = true;
        });
        services.AddVersionedApiExplorer(options =>
        {
            options.GroupNameFormat = "'v'VVV";
            options.SubstituteApiVersionInUrl = true;
        });
        // services.AddAuthentication("Bearer").AddJwtBearer();
        services.AddSwaggerGen(options =>
        {
            options.CustomOperationIds(x =>
                $"{x.HttpMethod}: {x.ActionDescriptor.RouteValues["controller"]}/{x.ActionDescriptor.RouteValues["action"]}");
            options.SwaggerDoc("v1", new OpenApiInfo
            {
                Version = "v1",
                Title = "Fon Radar FrontEndCase API",
                Description = "An ASP.NET Core Web API for managing FrontEndCase",
                TermsOfService = new Uri("https://example.com/terms"),
                Contact = new OpenApiContact
                {
                    Name = "Example Contact",
                    Url = new Uri("https://example.com/contact")
                },
                License = new OpenApiLicense
                {
                    Name = "Example License",
                    Url = new Uri("https://example.com/license")
                }
            });

            OpenApiSecurityScheme openApiSecurityScheme = new OpenApiSecurityScheme()
            {
                Reference = new OpenApiReference()
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = JwtBearerDefaults.AuthenticationScheme
                },

                Scheme = JwtBearerDefaults.AuthenticationScheme,
                BearerFormat = "JWT",
                Name = "Authorization",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.Http
            };

            options.AddSecurityDefinition(openApiSecurityScheme.Reference.Id, openApiSecurityScheme);

            options.AddSecurityRequirement(new OpenApiSecurityRequirement()
            {
                {
                    openApiSecurityScheme,
                    new List<string>(0)
                }
            });

            
        });
        services.AddRouting(options =>
        {
            options.LowercaseUrls = true;
            options.LowercaseQueryStrings = true;
        });

        services.AddCors();
        services.AddScoped<DomainExceptionHandlerMiddleware>();
        return services;
    }
}