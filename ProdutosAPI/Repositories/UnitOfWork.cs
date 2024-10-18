
using AutoMapper;

using Microsoft.EntityFrameworkCore;

using ProdutosAPI.Context;
using ProdutosAPI.Repositories.Interfaces;

namespace ProdutosAPI.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private ProdutoRepository _produtoRepo;
    private PedidoRepository _pedidoRepo;
    public AppDbContext _context;
    private readonly IMapper _mapper;

    public UnitOfWork(AppDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }


    public IProdutoRepository ProdutoRepository
    {
        get
        {
            return _produtoRepo = _produtoRepo ?? new ProdutoRepository(_context, _mapper);
        }
    }

    public IPedidoRepository PedidoRepository
    {
        get
        {
            return (_pedidoRepo = _pedidoRepo ?? new PedidoRepository(_context, _mapper));
        }
    }
    public async Task Commit()
    {
        await _context.SaveChangesAsync();
    }

    public void Dispose()
    {
        _context.Dispose();
    }
}
