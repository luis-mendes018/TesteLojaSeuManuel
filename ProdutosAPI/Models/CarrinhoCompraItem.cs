using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProdutosAPI.Models;

public class CarrinhoCompraItem
{
    public int CarrinhoCompraItemId { get; set; }
    public Produto Produto { get; set; }
    public int Quantidade { get; set; }
    public int ProdutoId { get; set; }

    [StringLength(200)]
    public string CarrinhoCompraId { get; set; }
    public DateTime CreatedAt { get; set; }

    public Dimensoes Dimensoes { get; set; }
}
