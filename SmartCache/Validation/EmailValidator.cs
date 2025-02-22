using System.Net.Mail;

namespace SmartCache.Validation;

public static class EmailValidator
{
    public static bool IsValidEmail(string? email)
    {
        if (string.IsNullOrWhiteSpace(email))
            return false;
        
        try
        {
            var address = new MailAddress(email);
            return address.Address == email;
        }
        catch (FormatException)
        {
            return false;
        }
    }  
}