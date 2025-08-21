public class ReserveInfo
{
    public required int FloorId { get; set; }
    public required int BuildingId { get; set; }
    public required string RoomsType{ get; set; }
    public required DateTime StartTime { get; set; }
    public required DateTime EndTime { get; set;}
}