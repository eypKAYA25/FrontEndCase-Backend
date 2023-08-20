using MediatR;

namespace Domain.Queries;

public class AuthenticationQuery : IRequest<string>
{
    public string Email { get; set; }
    
    public string Password { get; set; }
}