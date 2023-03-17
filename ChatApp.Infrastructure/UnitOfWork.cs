using ChatApp.Infrastructure.Interfaces;

namespace ChatApp.Infrastructure;

public class UnitOfWork : IUnitOfWork
{
    private readonly IChatDbContext _dbContext;

    public UnitOfWork(IChatDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<int> CommitAsync()
    {
        return await _dbContext.SaveChangesAsync();
    }

    public void Dispose()
    {
        throw new NotImplementedException();
    }
}