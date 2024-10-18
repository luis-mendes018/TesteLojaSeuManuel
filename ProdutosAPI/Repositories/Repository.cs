using Microsoft.EntityFrameworkCore;
using ProdutosAPI.Context;
using System.Linq.Expressions;

using ProdutosAPI.Repositories.Interfaces;

namespace ProdutosAPI.Repositories;

public class Repository<T> : IRepository<T> where T : class
{
    protected AppDbContext _context;

    public Repository(AppDbContext contexto)
    {
        _context = contexto;
    }

    public IQueryable<T> Get()
    {
        return _context.Set<T>().AsNoTracking();
    }

    public async Task<T> GetById(Expression<Func<T, bool>> predicate)
    {
        return await _context.Set<T>().SingleOrDefaultAsync(predicate);
    }

    public void Add(T entity)
    {
        _context.Set<T>().Add(entity);
    }

    public void Delete(T entity)
    {
        _context.Set<T>().Remove(entity);
    }

    public void Update(T entity)
    {
        _context.Entry(entity).State = EntityState.Modified;
        _context.Set<T>().Update(entity);
    }

    public async Task<T> GetProdutoById(int id)
    {
        return await _context.Set<T>().FindAsync(id);
    }
}
