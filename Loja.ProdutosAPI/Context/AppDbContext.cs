﻿using Loja.ProdutosAPI.Models;

using Microsoft.EntityFrameworkCore;

namespace Loja.ProdutosAPI.Context;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Product> Produtos { get; set; }
}
