using Microsoft.EntityFrameworkCore.Diagnostics;

namespace Auditing;

public class TrackDataInterceptor : SaveChangesInterceptor
{
    private readonly IBaseAuditService _baseAuditService;
    private readonly Func<string> _getUserId;
    public TrackDataInterceptor(IBaseAuditService baseAuditService)
    {
        _baseAuditService = baseAuditService;
    }

    public override async ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData,
        InterceptionResult<int> result,
        CancellationToken cancellationToken = default)
    {
        if (eventData.Context is not IAuditDbContext context)
        {
            return await base.SavingChangesAsync(eventData, result, cancellationToken);
        }


        var entries = eventData.Context.ChangeTracker.Entries<IAuditableEntity>();

        var saveChangeResult = await base.SavingChangesAsync(eventData, result, cancellationToken);
        List<UserActionLog> userActionLogs = new();
        foreach (var entry in entries)
        {
            userActionLogs.Add(new UserActionLog(entry, _baseAuditService.GetUserId));
        }

        try
        {
            await eventData.Context.Set<UserActionLog>().AddRangeAsync(userActionLogs, cancellationToken);

            await base.SavingChangesAsync(eventData, result, cancellationToken);
        }
        catch (Exception e)
        {
        }
        finally
        {
        }

        return saveChangeResult;
    }
}