using System.Linq.Expressions;
using Data_Access_Layer.Data;
using Data_Access_Layer.Repositories.Abstract;
using Microsoft.EntityFrameworkCore;
using Models.Models;

namespace Data_Access_Layer.Repositories;

public class Repository<T> : IRepository<T> where T : Entity
{
    protected readonly ApplicationDbContext _context;
    protected readonly DbSet<T> _dbSet;

    public Repository(ApplicationDbContext context)
    {
        _context = context;
        _dbSet = _context.Set<T>();
    }

    public async Task<IEnumerable<T>> GetAll()
    {
        return await _dbSet.ToListAsync();
    }

    public async Task<T> GetById(int id)
    {
        return await _dbSet.FirstAsync(a => a.id == id);
    }

    public async Task<T> Add(T entity)
    {
        await _dbSet.AddAsync(entity);
        await _context.SaveChangesAsync();
        return entity;
    }

    public async Task<T> Update(T entity)
    {
        var existingEntity = _dbSet.Local.FirstOrDefault(e => e.id == (entity as Entity).id);
        if (existingEntity != null)
        {
            _context.Entry(existingEntity).State = EntityState.Detached;
        }

        _dbSet.Attach(entity);
        _context.Entry(entity).State = EntityState.Modified;
        await _context.SaveChangesAsync();
        return entity;
    }

    public async Task Delete(int id)
    {
        var entity = await _dbSet.FirstOrDefaultAsync(a => a.id == id);
        if (entity == null) return;
        _dbSet.Remove(entity);
        await _context.SaveChangesAsync();
    }

    public async Task Delete(T entity)
    {
        _dbSet.Remove(entity);
        await _context.SaveChangesAsync();
    }

    public async Task<IEnumerable<T>> Find(Expression<Func<T, bool>> predicate)
    {
        return await _context.Set<T>().Where(predicate).ToListAsync();
    }

    public async Task<bool> Contains(Expression<Func<T, bool>> predicate)
    {
        return await _context.Set<T>().Where(predicate).AnyAsync();
    }
}