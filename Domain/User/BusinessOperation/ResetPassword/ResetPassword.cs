using System.Security.Claims;
using Domain.Base.Provider;
using MediatR;

namespace Domain.User.BusinessOperation.ResetPassword;

public record ResetPasswordBusinessRequest : AuthorizedRequest, IRequest<Unit>
{
    public string ResetPasswordToken { get; set; }
   
    public string NewPassword { get; set; }
    
    public string NewPasswordConfirmation { get; set; }
    
    public ResetPasswordBusinessRequest(ClaimsPrincipal claims) : base(claims)
    {
    }
}

public class ResetPasswordBusinessOperation : IRequestHandler<ResetPasswordBusinessRequest, Unit>
{
    private readonly IIdentityProvider _identityProvider;

    public ResetPasswordBusinessOperation(IIdentityProvider identityProvider)
    {
        _identityProvider = identityProvider;
    }

    public async Task<Unit> Handle(ResetPasswordBusinessRequest request, CancellationToken cancellationToken)
    {
        // if (!request.NewPassword.Equals(request.NewPasswordConfirmation))
        // {
        //     throw new PasswordDoesNotMatchException();
        // }
        
        await this._identityProvider.ResetPassword(
            request.GetClaimValue(Strings.USERID)
            , request.ResetPasswordToken
            , request.NewPassword
        );

        return Unit.Value;
    }
}