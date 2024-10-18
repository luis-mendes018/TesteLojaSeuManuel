using ProdutosAPI.DTOs.PedidosDTO;
using ProdutosAPI.Models;
using X.PagedList;

namespace ProdutosAPI.Repositories.Interfaces;

public interface IPedidoRepository : IRepository<Pedido>
{
    Task<IPagedList<PedidoDTO>> GetPedidos(int pageNumber, int pageSize);
    Task<IPagedList<PedidoDTO>> GetPedidosByNomeCliente(string nomeCliente, int pageNumber, int pageSize);
    Task<PedidoDTO> GetPedidoDetalhesByIdAsync(int pedidoId);


    Task DeleteAll();


    void RemoveRange(IEnumerable<Pedido> pedidos);


}
