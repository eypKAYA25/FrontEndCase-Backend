using AutoMapper;
using Domain.Queries;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.QueryHandler;

public class IsExistsCompanyByIdQueryHandler : IRequestHandler<IsExistsCompanyByIdQuery, bool>
{
    private readonly IMapper _mapper;
    private readonly PostgreSqlDbContext _postgreSqlDbContext;

    public IsExistsCompanyByIdQueryHandler(IMapper mapper, PostgreSqlDbContext postgreSqlDbContext)
    {
        _mapper = mapper;
        _postgreSqlDbContext = postgreSqlDbContext;
    }

    public async Task<bool> Handle(IsExistsCompanyByIdQuery request, CancellationToken cancellationToken)
    {
        bool isExistsCompany = await this._postgreSqlDbContext.Company.AnyAsync(x => x.Id.Equals(request.CompanyId));
        return isExistsCompany;
    }
}