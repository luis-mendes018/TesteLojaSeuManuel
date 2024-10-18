using System.ComponentModel.DataAnnotations;

namespace ProdutosAPI.DTOs.PedidosDTO;

public class CriarPedidoDTO
{
    [Required(ErrorMessage = "Informe o nome")]
    [StringLength(50, ErrorMessage = "O nome ultrapassou o limite de 50 caracteres!")]
    public string Nome { get; set; }

    [Required(ErrorMessage = "Informe o seu sobrenome")]
    [StringLength(50, ErrorMessage = "O sobrenome ultrapassou o limite de 50 caracteres!")]
    public string Sobrenome { get; set; }
}
