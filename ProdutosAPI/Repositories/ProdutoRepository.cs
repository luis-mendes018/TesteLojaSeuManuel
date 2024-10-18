using AutoMapper;

using Microsoft.EntityFrameworkCore;

using ProdutosAPI.Context;
using ProdutosAPI.DTOs.ProdutosDTO;
using ProdutosAPI.Models;
using ProdutosAPI.Repositories.Interfaces;
using ProdutosAPI.StringFormat;

using X.PagedList;

namespace ProdutosAPI.Repositories;

public class ProdutoRepository : Repository<Produto>, IProdutoRepository
{
    private readonly IMapper _mapper;

    public ProdutoRepository(AppDbContext context, IMapper mapper) : base(context)
    {
        _mapper = mapper;
    }


    public async Task<Produto> GetById(int produtoId)
    {
        // Chamando o método GetById do repositório base
        return await GetById(p => p.Id == produtoId);
    }


    public async Task<IPagedList<ProdutoDTO>> GetProdutos(int pageNumber, int pageSize)
    {
        var produtos = await Get().OrderBy(on => on.Id)
                                   .Include(p => p.Dimensoes)
                                   .ToPagedListAsync(pageNumber, pageSize);

        var produtosDto = _mapper.Map<List<ProdutoDTO>>(produtos);

        return new StaticPagedList<ProdutoDTO>(produtosDto, produtos.PageNumber, produtos.PageSize, produtos.TotalItemCount);
    }

    public async Task<IPagedList<ProdutoDTO>> GetProdutosPorNome(string nome, int pageNumber, int pageSize)
    {
        var nomeSemAcentos = nome.RemoveAccents();

        // Fetch all products first
        var produtos = await Get()
            .ToListAsync();

        // Apply in-memory filtering for accent removal
        var produtosFiltrados = produtos
            .Where(p => p.Nome.RemoveAccents().Contains(nomeSemAcentos))
            .ToList();

        var produtosDto = _mapper.Map<List<ProdutoDTO>>(produtosFiltrados);

        // Paginate in-memory data
        var pagedProdutosDto = produtosDto
            .ToPagedList(pageNumber, pageSize);

        return new StaticPagedList<ProdutoDTO>(pagedProdutosDto, pageNumber, pageSize, produtosDto.Count);
    }

}
