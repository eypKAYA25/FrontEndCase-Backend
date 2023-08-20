using System.Formats.Asn1;
using AutoMapper;
using Domain.Entities;
using Domain.Queries;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.QueryHandler;

public class GetCompanyByIdQueryHandler : IRequestHandler<GetCompanyByIdQuery, Domain.Entities.Company>
{
    private readonly IMapper _mapper;
    private readonly PostgreSqlDbContext _postgreSqlDbContext;

    public GetCompanyByIdQueryHandler(PostgreSqlDbContext postgreSqlDbContext, IMapper mapper)
    {
        _postgreSqlDbContext = postgreSqlDbContext;
        _mapper = mapper;
    }

    public async Task<Company> Handle(GetCompanyByIdQuery request, CancellationToken cancellationToken)
    {
        Infrastructure.Entities.Company company =
            await this._postgreSqlDbContext.Company.FirstOrDefaultAsync(x => x.Id.Equals(request.Id));

        Domain.Entities.Company domainCompany = this._mapper.Map<Domain.Entities.Company>(company);
        
        return domainCompany;
    }
}