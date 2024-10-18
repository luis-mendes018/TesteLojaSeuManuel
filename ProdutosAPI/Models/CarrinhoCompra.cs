using Microsoft.EntityFrameworkCore;

using ProdutosAPI.Context;

namespace ProdutosAPI.Models;

public class CarrinhoCompra
{
    private readonly AppDbContext _context;
    public string CarrinhoCompraId { get; private set; }
    public List<CarrinhoCompraItem> CarrinhoCompraItens { get; set; }
    public DateTime? DataExpiracao { get; set; }

    public CarrinhoCompra(AppDbContext context, string userId)
    {
        _context = context;
        CarrinhoCompraId = userId; // Usa o userId como identificador do carrinho
        CarrinhoCompraItens = new List<CarrinhoCompraItem>();
    }


    public static async Task<CarrinhoCompra> CreateAsync(AppDbContext context, string userId)
    {
        var carrinhoCompra = new CarrinhoCompra(context, userId);

        // Verifica se o carrinho já existe
        var existingCarrinho = await context.CarrinhoCompraItens
            .Where(c => c.CarrinhoCompraId == carrinhoCompra.CarrinhoCompraId)
            .ToListAsync();

        carrinhoCompra.CarrinhoCompraItens = existingCarrinho.Any() ? existingCarrinho : new List<CarrinhoCompraItem>();
        return carrinhoCompra;
    }

    public async Task AdicionarAoCarrinho(Produto produto)
    {
        
        var carrinhoCompraItem = await _context.CarrinhoCompraItens
            .FirstOrDefaultAsync(c => c.Produto.Id == produto.Id && c.CarrinhoCompraId == CarrinhoCompraId);

        if (carrinhoCompraItem == null)
        {
            carrinhoCompraItem = new CarrinhoCompraItem
            {
                CarrinhoCompraId = CarrinhoCompraId,
                Produto = produto,
                Dimensoes = produto.Dimensoes,
                Quantidade = 1,
                CreatedAt = DateTime.Now
            };
            await _context.CarrinhoCompraItens.AddAsync(carrinhoCompraItem);
        }
        else
        {
            carrinhoCompraItem.Quantidade++;
        }

        await _context.SaveChangesAsync();
    }

    public void RemoverDoCarrinho(Produto produto)
    {
        var carrinhoCompraItem = _context.CarrinhoCompraItens.FirstOrDefault(
            c => c.Produto.Id == produto.Id && c.CarrinhoCompraId == CarrinhoCompraId);

        if (carrinhoCompraItem != null)
        {
            
            if (carrinhoCompraItem.Quantidade > 1)
            {
                carrinhoCompraItem.Quantidade--;
            }
            else
            {
                _context.CarrinhoCompraItens.Remove(carrinhoCompraItem);
            }

            _context.SaveChanges();
        }
    }

    public async Task<List<CarrinhoCompraItem>> GetCarrinhoCompraItens()
    {
        Console.WriteLine($"Recuperando itens para o carrinho com ID: {CarrinhoCompraId}");

        var itens = await _context.CarrinhoCompraItens
         .Where(c => c.CarrinhoCompraId == CarrinhoCompraId)
         .Include(c => c.Produto)
         .ThenInclude(p => p.Dimensoes)
         .ToListAsync();

        Console.WriteLine($"Número de itens encontrados: {itens.Count}");

        return itens;
    }

    public async Task LimparCarrinho()
    {
        var carrinhoItens = await _context.CarrinhoCompraItens
            .Where(carrinho => carrinho.CarrinhoCompraId == CarrinhoCompraId)
            .Include(c => c.Produto)
            .ToListAsync();

        _context.CarrinhoCompraItens.RemoveRange(carrinhoItens);
        await _context.SaveChangesAsync();
    }

    public async Task LimparCarrinhoPosPedido()
    {
        var carrinhoItens = _context.CarrinhoCompraItens
            .Where(carrinho => carrinho.CarrinhoCompraId == CarrinhoCompraId);

        _context.CarrinhoCompraItens.RemoveRange(carrinhoItens);
        await _context.SaveChangesAsync();
    }

    public async Task VerificarEAtualizarCarrinho()
    {
        await LimparItensExpirados();
    }


    public async Task LimparItensExpirados()
    {
        // Define o tempo de permanência dos itens (por exemplo, 24 horas)
        var tempoDePermanencia = TimeSpan.FromHours(4);
        var dataLimite = DateTime.Now - tempoDePermanencia;

        // Busca os itens expirados
        var itensExpirados = await _context.CarrinhoCompraItens
        .Include(c => c.Produto) // Inclui a entidade Produto para incrementar a quantidade disponível
        .Where(c => c.CreatedAt < dataLimite && c.CarrinhoCompraId == CarrinhoCompraId)
        .ToListAsync();

        // Remove os itens expirados
        _context.CarrinhoCompraItens.RemoveRange(itensExpirados);

        // Salva as alterações no banco de dados
        await _context.SaveChangesAsync();
    }


    public async Task<decimal> GetCarrinhoCompraTotal()
    {
        var total = await _context.CarrinhoCompraItens
            .Where(c => c.CarrinhoCompraId == CarrinhoCompraId)
            .Select(c => c.Produto.Preco * c.Quantidade)
            .SumAsync();

        return Math.Round(total, 2);
    }
}

