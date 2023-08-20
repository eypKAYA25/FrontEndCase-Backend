using AutoMapper;
using Domain.Queries;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.QueryHandler;

public class CreateCompanyQueryHandler :IRequestHandler<CreateCompanyQuery, bool>
{
    private readonly IMapper _mapper;
    private readonly PostgreSqlDbContext _postgreSqlDbContext;
    
    public CreateCompanyQueryHandler(IMapper mapper, PostgreSqlDbContext postgreSqlDbContext)
    {
        this._mapper = mapper;
        this._postgreSqlDbContext = postgreSqlDbContext;
    }
    public async Task<bool> Handle(CreateCompanyQuery request, CancellationToken cancellationToken)
    {
        bool isCompanyExists =  await _postgreSqlDbContext.Company
            .AnyAsync(x => x.TaxNumber.Equals(request.TaxNumber), cancellationToken);
        
        return isCompanyExists;
    }
}