using ProdutosAPI.DTOs.ProdutosDTO;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProdutosAPI.DTOs.PedidosDTO;

public class PedidoDetalheDTO
{
    public int PedidoDetalheId { get; set; }
    public int PedidoId { get; set; }
    public int ProdutoId { get; set; }
    public int Quantidade { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal Preco { get; set; }

    public ProdutoDTO Produto { get; set; }
}
