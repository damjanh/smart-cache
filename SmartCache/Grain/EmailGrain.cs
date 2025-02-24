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
    private bool _changed;

    public Task<bool> SetEmail(string email)
    {
        if (state.State != null && email.Equals(state.State.Email))
        {
            logger.LogInformation("Email {Email} already exists in cache.", email);
            return Task.FromResult(false);
        }

        logger.LogInformation("Adding email: {Email} to cache.", email);
        _changed = true;
        state.State = new EmailDetail {
           Email = email
        };
       return Task.FromResult(true);
    }

    public Task<string?> GetEmail(string email)
    {
        logger.LogInformation("Retrieving email: {Email} from cache.", email);
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
        if (!_changed) return;
        await state.WriteStateAsync(); // Persist state to Azure Blob Storage
        logger.LogInformation("Persisting email: {Email}", state.State.Email);
    }

}

[GenerateSerializer, Alias(nameof(EmailDetail))]
public sealed record EmailDetail
{
    [Id(0)] public string Email { get; set; } = "";
}