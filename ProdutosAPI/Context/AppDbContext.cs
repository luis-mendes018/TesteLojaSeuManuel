using Microsoft.EntityFrameworkCore;

using ProdutosAPI.Models;

namespace ProdutosAPI.Context;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {

    }

    public DbSet<Produto> Produtos { get; set; }

    public DbSet<Dimensoes> Dimensoes { get; set; }

    public DbSet<CarrinhoCompraItem> CarrinhoCompraItens { get; set; }
    public DbSet<Pedido> Pedidos { get; set; }
    public DbSet<PedidoDetalhe> PedidosDetalhes { get; set; }
    public DbSet<Caixa> Caixas { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Caixa>().HasData(
            new Caixa { Id = 1, Altura = 30, Largura = 40, Comprimento = 80 },
            new Caixa { Id = 2, Altura = 80, Largura = 50, Comprimento = 40 },
            new Caixa { Id = 3, Altura = 50, Largura = 80, Comprimento = 60 }
        );

        // Configurações para a entidade Caixa
        modelBuilder.Entity<Caixa>(entity =>
        {
            // Torna os campos Altura, Largura e Comprimento obrigatórios
            entity.Property(c => c.Altura)
                .IsRequired()
                .HasDefaultValue(1.00m)
                .HasConversion<double>(); // Converte para double

            entity.Property(c => c.Largura)
                .IsRequired()
                .HasDefaultValue(1.00m)
                .HasConversion<double>(); // Converte para double

            entity.Property(c => c.Comprimento)
                .IsRequired()
                .HasDefaultValue(1.00m)
                .HasConversion<double>(); // Converte para double
        });

        // Configurações para a entidade Produto
        modelBuilder.Entity<Produto>(entity =>
        {
            // Definindo o tamanho máximo da string Nome
            entity.Property(p => p.Nome)
                .IsRequired() // Torna o campo obrigatório
                .HasMaxLength(200); // Define o tamanho máximo

            // Define o valor mínimo para Preco
            entity.Property(p => p.Preco)
                .IsRequired() // Campo obrigatório
                .HasDefaultValue(1.00m)
                .HasConversion<double>(); // Converte para double

            // Configuração para chave estrangeira
            entity.HasOne(p => p.Dimensoes)
                .WithMany()
                .HasForeignKey(p => p.DimensoesId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        // Configurações para a entidade Dimensoes
        modelBuilder.Entity<Dimensoes>(entity =>
        {
            // Define o valor mínimo para Altura, Largura e Comprimento
            entity.Property(d => d.Altura)
                .IsRequired()
                .HasDefaultValue(1.00m)
                .HasConversion<double>();

            entity.Property(d => d.Largura)
                .IsRequired()
                .HasDefaultValue(1.00m)
                .HasConversion<double>();

            entity.Property(d => d.Comprimento)
                .IsRequired()
                .HasDefaultValue(1.00m)
                .HasConversion<double>();
        });
    }

}
