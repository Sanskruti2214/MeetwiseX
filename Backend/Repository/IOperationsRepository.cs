public interface IOperationsRepository
{
    Task<Rooms> AddRoom(RoomsInfo room,int companyId);
    Task<Floors> AddFloor(Floors floor);
    Task<Buildings> AddBuilding(Buildings floor);
    // Task<RoomAvailability> AddRoomAvailability(RoomAvailability roomAvailability);
    Task<Buildings> GetBuildingsById(int companyId,int buildingId);
}