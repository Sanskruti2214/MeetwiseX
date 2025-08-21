public class RoomsInfo{
    public required int RoomId { get; set; }
    public required string RoomType { get; set; }
    public required int BuildingId{get;set;}
    public required string BuildingName{get;set;}
    public required int FloorId{get;set;}
    public bool IsAvailable{get;set;}=true;
}