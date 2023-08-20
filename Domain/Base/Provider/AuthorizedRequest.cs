using System.Security.Claims;

namespace Domain.Base.Provider;

public abstract record AuthorizedRequest
{
    private readonly ClaimsPrincipal _claims;
    
    public ClaimsPrincipal Claims
    {
        get { return this._claims; }
    }

    protected AuthorizedRequest(ClaimsPrincipal claims)
    {
        this._claims = claims;
    }

    public string GetClaimValue(string claimType)
    {
        Claim? claim = this.Claims.FindFirst(claimType);
        if (claim == null)
        {
            throw new UnauthorizedAccessException($"{claim} claim not found in token!");
        }

        return claim.Value;
    }
    
    
}