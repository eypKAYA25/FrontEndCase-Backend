using System.Security.Claims;
using AutoMapper;
using Domain.Base.Provider;
using Domain.Company.BusinessOperation.Exceptions;
using Domain.Entities;
using Domain.Queries;
using MediatR;

namespace Domain.Company.BusinessOperation.CreateCompany;


public record CreateCompanyBusinessRequest : AuthorizedRequest, IRequest<Unit>
{
    public string CompanyName { get; set; }
    
    public ushort Province { get; set; }
    
    public string TaxNumber { get; set; }
    
    public string TaxOffice { get; set; }
    
    public string CountOfInvoice { get; set; }
    
    public string ContactNumber { get; set; }
    public CreateCompanyBusinessRequest(ClaimsPrincipal claims) : base(claims)
    {
    }
}

public class CreateCompanyBusinessOperation : IRequestHandler<CreateCompanyBusinessRequest, Unit>
{
    private readonly IMapper _mapper;
    private readonly IMediator _mediator;
    private readonly IPostgresSqlDbProvider _postgresSqlDbProvider;

    public CreateCompanyBusinessOperation(IMapper mapper, IMediator mediator, IPostgresSqlDbProvider postgresSqlDbProvider)
    {
        _mapper = mapper;
        _mediator = mediator;
        _postgresSqlDbProvider = postgresSqlDbProvider;
    }

    public async Task<Unit> Handle(CreateCompanyBusinessRequest request, CancellationToken cancellationToken)
    {
        CreateCompanyQuery queryRequest = this._mapper.Map<CreateCompanyQuery>(request);
        bool isCompanyExists= await this._mediator.Send(queryRequest, cancellationToken);
        if (isCompanyExists)
            throw new CompanyWithSameTaxNumberExistException("This TaxNumber has been used before.");
        var asd =request.GetClaimValue(Strings.USERID);
        Domain.Entities.Company company =new Entities.Company()
        {
            Id = Guid.NewGuid(),
            UserId = Guid.Parse(request.GetClaimValue(Strings.USERID)),
            Province = request.Province,
            CompanyName = request.CompanyName,
            TaxOffice = request.TaxOffice,
            TaxNumber = request.TaxNumber,
            ContactNumber = request.ContactNumber,
            CreateDate = DateOnly.FromDateTime(DateTime.Now),
        };
        await _postgresSqlDbProvider.PersistAsync(State.Added, company);
        return Unit.Value;
    }
}
//.GetClaimValue(Strings.BUYERID