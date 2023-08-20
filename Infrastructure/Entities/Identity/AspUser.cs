using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Entities.Identity;

public class AspUser : IdentityUser<Guid>
{
    public Guid UserId { get; set; }
}