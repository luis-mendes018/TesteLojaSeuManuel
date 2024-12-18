﻿// <auto-generated />
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
    [Migration("20241016123843_Versao inicial")]
    partial class Versaoinicial
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.10")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            MySqlModelBuilderExtensions.AutoIncrementColumns(modelBuilder);

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

            modelBuilder.Entity("ProdutosAPI.Models.Produto", b =>
                {
                    b.HasOne("ProdutosAPI.Models.Dimensoes", "Dimensoes")
                        .WithMany()
                        .HasForeignKey("DimensoesId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Dimensoes");
                });
#pragma warning restore 612, 618
        }
    }
}
