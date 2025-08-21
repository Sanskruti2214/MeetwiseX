using Microsoft.EntityFrameworkCore;
public class OperationsRepository:IOperationsRepository
{
    private readonly AppDbContext _context;
    public OperationsRepository(AppDbContext context)
    {
        _context = context;
    }
    public async Task<Rooms> AddRoom(RoomsInfo room,int companyId)
    {
        var exist = await _context.Rooms.AnyAsync(r =>
            r.CompanyId==companyId &&
            r.RoomId == room.RoomId &&
            r.RoomType == room.RoomType &&
            r.FloorId == room.FloorId &&
            r.BuildingId == room.BuildingId
        );
        var roomEntry = new Rooms
        {
            CompanyId = companyId,
            RoomId = room.RoomId,
            RoomType = room.RoomType,
            FloorId = room.FloorId,
            BuildingName = room.BuildingName,
            BuildingId = room.BuildingId
        };
        if (exist)
        {
            return roomEntry;
        }
        _context.Rooms.Add(roomEntry);
        await _context.SaveChangesAsync();
        return roomEntry;
    }

    public async Task<Floors> AddFloor(Floors floor)
    {   
        var exist = await _context.Floors.AnyAsync(f =>
            f.CompanyId==floor.CompanyId &&
            f.FloorId == floor.FloorId &&
            f.BuildingId == floor.BuildingId
        );
        if (exist)
        {
            return floor;
        }
        _context.Floors.Add(floor);
        await _context.SaveChangesAsync();
        return floor;
    }

    public async Task<Buildings> AddBuilding(Buildings building)
    {   
        var exist = await _context.Buildings.AnyAsync(b =>
            b.CompanyId == building.CompanyId &&
            b.BuildingId == building.BuildingId &&
            b.BuildingName==building.BuildingName
        );
        if (exist)
        {
            return building;
        }
        _context.Buildings.Add(building);
        await _context.SaveChangesAsync();
        return building;
    }

    // public async Task<RoomAvailability> AddRoomAvailability(RoomAvailability roomAvailability)
    // {
    //     _context.RoomAvailability.Add(roomAvailability);
    //     await _context.SaveChangesAsync();
    //     return roomAvailability;
    // }

    public async Task<Buildings> GetBuildingsById(int companyId,int buildingId){
        return await _context.Buildings.FirstOrDefaultAsync(b => b.CompanyId==companyId && b.BuildingId == buildingId);
    }
    
}