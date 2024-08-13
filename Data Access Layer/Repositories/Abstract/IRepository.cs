using System.Linq.Expressions;

namespace Data_Access_Layer.Repositories;

public interface IRepository<T>
{
    public IEnumerable<T> GetAll();
    public T GetById(int id);
    public void Add(T entity);
    public void Update(T entity);
    public void Delete(int id);
    public void Delete(T entity);
    public IEnumerable<T> Find(Expression<Func<T, bool>> predicate);
}