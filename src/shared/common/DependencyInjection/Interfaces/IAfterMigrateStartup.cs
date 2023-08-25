namespace Common.DependencyInjection.Interfaces;

public interface IAfterMigrateStartup
{
    Task StartAsync(CancellationToken cancellationToken);
}