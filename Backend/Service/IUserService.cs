using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ActionConstraints;

public interface IUserService
{
        Task<Users> RegisterUser(Users user);
        Task<LoginResult> AuthenticateUserAsync(string Email,string Password,string companyId);
        Task<IEnumerable<Users>> GetAllUsers(string CompanyUniqueId);
        Task<Bookings> AddReservation(ReserveInfo reserve,Rooms room,string email,string companyId);
        // Task<IEnumerable<Bookings>> AllBookings();
        Task<IEnumerable<Bookings>> MyBookings(string email,string companyId);

}