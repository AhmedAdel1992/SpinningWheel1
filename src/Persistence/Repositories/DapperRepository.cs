using System.Data;
using Microsoft.EntityFrameworkCore;

namespace Persistence.Repositories;

public abstract class DapperRepository
{
    private readonly DbContext _dbContext;

    protected DapperRepository(DbContext dbContext)
    {
        _dbContext = dbContext;
    }

    protected IDbConnection Connection => _dbContext.Database.GetDbConnection();
}