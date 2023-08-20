using AutoMapper;
using Domain.Queries;
using MediatR;

namespace Domain.Authentication.BusinessOperation;

public class AuthenticationBusinessRequest : IRequest<AuthenticationBusinessResponse>
{
    public string Email { get; set; }
    
    public string Password { get; set; }
}

public class AuthenticationBusinessResponse
{
    public string Token { get; set; }
}

public class AuthenticationBusinessOperation : IRequestHandler<AuthenticationBusinessRequest, AuthenticationBusinessResponse>
{
    private readonly IMapper _mapper;
    private readonly IMediator _mediator;

    public AuthenticationBusinessOperation(IMapper mapper, IMediator mediator)
    {
        _mapper = mapper;
        _mediator = mediator;
    }

    public async Task<AuthenticationBusinessResponse> Handle(AuthenticationBusinessRequest request, CancellationToken cancellationToken)
    {
        AuthenticationQuery queryRequest = this._mapper.Map<AuthenticationQuery>(request);
        string response = await this._mediator.Send(queryRequest, cancellationToken);
        
        return new AuthenticationBusinessResponse()
        {
            Token = response
        };
    }
}