using System.Security.Claims;
using AutoMapper;
using Domain.Base.Provider;
using Domain.Entities;
using Domain.Queries;
using Domain.User.BusinessOperation.Exceptions;
using MediatR;

namespace Domain.User.BusinessOperation.CreateUser;

public record CreateUserBusinessRequest : AuthorizedRequest, IRequest<Guid>
{
    public string Name { get; set; }
    
    public string Surname { get; set; }
    
    public string TcKn { get; set; }
    
    public string PhoneNumber { get; set; }
    
    public string Email { get; set; }
    public string Password { get; set; }

    public CreateUserBusinessRequest(ClaimsPrincipal claims) : base(claims)
    {
    }
}
public class CreateUserBusinessResponse
{
    public Guid UserId { get; set; }
}
public class CreateUserBusinessOperation : IRequestHandler<CreateUserBusinessRequest ,Guid>
{
    private readonly IMapper _mapper;
    private readonly IMediator _mediator;
    private readonly IPostgresSqlDbProvider _postgresSqlDbProvider;
    private readonly IIdentityProvider _identityProvider;

    public CreateUserBusinessOperation(IMapper mapper, IMediator mediator, IPostgresSqlDbProvider postgresSqlDbProvider,
        IIdentityProvider identityProvider)
    {
        _mapper = mapper;
        _mediator = mediator;
        _postgresSqlDbProvider = postgresSqlDbProvider;
        _identityProvider = identityProvider;
    }

    public async Task<Guid> Handle(CreateUserBusinessRequest request, CancellationToken cancellationToken)
    {
        CreateUserQuery queryRequest = this._mapper.Map<CreateUserQuery>(request);
        bool isEmailExists = await this._mediator.Send(queryRequest, cancellationToken);
        if (isEmailExists)
            throw new UserWithSameEmailExistException("This Email address has been used before.");

        Domain.Entities.User user =new Entities.User()
        {
            Id = Guid.NewGuid(),
            Email = request.Email,
            Name = request.Name,
            Surname = request.Surname,
            PhoneNumber = request.PhoneNumber,
            TcKn = request.TcKn,
            CreateDate = DateOnly.FromDateTime(DateTime.Now)
        };

       
        Entities.AspUser aspUser = new Entities.AspUser()
        {
            Id = user.Id,
            UserName = user.Email,
            Password = request.Password,
            PhoneNumber = user.PhoneNumber,
            Email = user.Email,
        };
        await this._postgresSqlDbProvider.PersistAsync(State.Added, user);
        await this._identityProvider.CreateAspUser(aspUser, cancellationToken);
        
        return user.Id;
    }
}