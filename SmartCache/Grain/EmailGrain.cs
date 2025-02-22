namespace SmartCache.Grain;

public class EmailGrain(
    ILogger<EmailGrain> logger,
    [PersistentState(
        stateName: "emails",
        storageName: "emails"
        )] IPersistentState<EmailDetail> state) : Orleans.Grain, IEmail
{
    private const int PersistPeriodMinutes = 5;
    private const int PersistDueMinutes = 5;
    
    private IGrainTimer? _timer;

    public Task<bool> SetEmail(string email)
    {
        if (state.State != null && email.Equals(state.State.Email))
        {
            logger.LogInformation("Email already exists");
            return Task.FromResult(false);
        }

        logger.LogInformation("Setting email {Email}", email);
       state.State = new EmailDetail {
           Email = email
       };
       return Task.FromResult(true);
    }

    public Task<string?> GetEmail(string email)
    {
        logger.LogInformation("Getting email {Email}", email);
        return Task.FromResult(state.State.Email)!;
    }

    public override async Task OnActivateAsync(CancellationToken cancellationToken)
    {
        // Restore state on activation
        await state.ReadStateAsync(cancellationToken);

        // Start a timer to save state every x minutes
        _timer = this.RegisterGrainTimer<object?>(
            callback: SaveStateAsync,
            state: null,
            dueTime: TimeSpan.FromMinutes(PersistDueMinutes),
            period: TimeSpan.FromMinutes(PersistPeriodMinutes)
        );
    }

    public override async Task OnDeactivateAsync(DeactivationReason reason, CancellationToken cancellationToken)
    {
        _timer?.Dispose(); // Clean up the timer
        await state.WriteStateAsync(cancellationToken); // Ensure final save before deactivation
        await base.OnDeactivateAsync(reason, cancellationToken);
    }
    
    private async Task SaveStateAsync(object? _)
    {
        await state.WriteStateAsync(); // Persist state to Azure Blob Storage
        logger.LogInformation("Saving state for email: {Email}", state.State.Email);
    }

}

[GenerateSerializer, Alias(nameof(EmailDetail))]
public sealed record EmailDetail
{
    [Id(0)] public string Email { get; set; } = "";
}