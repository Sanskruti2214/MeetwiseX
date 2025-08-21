using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
public class Company
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int companyId{ get; set; }
    public required string CompanyUniqueId { get; set; }
    public required string CompanyName{get;set;}
}