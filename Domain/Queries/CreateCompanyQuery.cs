using MediatR;

namespace Domain.Queries;

public class CreateCompanyQuery : IRequest<bool>
{
    public string? TaxNumber { get; set; }
}