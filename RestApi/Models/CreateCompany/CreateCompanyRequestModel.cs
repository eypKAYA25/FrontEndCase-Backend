using System.ComponentModel.DataAnnotations;

namespace RestApi.Models.CreateCompany;

public class CreateCompanyRequestModel
{
    [Required]
    public required string CompanyName { get; set; }
    
    [Required]
    public required ushort Province { get; set; }
    
    [Required]
    public required string TaxNumber { get; set; }
    
    [Required]
    public required string TaxOffice { get; set; }
    
    [Required]
    public required string CountOfInvoice { get; set; }
    
    [Required]
    [Phone]
    public required string ContactNumber { get; set; }
}