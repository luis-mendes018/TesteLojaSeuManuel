namespace ProdutosAPI.Repositories.Interfaces;

public interface IUnitOfWork
{
    IProdutoRepository ProdutoRepository { get; }
    IPedidoRepository PedidoRepository { get; }

    Task Commit();
}
