using System.Security.Claims;
using Domain.Base.Provider;
using MediatR;

namespace Domain.User.BusinessOperation.ForgotPassword;

public record GetResetPasswordTokenBusinessRequest : AuthorizedRequest, IRequest<string>
{
    public GetResetPasswordTokenBusinessRequest(ClaimsPrincipal claims) : base(claims)
    {
    }
}

public class GetResetPasswordTokenBusinessOperation : IRequestHandler<GetResetPasswordTokenBusinessRequest, string>
{
    private readonly IIdentityProvider _identityManagerProvider;

    public GetResetPasswordTokenBusinessOperation(IIdentityProvider identityManagerProvider)
    {
        _identityManagerProvider = identityManagerProvider;
    }

    public async Task<string> Handle(GetResetPasswordTokenBusinessRequest request, CancellationToken cancellationToken)
    {
        string resetToken = await this._identityManagerProvider.GetResetPasswordToken(Guid.Parse(request.GetClaimValue(Strings.USERID)), cancellationToken);
        return resetToken;
    }
}