using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
public class Buildings{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    public required int CompanyId{get;set;}
    public required int BuildingId { get; set; }
    public required string BuildingName{get;set;}
}