using MediatR;

namespace Domain.Queries;

public class GetCompanyByIdQuery : IRequest<Domain.Entities.Company>
{
    public Guid Id { get; set; }
}