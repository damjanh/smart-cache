namespace SmartCache.Grain;

public interface IEmail : IGrainWithStringKey
{
    Task<bool> SetEmail(string email);
    Task<string?> GetEmail(string email); 
}