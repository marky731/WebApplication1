 namespace Intermediary.Interfaces;

public interface IJwtService
{
    string GenerateToken(int userId, string userEmail, string role);
}