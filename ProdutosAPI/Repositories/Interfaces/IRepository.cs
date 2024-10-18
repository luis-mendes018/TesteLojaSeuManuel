using System.Linq.Expressions;

namespace ProdutosAPI.Repositories.Interfaces;

public interface IRepository<T>
{
    IQueryable<T> Get();

    Task<T> GetProdutoById(int id);

    Task<T> GetById(Expression<Func<T, bool>> predicate);
    void Add(T entity);
    void Update(T entity);
    void Delete(T entity);
}
