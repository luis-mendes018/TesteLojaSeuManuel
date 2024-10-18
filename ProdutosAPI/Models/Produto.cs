namespace ProdutosAPI.Models;

public class Produto
{
    public int Id { get; set; }

    public string Nome { get; set; }

    public decimal Preco { get; set; }

    // Propriedade de navegação para a classe Dimensoes
    public int DimensoesId { get; set; }
    public Dimensoes Dimensoes { get; set; }
}
