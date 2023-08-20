using MediatR;

namespace Domain.Queries;

public class CreateUserQuery :IRequest<bool>
{
    public string Email { get; set; }
}