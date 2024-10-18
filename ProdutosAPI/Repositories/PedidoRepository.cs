using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ProdutosAPI.Context;
using ProdutosAPI.DTOs.PedidosDTO;
using ProdutosAPI.Models;
using ProdutosAPI.Repositories.Interfaces;
using ProdutosAPI.StringFormat;

using X.PagedList;

namespace ProdutosAPI.Repositories;

public class PedidoRepository : Repository<Pedido>, IPedidoRepository
{
    private readonly IMapper _mapper;
    public PedidoRepository(AppDbContext contexto, IMapper mapper) :
        base(contexto)
    {
        _mapper = mapper;
    }


    public async Task<IPagedList<PedidoDTO>> GetPedidos(int pageNumber, int pageSize)
    {
        var pedidosQuery = Get()
         .Include(p => p.PedidoItens)
         .ThenInclude(pi => pi.Produto)
             .ThenInclude(produto => produto.Dimensoes)
             .Include(p => p.Caixas)
             .OrderBy(p => p.Id);


        // Calcula o total de pedidos
        var totalPedidos = await pedidosQuery.CountAsync();

        // Realiza a paginação manualmente
        var pedidosPaginados = await pedidosQuery
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        // Mapeia para DTO diretamente com AutoMapper
        var pedidosDto = _mapper.Map<List<PedidoDTO>>(pedidosPaginados);

        // Cria a lista paginada usando StaticPagedList
        var pedidosPagedList = new StaticPagedList<PedidoDTO>(pedidosDto, pageNumber, pageSize, totalPedidos);

        return pedidosPagedList;
    }


    public async Task<IPagedList<PedidoDTO>> GetPedidosByNomeCliente(string nomeCliente, int pageNumber, int pageSize)
    {
        // Remove acentos do nome do cliente fornecido
        var nomeClienteSemAcentos = nomeCliente.RemoveAccents();

        // Consulta inicial para obter todos os pedidos com seus itens e produtos
        var pedidosQuery = Get()
            .Include(p => p.PedidoItens)
                .ThenInclude(pi => pi.Produto)
                .ThenInclude(produto => produto.Dimensoes)
                .Include(p => p.Caixas)
            .OrderBy(p => p.Id);

        // Filtra os pedidos em memória, aplicando a lógica de remoção de acentos nos nomes
        var pedidosFiltrados = await pedidosQuery
            .ToListAsync();

        // Aplica o filtro de nome com remoção de acentos
        var pedidosFiltradosPorNome = pedidosFiltrados
            .Where(p => p.Nome.RemoveAccents().Contains(nomeClienteSemAcentos))
            .ToList();

        // Calcula o total de pedidos filtrados pelo nome do cliente
        var totalPedidos = pedidosFiltradosPorNome.Count;

        // Realiza a paginação manualmente
        var pedidosPaginados = pedidosFiltradosPorNome
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToList();

        // Mapeia para DTO diretamente com AutoMapper
        var pedidosDto = _mapper.Map<List<PedidoDTO>>(pedidosPaginados);

        // Cria a lista paginada usando StaticPagedList
        var pedidosPagedList = new StaticPagedList<PedidoDTO>(pedidosDto, pageNumber, pageSize, totalPedidos);

        return pedidosPagedList;
    }


    public async Task<PedidoDTO> GetPedidoDetalhesByIdAsync(int pedidoId)
    {
        var pedido = await _context.Pedidos
            .Include(p => p.PedidoItens)
                .ThenInclude(pi => pi.Produto)
                .ThenInclude(produto => produto.Dimensoes)
                .Include(p => p.Caixas)
            .FirstOrDefaultAsync(p => p.Id == pedidoId);

        return pedido == null ? null : _mapper.Map<PedidoDTO>(pedido);
    }



    public async Task DeleteAll()
    {
        var pedidos = await _context.Pedidos.ToListAsync();
        _context.Pedidos.RemoveRange(pedidos);
    }


    // Implementação do método RemoveRange
    public void RemoveRange(IEnumerable<Pedido> pedidos)
    {
        _context.Pedidos.RemoveRange(pedidos);
    }


}
