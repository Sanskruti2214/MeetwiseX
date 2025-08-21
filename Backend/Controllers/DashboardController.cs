using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
[Authorize]
[Route("api/[controller]")]
[ApiController]
public class DashboardController : ControllerBase
{
    private readonly IUserService _userService;
    private readonly IBookingService _bookingService;
    public DashboardController(IUserService userService, IBookingService bookingService)
    {
        _userService = userService;
        _bookingService = bookingService;
    }
    [HttpPost("reserve")]
    public async Task<Rooms> reserving([FromBody] ReserveInfo bookEntry)
    {
        var userEmail = User.FindFirst("Email").Value;
        var CompanyUniqueId = User.FindFirst("CompanyId").Value;
        var result = (await _bookingService.ReserveRoom(bookEntry, CompanyUniqueId));
        if (result != null)
        {
            await _userService.AddReservation(bookEntry, result, userEmail, CompanyUniqueId);
        }
        return result;
    }

    [Authorize(Roles = "admin")]
    [HttpPost("addRoom")]
    public async Task<IActionResult> addRoom([FromBody] RoomsInfo room)
    {
        // foreach (var claim in User.Claims)
        // {
        //     Console.WriteLine($"{claim.Type} : {claim.Value}");
        // }
        var CompanyUniqueId = User.FindFirst("CompanyId").Value;
        var newRoom = await _bookingService.RegisterRoom(room, CompanyUniqueId);
        return Ok(newRoom);
    }

    [HttpGet("mybookings")]
    public async Task<IActionResult> showMyBookings()
    {
        var userEmail = User.FindFirst("Email").Value;
        var CompanyUniqueId = User.FindFirst("CompanyId").Value;
        var list = await _userService.MyBookings(userEmail, CompanyUniqueId);
        // var updatedList=await _
        return Ok(list);
    }

    [HttpPost("scheduleMeeting")]
    public async Task<Rooms> scheduleMeeting([FromBody] ScheduleRequest bookEntry)
    {
        var userEmail = User.FindFirst("Email").Value;
        var CompanyUniqueId = User.FindFirst("CompanyId").Value;
        var result = (await _bookingService.ReserveForScheduleMeet(bookEntry, CompanyUniqueId));
        if (result != null)
        {
            ReserveInfo entry = new ReserveInfo
            {
                FloorId = bookEntry.FloorId,
                BuildingId = bookEntry.BuildingId,
                RoomsType = bookEntry.RoomsType,
                StartTime = result.Value.Item2,
                EndTime = result.Value.Item3
            };
            await _userService.AddReservation(entry, result.Value.Item1, userEmail, CompanyUniqueId);
        }
        return result.Value.Item1;
    }



}