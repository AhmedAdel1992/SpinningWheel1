namespace Infrastructure.Jobs.Base;

public interface IJobService
{
}

public class JobLocker : IDisposable
{
    public void Dispose()
    {
    }
}