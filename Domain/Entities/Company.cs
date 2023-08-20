using Domain.Base;

namespace Domain.Entities;

public class Company : IEntity
{
    public Guid Id { get; set; }
    
    public Guid UserId { get; set; }
    
    public string CompanyName { get; set; }
    
    public ushort Province { get; set; }
    
    public string TaxNumber { get; set; }
    
    public string TaxOffice { get; set; }
    
    public string ContactNumber { get; set; }

    public DateOnly CreateDate { get; set; }


}