using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
public class Users{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id{get;set;}
    public required string Email{get;set;}
    public required string Username{get;set;}
    public required string Password{get;set;}
    public required string CompanyName{ get; set; }
    public required string CompanyUniqueId { get; set; }
    public string Role { get; set; } = "user";

}