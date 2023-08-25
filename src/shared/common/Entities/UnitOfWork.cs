namespace Common.Entities;

public interface IUnitOfWork : IDisposable
{
    void Commit();
    void Rollback();
}
public class UnitOfWork:IUnitOfWork
{
    private readonly IDbContext _context;

    public UnitOfWork(IDbContext context)
    {
        _context = context;
    }
    public void Dispose()
    {
        throw new NotImplementedException();
    }

    public void Commit()
    {
        throw new NotImplementedException();
    }

    public void Rollback()
    {
        throw new NotImplementedException();
    }
}