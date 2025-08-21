public interface IBookingService
{
    Task<Rooms> RegisterRoom(RoomsInfo room,string companyUniqueId);
    Task<Rooms?> ReserveRoom(ReserveInfo booking,string CompanyUniqueId);
    Task<(Rooms,DateTime,DateTime)?> ReserveForScheduleMeet(ScheduleRequest booking,string CompanyUniqueId);
    (int, DateTime, DateTime)? AlgorithmForSchedulingMeet(Dictionary<int, List<(DateTime, DateTime)>> roomTiming, int duration, DateTime allowedStart, DateTime allowedEnd);
    Task<Rooms?> FloorReserveRoom(ReserveInfo booking,int companyId, int FloorId);
    int FindAvailableRoomAsync(Dictionary<int, List<(DateTime StartTime, DateTime EndTime)>> cache, DateTime RequestStartTime, DateTime RequestEndTime);
    
}