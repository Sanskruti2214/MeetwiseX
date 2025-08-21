using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.VisualBasic;
public class UserRepository : IUserRepository
{
    private readonly AppDbContext _context;
    public UserRepository(AppDbContext context) => _context = context;

    public async Task<Users> AddUser(Users user)
    {
        var userSafe = new Users
        {
            Email = user.Email,
            Username = user.Username,
            Password = BCrypt.Net.BCrypt.HashPassword(user.Password),
            CompanyUniqueId  = user.CompanyUniqueId,
            CompanyName=user.CompanyName
        };
        var exist = await _context.Users.AnyAsync(u =>
            u.Email == user.Email &&
            u.Username == user.Username &&
            u.CompanyUniqueId  == user.CompanyUniqueId &&
            u.CompanyName==user.CompanyName
        );
        if (exist)
        {
            return userSafe;
        }
        _context.Users.Add(userSafe);
        await _context.SaveChangesAsync();
        return userSafe;
    }
    public async Task<int> AddCompany(string CompanyName,string CompanyUniqueId)
    {   
        var companyEntry = new Company
        {
            CompanyName = CompanyName,
            CompanyUniqueId = CompanyUniqueId
        };
        var exist = await _context.Company.AnyAsync(c =>
            c.CompanyName==CompanyName &&
            c.CompanyUniqueId == CompanyUniqueId
        );
        var entry = await _context.Company.FirstOrDefaultAsync(c=>c.CompanyName==CompanyName && c.CompanyUniqueId==CompanyUniqueId);
        if (exist)
        {
            return  entry.companyId;
        }
        
        _context.Company.Add(companyEntry);
        await _context.SaveChangesAsync();
        return  entry.companyId;
    }
    
    public async Task<Users> GetUserByEmail(string Email, string CompanyUniqueId)
    {
        return await _context.Users.FirstOrDefaultAsync(u => u.Email == Email && u.CompanyUniqueId == CompanyUniqueId);
    }

    public async Task<IEnumerable<Users>> GetAllUsersOfCompany(string CompanyUniqueId )
    {
        return await _context.Users.Where(c=>c.CompanyUniqueId==CompanyUniqueId).ToListAsync();
    }

    public async Task<Bookings> AddBooking(Bookings book)
    {
        _context.Bookings.Add(book);
        await _context.SaveChangesAsync();
        return book;
    }

    public async Task<int> GetCompanyId(string CompanyUniqueId)
    {
        var company = await _context.Company.FirstOrDefaultAsync(c => c.CompanyUniqueId == CompanyUniqueId);
        return company.companyId;
    }
    
    public async Task<IEnumerable<Bookings>> GetMyBookings(int userId, int companyId)
    {
        var result = await _context.Bookings.Where(b => b.UserId == userId && b.CompanyId == companyId).
        ToListAsync();
        return result;
    }
}