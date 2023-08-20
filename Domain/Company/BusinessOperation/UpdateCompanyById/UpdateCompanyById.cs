using System.Security.Claims;
using AutoMapper;
using Domain.Base.Provider;
using Domain.Company.BusinessOperation.Exceptions;
using Domain.Entities;
using Domain.Queries;
using MediatR;

namespace Domain.Company.BusinessOperation.UpdateCompanyById;

public record UpdateCompanyByIdBusinessRequest : AuthorizedRequest, IRequest<Unit>
{
    public Guid CompanyId { get; set; }
    
    public string CompanyName { get; set; }
    
    public ushort Province { get; set; }
    
    public string TaxNumber { get; set; }
    
    public string TaxOffice { get; set; }
    
    public string ContactNumber { get; set; }
    public UpdateCompanyByIdBusinessRequest(ClaimsPrincipal claims) : base(claims)
    {
    }
}

public record UpdateCompanyByIdBusinessOperation : IRequestHandler<UpdateCompanyByIdBusinessRequest, Unit>
{
    private readonly IMapper _mapper;
    private readonly IMediator _mediator;
    private readonly IPostgresSqlDbProvider _postgresSqlDbProvider;

    public UpdateCompanyByIdBusinessOperation(IMapper mapper, IMediator mediator, IPostgresSqlDbProvider postgresSqlDbProvider)
    {
        _mapper = mapper;
        _mediator = mediator;
        _postgresSqlDbProvider = postgresSqlDbProvider;
    }

    public async Task<Unit> Handle(UpdateCompanyByIdBusinessRequest request, CancellationToken cancellationToken)
    {
        IsExistsCompanyByIdQuery queryRequest = new()
        {
            CompanyId = request.CompanyId
        };
        bool isExistsCompany = await this._mediator.Send(queryRequest, cancellationToken);
        if (isExistsCompany)
            throw new CompanyNotFoundException("No such company has been found.");
        
        GetCompanyByIdQuery getCompany = this._mapper.Map<GetCompanyByIdQuery>(request);
        
        Domain.Entities.Company company = await this._mediator.Send(getCompany, cancellationToken);
        company.CompanyName = request.CompanyName;
        company.TaxNumber = request.TaxNumber;
        company.Province = request.Province;
        company.TaxOffice = request.TaxOffice;
        company.ContactNumber = request.ContactNumber;

        await this._postgresSqlDbProvider.PersistAsync(State.Modified, company);
        return Unit.Value;
    }
}