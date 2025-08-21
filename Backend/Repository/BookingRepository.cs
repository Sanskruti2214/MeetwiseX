using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using System.Collections;
using System.Linq;
public class BookingRepository : IBookingRepository
{
    private readonly AppDbContext _context;
    private readonly IMemoryCache _memoryCache;
    private const string RoomCacheKey = "RoomDictionary";
    public BookingRepository(AppDbContext context, IMemoryCache memorycache)
    {
        _context = context;
        _memoryCache = memorycache;
    }
    // Adding a RoomId to Memory Cache
    public void AddRoomToCache(int companyId, int buildingId, int floorId, int roomId)
    {
        var cache = _memoryCache.Get<Dictionary<int, Dictionary<(int, int), List<int>>>>(RoomCacheKey) ??
           new Dictionary<int, Dictionary<(int, int), List<int>>>();
        if (!cache.ContainsKey(companyId))
        {
            cache[companyId] = new Dictionary<(int, int), List<int>>();
        }
        var innerDictionary = cache[companyId];
        var key = (buildingId, floorId);
        if (!innerDictionary.ContainsKey(key))
        {
            innerDictionary[key] = new List<int>();
        }
        innerDictionary[key].Add(roomId);
        _memoryCache.Set(RoomCacheKey, cache);
    }
    // Obtaining RoomId for given company -> (buildingId and floorId)
    public async Task<List<int>> GetRoomsForGivenBuildingsFloors(int companyId, int buildingId, int floorId)
    {
        if (!_memoryCache.TryGetValue(RoomCacheKey, out Dictionary<int, Dictionary<(int, int), List<int>>> cache))
        {
            // If cache miss, load from DB
            cache = await LoadRoomsFromDb();

            // Set cache with absolute expiration (optional)
            _memoryCache.Set(RoomCacheKey, cache, TimeSpan.FromHours(12));
        }
        if (cache.TryGetValue(companyId, out Dictionary<(int, int), List<int>> buildingIdFloorIdData))
        {
            var key = (buildingId, floorId);
            if (buildingIdFloorIdData.ContainsKey(key))
            {
                return buildingIdFloorIdData[key];
            }
        }
        return new List<int>();
    }

    public async Task<Dictionary<int, Dictionary<(int, int), List<int>>>> LoadRoomsFromDb()
    {
        // Load rooms from DB
        var roomList = await _context.Rooms.ToListAsync();

        var rooms = roomList
            .GroupBy(r => r.CompanyId)
            .ToDictionary(
                g => g.Key,
                g => g
                    .GroupBy(r => (r.BuildingId, r.FloorId))
                    .ToDictionary(
                        gg => gg.Key,
                        gg => gg.Select(r => r.RoomId).ToList()
                    )
            );
        return rooms;
    }
    // Obtaining all roomId bookings timinings for given buildingId and floorId
    public async Task<Dictionary<int, List<(DateTime, DateTime)>>> GetRoomBookingsWithTimings(int companyId, int buildingId, int floorId, string roomType, DateOnly date)
    {
        var key = (buildingId, floorId);
        var roomList = await GetRoomsForGivenBuildingsFloors(companyId, buildingId, floorId);
        if (roomList.Count == 0)
        {
            return new Dictionary<int, List<(DateTime, DateTime)>>();
        }
        // Step 2: Query bookings for only these RoomIds from DB
        var bookings = await _context.Bookings
        .Where(b => b.CompanyId == companyId &&
                roomList.Contains(b.RoomsId) &&
                b.RoomsType == roomType &&
                DateOnly.FromDateTime(b.StartTime.Date) == date)
       .OrderBy(b => b.RoomsId)
       .ThenBy(b => b.StartTime)
       .ToListAsync();
        // Step 3: Build Dictionary<int, List<(StartTime, EndTime)>>
        var roomTimeMapping = bookings
        .GroupBy(b => b.RoomsId)
        .ToDictionary(
            g => g.Key,
            g => g.Select(b => (b.StartTime, b.EndTime)).ToList()
        );
        // Step 4: Ensure even rooms with no bookings exist in final dictionary
        foreach (var roomId in roomList)
        {
            if (!roomTimeMapping.ContainsKey(roomId))
            {
                roomTimeMapping[roomId] = new List<(DateTime, DateTime)>();
            }
        }
        return roomTimeMapping;
    }

    // public async Task<IEnumerable<int>> GetAllFloorsForBuildings(int buildingId)
    // {
    //     var result = await _context.Floors.Where(f => f.BuildingId == buildingId).
    //     Select(f => f.FloorId).
    //     ToListAsync();
    //     return result;
    // }

    public async Task<int> MarkExpiredBookingAsync()
    {
        var currTime = DateTime.UtcNow;
        var expiredBooking = await _context.Bookings.Where(b => !b.IsExpired && b.EndTime < currTime).ToListAsync();
        foreach (var booking in expiredBooking)
        {
            booking.IsExpired = true;
        }
        await _context.SaveChangesAsync();
        return expiredBooking.Count;
    }
}