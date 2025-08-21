public class ScheduleRequest
{
    public required int FloorId { get; set; }
    public required int BuildingId { get; set; }
    public required string RoomsType{ get; set; }
    public required DateOnly Date{ get; set; }
    public required int Duration{ get; set; }
    public required TimeOnly OfficeStartTime { get; set; }
    public required TimeOnly OfficeEndTime { get; set;}
}