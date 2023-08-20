using System.Security.Claims;
using AutoMapper;
using Domain.Base.Provider;
using Domain.Entities;
using Domain.Queries;
using MediatR;

namespace Domain.User.BusinessOperation.UpdateUser;

public record UpdateUserBusinessRequest : AuthorizedRequest, IRequest<Unit>
{
    public string Name { get; set; }
    
    public string Surname { get; set; }
    
    public string TcKn { get; set; }
    
    public UpdateUserBusinessRequest(ClaimsPrincipal claims) : base(claims)
    {
    }
}

public record UpdateUserBusinessOperation : IRequestHandler<UpdateUserBusinessRequest, Unit>
{
    private readonly IMapper _mapper;
    private readonly IMediator _mediator;
    private readonly IPostgresSqlDbProvider _postgresSqlDbProvider;

    public UpdateUserBusinessOperation(IMapper mapper, IMediator mediator, IPostgresSqlDbProvider postgresSqlDbProvider)
    {
        _mapper = mapper;
        _mediator = mediator;
        _postgresSqlDbProvider = postgresSqlDbProvider;
    }

    public async Task<Unit> Handle(UpdateUserBusinessRequest request, CancellationToken cancellationToken)
    {
        GetUserByIdQuery queryRequest = this._mapper.Map<GetUserByIdQuery>(request);
       
        Domain.Entities.User user = await this._mediator.Send(queryRequest, cancellationToken);
        user.Name = request.Name;
        user.TcKn = request.TcKn;
        user.Surname = request.Surname;

        await this._postgresSqlDbProvider.PersistAsync(State.Modified, user);
        return Unit.Value;
    }
}
