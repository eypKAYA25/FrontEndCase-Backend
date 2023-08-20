using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Domain.Base;

namespace Infrastructure.Entities;

public class Company 
{
    [Key]
    public Guid Id { get; set; }
    
    public Guid UserId { get; set; }
    
    public string CompanyName { get; set; }
    
    public ushort Province { get; set; }
    
    public string TaxNumber { get; set; }
    
    public string TaxOffice { get; set; }

    public string ContactNumber { get; set; }

    public DateOnly CreateDate { get; set; }


}