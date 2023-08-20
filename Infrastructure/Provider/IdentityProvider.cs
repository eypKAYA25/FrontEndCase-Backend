using System.Text.Json;
using System.Web;
using AutoMapper;
using Domain.Base.Provider;
using Infrastructure.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;

namespace Infrastructure.Provider;

public class IdentityProvider : IIdentityProvider
{
    private readonly PostgreSqlDbContext _postgreSqlDbContext;
    private readonly UserManager<Infrastructure.Entities.Identity.AspUser> _userManager;
    private readonly IMapper _mapper;
    private readonly IMediator _mediator;

    public IdentityProvider(
        UserManager<Infrastructure.Entities.Identity.AspUser> userManager
        , IMapper mapper
        , IMediator mediator, PostgreSqlDbContext postgreSqlDbContext)
    {
        this._userManager = userManager;
        this._mapper = mapper;
        this._mediator = mediator;
        this._postgreSqlDbContext = postgreSqlDbContext;
    }

    public async Task CreateAspUser(Domain.Entities.AspUser user, CancellationToken cancellationToken)
    {
        try
        {
            Infrastructure.Entities.Identity.AspUser dbUser = this._mapper.Map<Infrastructure.Entities.Identity.AspUser>(user);
            IdentityResult createUserResult = await this._userManager.CreateAsync(dbUser, user.Password);
            if (!createUserResult.Succeeded)
            {
                throw new Exception(JsonSerializer.Serialize(createUserResult.Errors));
            }
        }
        catch (Exception exception)
        {
            Console.WriteLine(exception);
            throw;
        }
    }
    public async Task<string> GetResetPasswordToken(Guid userId, CancellationToken cancellationToken)
    {
        Infrastructure.Entities.Identity.AspUser? user = await this._userManager.FindByIdAsync(userId.ToString());

        string resetPasswordToken = await this._userManager.GeneratePasswordResetTokenAsync(user);
        string encodedResetPasswordToken = HttpUtility.UrlEncode(resetPasswordToken);
        return encodedResetPasswordToken;
    }
    
    public async Task ResetPassword(string userId, string resetPasswordToken, string newPassword)
    {
        Infrastructure.Entities.Identity.AspUser? user = await this._userManager.FindByIdAsync(userId.ToString());
       

        string decodedResetPasswordToken = HttpUtility.UrlDecode(resetPasswordToken);
        bool isVerifiedResetPasswordToken = await this.VerifyPasswordResetTokenAsync(user, decodedResetPasswordToken);
        if (!isVerifiedResetPasswordToken)
        {
            throw new SecurityTokenValidationException("The token isn't valid!");
        }

        string? passwordViolation = await this.VerifyPasswordAsync(user, newPassword);
        
        await this._userManager.ResetPasswordAsync(user, decodedResetPasswordToken, newPassword);
        await this._userManager.UpdateAsync(user);
    }
    private async Task<bool> VerifyPasswordResetTokenAsync(Infrastructure.Entities.Identity.AspUser user, string resetPasswordToken)
    {
        bool isTokenValid = await _userManager.VerifyUserTokenAsync(user
            , TokenOptions.DefaultProvider
            , UserManager<User>.ResetPasswordTokenPurpose
            , resetPasswordToken
        );
        
        return isTokenValid;
    }
    
    private async Task<string?> VerifyPasswordAsync(Infrastructure.Entities.Identity.AspUser user, string password)
    {
        foreach (IPasswordValidator<Infrastructure.Entities.Identity.AspUser> validator in _userManager.PasswordValidators)
        {
            IdentityResult validationResult = await validator.ValidateAsync(_userManager, user, password);
            if (!validationResult.Succeeded)
            {
                return validationResult.Errors.FirstOrDefault()!.Description;
            }
        }

        return null;
    }
}