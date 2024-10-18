using System.ComponentModel.DataAnnotations.Schema;

namespace ProdutosAPI.Models;

public class PedidoDetalhe
{
    public int Id { get; set; }
    public int PedidoId { get; set; }
    public int ProdutoId { get; set; }
    public int Quantidade { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal Preco { get; set; }

    public virtual Produto Produto { get; set; }
    public virtual Pedido Pedido { get; set; }
}
