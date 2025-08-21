using System.Security.Claims;
public interface IJwtTokenService
{
    string GenerateToken(string email,string role,string companyId);
}