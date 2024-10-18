using ProdutosAPI.DTOs.ProdutosDTO;
using ProdutosAPI.Models;

using X.PagedList;

namespace ProdutosAPI.Repositories.Interfaces;

public interface IProdutoRepository : IRepository<Produto>
{
    Task<IPagedList<ProdutoDTO>> GetProdutos(int pageNumber, int pageSize);
    Task<IPagedList<ProdutoDTO>> GetProdutosPorNome(string nome, int pageNumber, int pageSize);
}
