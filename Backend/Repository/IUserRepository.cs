using Microsoft.AspNetCore.Mvc;

public interface IUserRepository
{
        Task<Users> AddUser(Users user);
        Task<int> AddCompany(string CompanyName, string CompanyUniqueId);
        Task<Users> GetUserByEmail(string Email,string CompanyUniqueId);
        Task<IEnumerable<Users>> GetAllUsersOfCompany(string CompanyUniqueId);
        Task<Bookings> AddBooking(Bookings book);
        Task<int> GetCompanyId (string CompanyUniqueId);
        // Task<IEnumerable<Bookings>> GetAllBookings();
        Task<IEnumerable<Bookings>> GetMyBookings(int userId,int companyId);
}