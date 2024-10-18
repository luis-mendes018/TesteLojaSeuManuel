using Microsoft.EntityFrameworkCore;
using ProdutosAPI.Context;
using ProdutosAPI.Models;

namespace ProdutosAPI.teste;

public class AppDbContextTests : IDisposable
{
    private readonly AppDbContext _context;

    public AppDbContextTests()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
     .UseMySql("Server=localhost;Database=CatalogoProdutosDB;User=root;Password=lf2788960;",
                ServerVersion.AutoDetect("Server=localhost;Database=CatalogoProdutosDB;User=root;Password=lf2788960;"),
                options => options.EnableRetryOnFailure())
     .Options;


        _context = new AppDbContext(options);
        _context.Database.EnsureCreated();
    }

    [Fact]
    public void CanInsertProductIntoDatabase()
    {
        var product = new Produto
        {
            Nome = "Produto Teste",
            Preco = 9.99M
        };

        _context.Produtos.Add(product);
        _context.SaveChanges();

        Assert.Equal(1, _context.Produtos.Count(p => p.Nome == "Produto Teste"));
    }

    public void Dispose()
    {
        _context.Database.EnsureDeleted();
        _context.Dispose();
    }
}