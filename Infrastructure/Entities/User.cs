using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Infrastructure.Entities;

public class User
{
    [Key]
    public Guid Id { get; set; } 
    
    public required string Name { get; set; }
    
    public required string Surname { get; set; }
    
    public required string TcKn { get; set; }
    
    public required string PhoneNumber { get; set; }
    
    public required string Email { get; set; }
    
    public DateOnly CreateDate { get; set; }
    
    [ForeignKey("UserId")]
    public ICollection<Company> Companies { get; set; }
}