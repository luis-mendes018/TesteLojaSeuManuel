using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ProdutosAPI.Models;

public class Pedido
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Informe o nome")]
    [StringLength(50, ErrorMessage = "O nome ultrapassou o limite de 50 caracteres!")]
    public string Nome { get; set; }

    [ScaffoldColumn(false)]
    [Column(TypeName = "decimal(18,2)")]
    public decimal PedidoTotal { get; set; }

    [ScaffoldColumn(false)]
    public int TotalItensPedido { get; set; }

    public DateTime PedidoEnviado { get; set; }

    public List<PedidoDetalhe> PedidoItens { get; set; }

    public string UsuarioId { get; set; }

    public List<Caixa> Caixas { get; set; }

}
