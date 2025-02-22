namespace SmartCache.Services;

public interface IEmailsService
{
    string? GetEmail(string email); 
    bool SetEmail(string email); 
}