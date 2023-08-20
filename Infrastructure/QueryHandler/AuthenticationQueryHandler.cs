using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Domain.Queries;
using Infrastructure.Entities.Identity;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;

namespace Infrastructure.QueryHandler;

public class AuthenticationQueryHandler : IRequestHandler<AuthenticationQuery, string>
{
    private readonly UserManager<AspUser> _userManager;
    private readonly SignInManager<AspUser> _signInManager;
    private readonly JwtSecurityTokenHandler _jwtSecurityTokenHandler;
    private readonly SigningCredentials _signingCredentials;
    private readonly PostgreSqlDbContext _postgreSqlDbContext;

    public AuthenticationQueryHandler(UserManager<AspUser> userManager, SignInManager<AspUser> signInManager, JwtSecurityTokenHandler jwtSecurityTokenHandler, SigningCredentials signingCredentials, PostgreSqlDbContext postgreSqlDbContext)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _jwtSecurityTokenHandler = jwtSecurityTokenHandler;
        _signingCredentials = signingCredentials;
        _postgreSqlDbContext = postgreSqlDbContext;
    }

    public async Task<string> Handle(AuthenticationQuery request, CancellationToken cancellationToken)
    {
        AspUser? user = await this._userManager.FindByEmailAsync(request.Email);
        if (user == null)
        {
            //throw new UserNotFoundException("Username is not correct!");
        }
        SignInResult signInResult = await this._signInManager.PasswordSignInAsync(user, request.Password, true, true);
        if (signInResult.Succeeded)
        {
            Claim[] claims = new Claim[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()), 
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Hash, user.SecurityStamp),
                new Claim(Strings.USER_ID, user.Id.ToString()),
            };
            DateTime currentDateTime = DateTime.Now;
            SecurityTokenDescriptor securityTokenDescriptor = new SecurityTokenDescriptor()
            {
                Subject = new ClaimsIdentity(claims),
                IssuedAt = currentDateTime, 
                NotBefore = currentDateTime.AddMilliseconds(-5),
                Expires = DateTime.Now.AddDays(1),
                SigningCredentials = this._signingCredentials,
                Audience = Strings.ISSUER,
                Issuer = Strings.ISSUER
            };
            JwtSecurityToken jwtToken = this._jwtSecurityTokenHandler.CreateJwtSecurityToken(securityTokenDescriptor);
            string token = this._jwtSecurityTokenHandler.WriteToken(jwtToken);

            return token;
        }
        throw new ("Username or password is not correct!");
    }
}