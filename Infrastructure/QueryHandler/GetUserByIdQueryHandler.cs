using AutoMapper;
using Domain.Entities;
using Domain.Queries;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.QueryHandler;

public class GetUserByIdQueryHandler : IRequestHandler<GetUserByIdQuery, Domain.Entities.User>
{
    private readonly IMapper _mapper;
    private readonly PostgreSqlDbContext _postgreSqlDbContext;

    public GetUserByIdQueryHandler(IMapper mapper, PostgreSqlDbContext postgreSqlDbContext)
    {
        _mapper = mapper;
        _postgreSqlDbContext = postgreSqlDbContext;
    }

    public async Task<User> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
    {
        Infrastructure.Entities.User user =
            await this._postgreSqlDbContext.User.FirstOrDefaultAsync(x => x.Id.Equals(request.Id), cancellationToken);

        Domain.Entities.User domainUser = this._mapper.Map<Domain.Entities.User>(user);
        return domainUser;
    }
}