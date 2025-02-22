namespace SmartCache.Grain;

public interface IEmail
{
    Task<bool> SetEmail(string email);
    Task<string?> GetEmail(string email); 
}