using Microsoft.IdentityModel.Tokens;
public class BookingService : IBookingService
{
    private readonly IBookingRepository _bookingRepository;
    private readonly IUserRepository _userRepository;
    private readonly IOperationsRepository _operationsRepository;
    public BookingService(IBookingRepository bookingRepository, IUserRepository userRepository, IOperationsRepository operationsRepository)
    {
        _bookingRepository = bookingRepository;
        _userRepository = userRepository;
        _operationsRepository = operationsRepository;
    }
    // Adding new Room
    public async Task<Rooms> RegisterRoom(RoomsInfo room, string companyUniqueId)
    {
        var companyId = await _userRepository.GetCompanyId(companyUniqueId);
        var result = await _operationsRepository.AddRoom(room, companyId);
        Floors floor = new Floors
        {
            FloorId = room.FloorId,
            CompanyId = companyId,
            BuildingId = room.BuildingId
        };
        await _operationsRepository.AddFloor(floor);
        Buildings building = new Buildings
        {
            CompanyId = companyId,
            BuildingId = room.BuildingId,
            BuildingName = room.BuildingName
        };
        await _operationsRepository.AddBuilding(building);
        _bookingRepository.AddRoomToCache(companyId, room.BuildingId, room.FloorId, room.RoomId);
        return result;
    }
    // Registering Room
    public async Task<Rooms?> ReserveRoom(ReserveInfo booking, string CompanyUniqueId)
    {
        var companyId = await _userRepository.GetCompanyId(CompanyUniqueId);
        Rooms ansRoom = await FloorReserveRoom(booking, companyId, booking.FloorId);
        if (ansRoom != null)
        {
            return ansRoom;
        }
        // var floorsForBuildings = await _bookingRepository.GetAllFloorsForBuildings(companyId,booking.BuildingId);
        // foreach (var floorId in floorsForBuildings)
        // {
        //     Rooms room = await FloorReserveRoom(booking, floorId);
        //     if (room != null) return room;
        // }
        return null;
    }

    public async Task<(Rooms, DateTime, DateTime)?> ReserveForScheduleMeet(ScheduleRequest booking, string CompanyUniqueId)
    {
        int duration = booking.Duration;
        var companyId = await _userRepository.GetCompanyId(CompanyUniqueId);
        var roomsAll = await _bookingRepository.GetRoomsForGivenBuildingsFloors(companyId, booking.BuildingId, booking.FloorId);
        var roomTimingList = await _bookingRepository.GetRoomBookingsWithTimings(companyId, booking.BuildingId, booking.FloorId, booking.RoomsType, booking.Date);
        DateTime officeSTime = booking.Date.ToDateTime(booking.OfficeStartTime);
        DateTime officeETime = booking.Date.ToDateTime(booking.OfficeEndTime);
        var result = AlgorithmForSchedulingMeet(roomTimingList, duration, officeSTime, officeETime);
        if (result != null)
        {
            var curr = await _operationsRepository.GetBuildingsById(companyId, booking.BuildingId);
            Rooms ans = new Rooms
            {
                RoomId = result.Value.Item1,
                RoomType = booking.RoomsType,
                BuildingId = booking.BuildingId,
                BuildingName = curr.BuildingName,
                FloorId = booking.FloorId,
                CompanyId = companyId
            };
            return (ans, result.Value.Item2, result.Value.Item3);
        }
        return null;

    }
    public (int, DateTime, DateTime)? AlgorithmForSchedulingMeet(Dictionary<int, List<(DateTime, DateTime)>> roomTiming, int duration, DateTime allowedStart, DateTime allowedEnd)
    {
        TimeSpan requiredDuration = TimeSpan.FromMinutes(duration);
        foreach (var kvp in roomTiming)
        {
            int roomId = kvp.Key;
            var bookings = kvp.Value.ToList();
            if (bookings == null || bookings.Count == 0)
            {
                Console.WriteLine($"Room {roomId}: {allowedStart} to {allowedStart + requiredDuration}");
                return (roomId, allowedStart, allowedStart + requiredDuration);
            }
            DateTime prevEnd = allowedStart;
            foreach (var booking in bookings)
            {
                var bookingStart = booking.Item1 < allowedStart ? allowedStart : booking.Item1;
                var bookingEnd = booking.Item2 > allowedEnd ? allowedEnd : booking.Item2;
                if (bookingStart - prevEnd > requiredDuration)
                {
                    Console.WriteLine($"Room {roomId}: {prevEnd} to {prevEnd + requiredDuration}");
                    return (roomId, prevEnd, prevEnd + requiredDuration);
                }
                if (bookingEnd > prevEnd)
                {
                    prevEnd = bookingEnd;
                }
            }
            if (allowedEnd - prevEnd > requiredDuration)
            {
                Console.WriteLine($"Room {roomId}: {prevEnd} to {prevEnd + requiredDuration}");
                return (roomId, prevEnd, prevEnd + requiredDuration);
            }
        }
        return null;
    }
    public async Task<Rooms?> FloorReserveRoom(ReserveInfo booking, int companyId, int FloorId)
    {
        DateOnly date = DateOnly.FromDateTime(booking.StartTime.Date);
        var result = await _bookingRepository.GetRoomBookingsWithTimings(companyId, booking.BuildingId, FloorId, booking.RoomsType, date);
        var roomId = FindAvailableRoomAsync(result, booking.StartTime, booking.EndTime);
        if (roomId != -1)
        {
            var curr = await _operationsRepository.GetBuildingsById(companyId, booking.BuildingId);
            Rooms ans = new Rooms
            {
                RoomId = roomId,
                RoomType = booking.RoomsType,
                BuildingId = booking.BuildingId,
                BuildingName = curr.BuildingName,
                FloorId = FloorId,
                CompanyId = companyId
            };
            return ans;
        }
        return null;
    }
    // Algorithm For Finding Room
    public int FindAvailableRoomAsync(Dictionary<int, List<(DateTime StartTime, DateTime EndTime)>> cache, DateTime RequestStartTime, DateTime RequestEndTime)
    {
        foreach (var rooms in cache)
        {
            var roomTimings = rooms.Value;
            if (roomTimings == null || roomTimings.Count == 0)
            {
                return rooms.Key;
            }
            bool hasConflict = false;
            foreach (var temp in roomTimings)
            {
                if (temp.StartTime >= RequestStartTime && RequestStartTime < temp.EndTime)
                {
                    hasConflict = true;
                    break;
                }
            }
            if (!hasConflict)
            {
                return rooms.Key;
            }
        }
        return -1;
    }

    // public Task<IEnumerable<Bookings>> RemoveExpiredEntries(List<Bookings> bookings)
    // {
    //     var result = bookings.Where(s => !s.IsExpired).ToList();
    //     return Task.FromResult(result);
    // }
}