using System.Linq.Expressions;
using AuthServer.Core.Repositories;
using Microsoft.EntityFrameworkCore;

namespace AuthServer.Data.Repositories;

public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : class
{
    private readonly DbContext _context;
    private readonly DbSet<TEntity> _dbSet;

    public GenericRepository(AppDbContext context)
    {
        _context = context;
        _dbSet = context.Set<TEntity>();
    }

    public async Task<TEntity> GetByIdAsync(int id)
    {
        var entity = await _dbSet.FindAsync(id);
        if (entity is not null)
        {
            _context.Entry(entity).State = EntityState.Detached;
        }
        return entity;
    }

    public async Task<IEnumerable<TEntity>> GetAllAsync() => await _dbSet.ToListAsync();

    public IQueryable<TEntity> Where(Expression<Func<TEntity, bool>> predicate) => _dbSet.Where(predicate);

    public async Task AddAsync(TEntity entity) => await _dbSet.AddAsync(entity);

    public void Remove(TEntity entity) => _dbSet.Remove(entity);

    public TEntity Update(TEntity entity)
    {
        _context.Entry(entity).State = EntityState.Modified;
        return entity;
    }
}