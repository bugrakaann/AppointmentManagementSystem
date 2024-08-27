using System.Linq.Expressions;

namespace Data_Access_Layer.Repositories.Abstract;

public interface IRepository<T>
{
    public Task<IEnumerable<T>> GetAll();
    public Task<T> GetById(int id);
    public Task<T> Add(T entity);
    public Task<T> Update(T entity);
    public Task Delete(int id);
    public Task Delete(T entity);
    public Task<IEnumerable<T>> Find(Expression<Func<T, bool>> predicate);
    public Task<bool> Contains(Expression<Func<T, bool>> predicate);
}