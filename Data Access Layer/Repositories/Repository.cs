using System.Linq.Expressions;
using Data_Access_Layer.Data;
using Microsoft.EntityFrameworkCore;
using Models.Models;

namespace Data_Access_Layer.Repositories;

public class Repository<T> : IRepository<T> where T : class, IEntity
{
    protected readonly ApplicationDbContext _context;
    protected readonly DbSet<T> _dbSet;

    public Repository(ApplicationDbContext context)
    {
        _context = context;
        _dbSet = _context.Set<T>();
    }

    private void IncludeRelationships()
    {
        if(typeof(T) == typeof(Customer))
        {
            (_dbSet as DbSet<Customer>)?.Include(c => c.Appointments);
        }
        else if (typeof(T) == typeof(Appointment))
        {
            (_dbSet as DbSet<Appointment>)?.Include(a => a.customer);
        }
    }
    
    public IEnumerable<T> GetAll()
    {
        IncludeRelationships();
        return _dbSet.ToList();
    }

    public T GetById(int id)
    {
        IncludeRelationships();
        return _dbSet.FirstOrDefault(a => a.id == id);
    }

    public void Add(T entity)
    {
        _dbSet.Add(entity);
        _context.SaveChanges();
    }

    public void Update(T entity)
    {
        var existingEntity = _dbSet.Local.FirstOrDefault(e => e.id == (entity as IEntity).id);
        if (existingEntity != null)
        {
            _context.Entry(existingEntity).State = EntityState.Detached;
        }
        _dbSet.Attach(entity);
        _context.Entry(entity).State = EntityState.Modified;
        _context.SaveChanges();
    }

    public void Delete(int id)
    {
        T entity = _dbSet.FirstOrDefault(a=>a.id == id);
        if(entity == null) return;
        _dbSet.Remove(entity);
        _context.SaveChanges();
    }

    public void Delete(T entity)
    {
        _dbSet.Remove(entity);
        _context.SaveChanges();
    }

    public IEnumerable<T> Find(Expression<Func<T, bool>> predicate)
    {
        return _context.Set<T>().Where(predicate).ToList();
    }
}