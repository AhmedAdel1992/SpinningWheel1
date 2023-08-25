using System;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.ValueGeneration;
using NUlid;
using NUlid.Rng;

namespace Persistence.ValueGenerators;

public class SeqGuidIdValueGenerator : ValueGenerator<Guid>
{
    public override bool GeneratesTemporaryValues => false;
    public override Guid Next(EntityEntry entry)
    {
        var rng = new MonotonicUlidRng();
        return Ulid.NewUlid(rng).ToGuid();
    }   
    public  Guid Next()
    {
        var rng = new MonotonicUlidRng();
        return Ulid.NewUlid(rng).ToGuid();
    }
}

public class SeqIdValueGenerator : ValueGenerator<string>
{
    public override bool GeneratesTemporaryValues => false;
    public override string Next(EntityEntry entry)
    {
        var rng = new MonotonicUlidRng(); 
        return Ulid.NewUlid(rng).ToString().ToLower();
    }
}