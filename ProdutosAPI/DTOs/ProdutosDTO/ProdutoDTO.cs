using System.ComponentModel.DataAnnotations;
using ProdutosAPI.DTOs.DimensoesDTO;

namespace ProdutosAPI.DTOs.ProdutosDTO;

public class ProdutoDTO
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Digite o nome do produto")]
    [StringLength(200, ErrorMessage = "Esse campo aceita no máximo 200 caracteres")]
    public string Nome { get; set; }

    [Required(ErrorMessage = "Digite o preço do produto!")]
    [Range(1, double.MaxValue, ErrorMessage = "O preço deve ser a partir de 1,00")]
    public decimal Preco { get; set; }

    [Required(ErrorMessage = "Digite as dimensões do produto")]
    public DimensaoDTO Dimensoes { get; set; }
}
