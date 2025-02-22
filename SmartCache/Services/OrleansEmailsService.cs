using SmartCache.Grain;

namespace SmartCache.Services;

public class OrleansEmailsService(IGrainFactory grains) : IEmailsService
{
    public async Task<string?> GetEmail(string email)
    {
        var grain = grains.GetGrain<IEmail>(email);
        var returned = await grain.GetEmail(email);
        return returned;
    }

    public async Task<bool> SetEmail(string email)
    {
        var grain = grains.GetGrain<IEmail>(email);
        var success = await grain.SetEmail(email);
        return success;
    }
}