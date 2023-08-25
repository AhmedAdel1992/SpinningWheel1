using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using Persistence;

namespace MSSql;

public class MSSqlDbContext:AppDbContext
{
    public MSSqlDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
        // test
    }
}




