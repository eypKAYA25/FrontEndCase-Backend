using AutoMapper;
using Domain.Base.Provider;
using Domain.Queries;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.QueryHandler;

public class CreateUserQueryHandler : IRequestHandler<CreateUserQuery, bool>
{
    private readonly IMapper _mapper;
    private readonly PostgreSqlDbContext _postgreSqlDbContext;
    
    public CreateUserQueryHandler(IMapper mapper, PostgreSqlDbContext postgreSqlDbContext)
    {
        this._mapper = mapper;
        this._postgreSqlDbContext = postgreSqlDbContext;
    }

    public async Task<bool> Handle(CreateUserQuery request, CancellationToken cancellationToken)
    {
        bool isEmailExists =  await _postgreSqlDbContext.User.AnyAsync(x => x.Email.Equals(request.Email), cancellationToken);
        
        return isEmailExists;
    }
}