using Domain.Base;

namespace Domain.Entities;

public class User : IEntity
{
    public Guid Id { get; set; }
    
    public string Name { get; set; }
    
    public string Surname { get; set; }
    
    public string TcKn { get; set; }
    
    public string PhoneNumber { get; set; }
    
    public string Email { get; set; }
    
    public DateOnly CreateDate { get; set; }
}