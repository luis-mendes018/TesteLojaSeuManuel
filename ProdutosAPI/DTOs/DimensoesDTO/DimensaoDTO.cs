using System.ComponentModel.DataAnnotations;

namespace ProdutosAPI.DTOs.DimensoesDTO;

public class DimensaoDTO
{
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
