using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ProdutosAPI.DTOs.PedidosDTO;

public class PedidoDTO
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Informe o nome")]
    [StringLength(50, ErrorMessage = "O nome ultrapassou o limite de 50 caracteres!")]
    public string Nome { get; set; }

    public string PedidoEnviado { get; set; }

    public int TotalItensPedido { get; set; }

    [ScaffoldColumn(false)]
    [Column(TypeName = "decimal(18,2)")]
    public decimal PedidoTotal { get; set; }

    public List<CaixaResponseDTO> Caixas { get; set; }
}
