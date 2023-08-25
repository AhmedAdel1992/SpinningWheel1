using Microsoft.EntityFrameworkCore;

namespace Auditing;

public interface IAuditDbContext
{
}
public abstract class BaseDbContext:DbContext
{
    private readonly IBaseAuditService _baseAuditService;

    protected BaseDbContext(DbContextOptions options, IBaseAuditService baseAuditService = null)
        : base(options)
    {
        _baseAuditService = baseAuditService;
    }

    public string? GetUserId() => _baseAuditService.GetUserId();
    public DbSet<UserActionLog> UserActionLogs { get; set; }
}

