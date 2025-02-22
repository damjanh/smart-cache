namespace SmartCache.Services;

public interface IEmailsService
{
    Task<string?> GetEmail(string email); 
    Task<bool> SetEmail(string email); 
}