using AutoMapper;
using Domain.Entities;
using Domain.Queries;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.QueryHandler;

public class GetAllCompaniesByIdQueryHandler : IRequestHandler<GetAllCompaniesByIdQuery, List<Domain.Entities.Company>>
{
    private readonly IMapper _mapper;
    private readonly PostgreSqlDbContext _postgreSqlDbContext;

    public GetAllCompaniesByIdQueryHandler(IMapper mapper, PostgreSqlDbContext postgreSqlDbContext)
    {
        _mapper = mapper;
        _postgreSqlDbContext = postgreSqlDbContext;
    }

    public async Task<List<Domain.Entities.Company>> Handle(GetAllCompaniesByIdQuery request, CancellationToken cancellationToken)
    {
        List<Infrastructure.Entities.Company> companies = await this._postgreSqlDbContext.Company
            .Where(x => x.UserId.Equals(request.UserId)).ToListAsync();
        List<Domain.Entities.Company> domainCompanies = this._mapper.Map<List<Domain.Entities.Company>>(companies);
        return domainCompanies;
    }
}