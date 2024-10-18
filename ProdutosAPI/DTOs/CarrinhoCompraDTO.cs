using ProdutosAPI.DTOs.ProdutosDTO;
using System.ComponentModel.DataAnnotations;

namespace ProdutosAPI.DTOs;

public class CarrinhoCompraDTO
{
    public int CarrinhoCompraItemId { get; set; }
    public ProdutoDTO Produto { get; set; }
    public int Quantidade { get; set; }

    [StringLength(200)]
    public string CarrinhoCompraId { get; set; }
}
