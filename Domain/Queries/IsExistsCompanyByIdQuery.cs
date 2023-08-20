using MediatR;

namespace Domain.Queries;

public class IsExistsCompanyByIdQuery : IRequest<bool>
{
    public Guid CompanyId { get; set; }
}