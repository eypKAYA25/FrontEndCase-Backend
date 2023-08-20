using MediatR;

namespace Domain.Queries;

public class GetAllCompaniesByIdQuery : IRequest<List<Domain.Entities.Company>>
{
    public Guid UserId { get; set; }
}