using System.ComponentModel.DataAnnotations;

namespace RestApi.Models.UpdateUser;

public class UpdateUserRequestModel
{
    [Required]
    public required string Name { get; set; }
    
    [Required]
    public required string Surname { get; set; }
    
    [Required]
    public required string TcKn { get; set; }
}