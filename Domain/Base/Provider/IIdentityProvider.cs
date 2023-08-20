namespace Domain.Base.Provider;

public interface IIdentityProvider
{
    Task CreateAspUser(Domain.Entities.AspUser user, CancellationToken cancellationToken);
    
    Task<string> GetResetPasswordToken(Guid userId, CancellationToken cancellationToken);
    Task ResetPassword(string userId, string resetPasswordToken, string newPassword);
}