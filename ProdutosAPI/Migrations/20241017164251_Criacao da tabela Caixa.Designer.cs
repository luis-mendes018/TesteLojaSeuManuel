﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using ProdutosAPI.Context;

#nullable disable

namespace ProdutosAPI.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20241017164251_Criacao da tabela Caixa")]
    partial class CriacaodatabelaCaixa
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.10")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            MySqlModelBuilderExtensions.AutoIncrementColumns(modelBuilder);

            modelBuilder.Entity("ProdutosAPI.Models.Caixa", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<int>("Id"));

                    b.Property<double>("Altura")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("double")
                        .HasDefaultValue(1.0);

                    b.Property<double>("Comprimento")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("double")
                        .HasDefaultValue(1.0);

                    b.Property<double>("Largura")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("double")
                        .HasDefaultValue(1.0);

                    b.HasKey("Id");

                    b.ToTable("Caixas");
                });

            modelBuilder.Entity("ProdutosAPI.Models.CarrinhoCompraItem", b =>
                {
                    b.Property<int>("CarrinhoCompraItemId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<int>("CarrinhoCompraItemId"));

                    b.Property<string>("CarrinhoCompraId")
                        .HasMaxLength(200)
                        .HasColumnType("varchar(200)");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime(6)");

                    b.Property<int>("ProdutoId")
                        .HasColumnType("int");

                    b.Property<int>("Quantidade")
                        .HasColumnType("int");

                    b.HasKey("CarrinhoCompraItemId");

                    b.HasIndex("ProdutoId");

                    b.ToTable("CarrinhoCompraItens");
                });

            modelBuilder.Entity("ProdutosAPI.Models.Dimensoes", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<int>("Id"));

                    b.Property<double>("Altura")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("double")
                        .HasDefaultValue(1.0);

                    b.Property<double>("Comprimento")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("double")
                        .HasDefaultValue(1.0);

                    b.Property<double>("Largura")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("double")
                        .HasDefaultValue(1.0);

                    b.HasKey("Id");

                    b.ToTable("Dimensoes");
                });

            modelBuilder.Entity("ProdutosAPI.Models.Pedido", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Nome")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("varchar(50)");

                    b.Property<DateTime>("PedidoEnviado")
                        .HasColumnType("datetime(6)");

                    b.Property<decimal>("PedidoTotal")
                        .HasColumnType("decimal(18,2)");

                    b.Property<int>("TotalItensPedido")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("Pedidos");
                });

            modelBuilder.Entity("ProdutosAPI.Models.PedidoDetalhe", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("PedidoId")
                        .HasColumnType("int");

                    b.Property<decimal>("Preco")
                        .HasColumnType("decimal(18,2)");

                    b.Property<int>("ProdutoId")
                        .HasColumnType("int");

                    b.Property<int>("Quantidade")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("PedidoId");

                    b.HasIndex("ProdutoId");

                    b.ToTable("PedidosDetalhes");
                });

            modelBuilder.Entity("ProdutosAPI.Models.Produto", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("DimensoesId")
                        .HasColumnType("int");

                    b.Property<string>("Nome")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("varchar(200)");

                    b.Property<double>("Preco")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("double")
                        .HasDefaultValue(1.0);

                    b.HasKey("Id");

                    b.HasIndex("DimensoesId");

                    b.ToTable("Produtos");
                });

            modelBuilder.Entity("ProdutosAPI.Models.CarrinhoCompraItem", b =>
                {
                    b.HasOne("ProdutosAPI.Models.Produto", "Produto")
                        .WithMany()
                        .HasForeignKey("ProdutoId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Produto");
                });

            modelBuilder.Entity("ProdutosAPI.Models.PedidoDetalhe", b =>
                {
                    b.HasOne("ProdutosAPI.Models.Pedido", "Pedido")
                        .WithMany("PedidoItens")
                        .HasForeignKey("PedidoId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ProdutosAPI.Models.Produto", "Produto")
                        .WithMany()
                        .HasForeignKey("ProdutoId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Pedido");

                    b.Navigation("Produto");
                });

            modelBuilder.Entity("ProdutosAPI.Models.Produto", b =>
                {
                    b.HasOne("ProdutosAPI.Models.Dimensoes", "Dimensoes")
                        .WithMany()
                        .HasForeignKey("DimensoesId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Dimensoes");
                });

            modelBuilder.Entity("ProdutosAPI.Models.Pedido", b =>
                {
                    b.Navigation("PedidoItens");
                });
#pragma warning restore 612, 618
        }
    }
}
