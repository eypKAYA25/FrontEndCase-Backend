using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace RestApi.Models.Authentication;

public class AuthenticationRequestModel
{
    [Required]
    [EmailAddress]
    public required string Email { get; set; }
    
    [Required]
    [PasswordPropertyText]
    public required string Password { get; set; }
}