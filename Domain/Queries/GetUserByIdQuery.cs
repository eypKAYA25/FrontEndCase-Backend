using MediatR;

namespace Domain.Queries;

public class GetUserByIdQuery : IRequest<Domain.Entities.User>
{
    public Guid Id { get; set; }
}