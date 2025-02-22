using System.Net.Mail;
using System.Text.RegularExpressions;

namespace SmartCache.Validation;

public static class EmailValidator
{
    
    private static readonly Regex EmailRegex = new Regex(
        @"^[^@\s]+@[^@\s]+\.[^@\s]+$", 
        RegexOptions.IgnoreCase);
    
    public static bool IsValidEmail(string? email)
    {
        if (string.IsNullOrWhiteSpace(email))
            return false;
        
        if (!EmailRegex.IsMatch(email))
            return false;
        
        try
        {
            var address = new MailAddress(email); // Doesn't strictly validate the domain format.
            return address.Address == email;
        }
        catch (FormatException)
        {
            return false;
        }
    }  
}