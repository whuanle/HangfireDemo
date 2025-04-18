using Hangfire;

namespace HangfireServer.Hangfires;

/// <summary>
/// JobActivator.
/// </summary>
public class HangfireActivator : JobActivator
{
    private readonly IServiceScopeFactory _serviceScopeFactory;

    /// <summary>
    /// Initializes a new instance of the <see cref="HangfireActivator"/> class.
    /// </summary>
    /// <param name="serviceScopeFactory"></param>
    public HangfireActivator(IServiceScopeFactory serviceScopeFactory)
    {
        _serviceScopeFactory = serviceScopeFactory ?? throw new ArgumentNullException(nameof(serviceScopeFactory));
    }

    /// <inheritdoc/>
    public override JobActivatorScope BeginScope(JobActivatorContext context)
    {
        return new HangfireJobActivatorScope(_serviceScopeFactory.CreateScope(), context.BackgroundJob.Id);
    }
}
