using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
public class Bookings{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int BookingId{get;set;}
    public required int CompanyId{ get; set; }
    public required int UserId { get; set; }
    public required int FloorId{get;set;}
    public required int BuildingId{get;set;}
    public required int RoomsId{get;set;}
    public required string RoomsType{ get; set; }
    public required DateTime StartTime { get; set; }
    public required DateTime EndTime { get; set;}
    public required bool IsExpired {get;set;}=false;

}