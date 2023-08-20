using System.Security.Claims;
using AutoMapper;
using Domain.Base.Provider;
using Domain.Queries;
using MediatR;

namespace Domain.Company.BusinessOperation.GetAllCompaniesById;

public record GetAllCompaniesByIdBusinessRequest : AuthorizedRequest, IRequest<GetAllCompaniesByIdBusinessResponse>
{
    public GetAllCompaniesByIdBusinessRequest(ClaimsPrincipal claims) : base(claims)
    {
    }
}

public class GetAllCompaniesByIdBusinessResponse 
{
    public List<Entities.Company> Companies { get; set; }
}

public class GetAllCompaniesByIdBusinessOperation : IRequestHandler<GetAllCompaniesByIdBusinessRequest, GetAllCompaniesByIdBusinessResponse>
{
    private readonly IMapper _mapper;
    private readonly IMediator _mediator;

    public GetAllCompaniesByIdBusinessOperation(IMapper mapper, IMediator mediator)
    {
        _mapper = mapper;
        _mediator = mediator;
    }

    public async Task<GetAllCompaniesByIdBusinessResponse> Handle(GetAllCompaniesByIdBusinessRequest request, CancellationToken cancellationToken)
    {
        GetAllCompaniesByIdQuery queryRequest = this._mapper.Map<GetAllCompaniesByIdQuery>(request);
        List<Domain.Entities.Company> companies = await this._mediator.Send(queryRequest, cancellationToken);
        GetAllCompaniesByIdBusinessResponse response = this._mapper.Map<GetAllCompaniesByIdBusinessResponse>(companies);
        return response;
    }
}