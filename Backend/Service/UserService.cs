using BCrypt.Net;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
public class UserService : IUserService
{
        private readonly IUserRepository _userRepository;
        public UserService(IUserRepository userRepository) => _userRepository = userRepository;

        public async Task<Users> RegisterUser(Users user){
            await _userRepository.AddCompany(user.CompanyName,user.CompanyUniqueId);
            return await _userRepository.AddUser(user);
        }

        public async Task<LoginResult> AuthenticateUserAsync(string Email,string Password,string companyId){
            var user=await _userRepository.GetUserByEmail(Email,companyId);
            if(user == null){
                return new LoginResult{ IsSuccess=false, Message="Email Not Found"};
            }
            bool isPasswordCorrect=BCrypt.Net.BCrypt.Verify(Password,user.Password);
            if(!isPasswordCorrect){
                return new LoginResult{ IsSuccess=false, Message="Incorrect Password"};
            }
            return new LoginResult{IsSuccess=true, User=user};
        }

        public async Task<IEnumerable<Users>> GetAllUsers(string CompanyUniqueId){
            return await _userRepository.GetAllUsersOfCompany(CompanyUniqueId);
        }

        public async Task<Bookings> AddReservation(ReserveInfo reserve,Rooms room,string email,string companyId){
            var user = await _userRepository.GetUserByEmail(email,companyId);
            int companyIdVal = await _userRepository.GetCompanyId(companyId);
            Bookings ans = new Bookings
            {
                UserId = user.Id,
                CompanyId=companyIdVal,
                FloorId = reserve.FloorId,
                BuildingId = reserve.BuildingId,
                RoomsId = room.RoomId,
                RoomsType = reserve.RoomsType,
                StartTime = reserve.StartTime,
                EndTime = reserve.EndTime,
                IsExpired=false
            };
            var book= await _userRepository.AddBooking(ans);
            return book;
        }
        
        // public async Task<IEnumerable<Bookings>> AllBookings()
        // {
        //     return await _userRepository.GetAllBookings();
        // }
        
        public async Task<IEnumerable<Bookings>> MyBookings(string email,string companyId)
        {
           var user =await  _userRepository.GetUserByEmail(email,companyId);
           int companyIdVal=await _userRepository.GetCompanyId(companyId);
           return await _userRepository.GetMyBookings(user.Id,companyIdVal);
        }
        
}