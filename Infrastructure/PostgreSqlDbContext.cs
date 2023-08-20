using Infrastructure.Entities;
using Infrastructure.Entities.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Infrastructure;

public class PostgreSqlDbContext : IdentityDbContext<AspUser, Role, Guid>
{
    private readonly IConfiguration _configuration;
    
    public DbSet<User> User { get; set; }
    
    public DbSet<Company> Company { get; set; }
    
    public PostgreSqlDbContext()
    {
    }

    public PostgreSqlDbContext(DbContextOptions options, IConfiguration configuration) : base(options)
    {
        this._configuration = configuration;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseNpgsql(_configuration
            .GetConnectionString("Host=localhost;Port=5432;Database=FrontEndCase;Username=postgres;Password=4231;Command Timeout=60;SSL Mode=Disable"));
        optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=FrontEndCase;Username=postgres;Password=4231;Command Timeout=60;SSL Mode=Disable", builder =>
        {
            builder.CommandTimeout(60);
            builder.SetPostgresVersion(new Version(15, 0));
        });
    }
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.ApplyConfigurationsFromAssembly(typeof(PostgreSqlDbContext).Assembly);
    }
    
}
//dotnet ef migrations add  --project Infrastructure.csproj -s YourWebProjectName -c PostgreSqlDbContext --verbose 
//dotnet ef --startup-project ./RestApi.csproj migrations add  --context PostgreSqlDbContext --output-dir Migrations --project ../Infrastructure/Infrastructure.csproj
