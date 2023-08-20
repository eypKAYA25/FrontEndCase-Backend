using System.ComponentModel.DataAnnotations;

namespace RestApi.Models.CreateUser;

public class CreateUserRequestModel
{
    public string Name { get; set; }
    
    public string Surname { get; set; }
   
    public string TcKn { get; set; }
    [Phone]
    public string PhoneNumber { get; set; }
    [EmailAddress]
    public string Email { get; set; }
    
    public string Password { get; set; }
}