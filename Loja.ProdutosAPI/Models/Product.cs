using System.ComponentModel.DataAnnotations;

namespace Loja.ProdutosAPI.Models;

public class Product
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Digite o nome do produto")]
    [StringLength(200, ErrorMessage = "Esse campo aceita no máximo 200 caracteres")]
    public string? Nome { get; set; }

    [Required(ErrorMessage = "Digite o preço do produto!")]
    [Range(1, double.MaxValue, ErrorMessage = "O preço deve ser a partir de 1,00")]
    public decimal Preco { get; set; }

    [Required(ErrorMessage = "Digite a altura do produto!")]
    [Range(1, double.MaxValue, ErrorMessage = "O valor deve ser maior que 0")]
    public decimal Altura { get; set; }

    [Required(ErrorMessage = "Digite a largura do produto")]
    [Range(1, double.MaxValue, ErrorMessage = "O valor deve ser maior que 0")]
    public decimal Largura { get; set; }

    [Required(ErrorMessage = "Digite o comprimento do produto")]
    [Range(1, double.MaxValue, ErrorMessage = "O valor deve ser maior que 0")]
    public decimal Comprimento { get; set; }
}
