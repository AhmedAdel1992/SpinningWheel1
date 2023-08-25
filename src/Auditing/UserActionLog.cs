using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Auditing;

public class UserActionLog
{
    private UserActionLog()
    {
    }

    public UserActionLog(EntityEntry entry, Func<string>? userId)
    {
        Id = Guid.NewGuid();
        ActionTime = DateTime.UtcNow;
        UserId = userId?.Invoke()??string.Empty;
        TableName = entry.Metadata.GetTableName();
        Type= entry.State.ToString();
        // if(entry.Entity is Baseen)
        // PrimaryKey= entry.Entity.Property("Id")?.CurrentValue.ToString();
        // var key=entry.Metadata.FindPrimaryKey()?.GetName();
        // PrimaryKey= entry.Property(key)?.CurrentValue?.ToString();
        if (entry.State != EntityState.Added)
            SetOriginalValues(entry.OriginalValues);
        if (entry.State != EntityState.Deleted)
            SetNewValues(entry.CurrentValues);
        SetChangedColumns(entry.Properties);
    }

    private void SetNewValues(PropertyValues entryOriginalValues)
    {
        foreach (var property in entryOriginalValues.Properties)
        {
            if (property.IsPrimaryKey())
            {
                this.PrimaryKey= entryOriginalValues[property.Name].ToString();
            }
            var value = entryOriginalValues[property.Name];
            if (value != null)
            {
                NewValues.Add(property.Name, value);
            }
        }
    }

    void SetOriginalValues(PropertyValues entryOriginalValues)
    {
        foreach (var property in entryOriginalValues.Properties)
        {
            var value = entryOriginalValues[property.Name];
            if (value != null)
            {
                OldValues.Add(property.Name, value);
            }
        }
    }
    void SetChangedColumns(IEnumerable<PropertyEntry> entryProperties)
    {
        foreach (var property in entryProperties)
        {
            if(property.IsModified)
            {
                ChangedColumns.Add(property.Metadata.Name);
            }
        }
    }

    public Guid Id { get; set; }
    public string UserId { get; set; }
    public string? Type { get; set; }
    public string? TableName { get; set; }
    public DateTime ActionTime { get; set; }
    public Dictionary<string, object>? OldValues { get; set; } = new();
    public Dictionary<string, object>? NewValues { get; set; } = new();
    public List<string> ChangedColumns { get; } = new();
    public string? PrimaryKey { get; set; }
}