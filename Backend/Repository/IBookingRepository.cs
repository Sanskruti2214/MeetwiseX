public interface IBookingRepository{
    void AddRoomToCache(int companyId,int buildingId, int floorId,int roomId);
    Task<List<int>> GetRoomsForGivenBuildingsFloors(int companyId,int buildingId,int floorId);
    Task<Dictionary<int,Dictionary<(int, int), List<int>>>> LoadRoomsFromDb();
    // Task<IEnumerable<Bookings>> RemoveExpiredEntries(List<Bookings> bookings);
    Task<Dictionary<int, List<(DateTime, DateTime)>>> GetRoomBookingsWithTimings(int companyId,int buildingId, int floorId, string roomType, DateOnly date);
    //Task<IEnumerable<int>> GetAllFloorsForBuildings(int buildingId);
    Task<int> MarkExpiredBookingAsync();

}