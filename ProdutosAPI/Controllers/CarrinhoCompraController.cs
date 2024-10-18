using System.Security.Claims;

using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using ProdutosAPI.Context;
using ProdutosAPI.DTOs;
using ProdutosAPI.Models;

namespace ProdutosAPI.Controllers;

[Route("api/v1/[controller]")]
[ApiController]
[Authorize]
public class CarrinhoCompraController : ControllerBase
{

    private readonly AppDbContext _context;
    private readonly IMapper _mapper;

    public CarrinhoCompraController(AppDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    private async Task<CarrinhoCompra> GetCarrinhoCompraAsync()
    {
        // Obtém o userId do token JWT
        var userId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(userId))
        {
            throw new UnauthorizedAccessException("Usuário não autenticado");
        }

        // Cria uma instância de CarrinhoCompra com base no userId
        var carrinhoCompra = await CarrinhoCompra.CreateAsync(_context, userId);
        await carrinhoCompra.VerificarEAtualizarCarrinho();

        return carrinhoCompra;
    }

    [HttpGet("itens")]
    public async Task<IActionResult> GetItensDoCarrinhoCompra()
    {
        var carrinhoCompra = await GetCarrinhoCompraAsync();
        var itens = await carrinhoCompra.GetCarrinhoCompraItens();
        var itensDTO = _mapper.Map<List<CarrinhoCompraDTO>>(itens);
        var total = await carrinhoCompra.GetCarrinhoCompraTotal();

        return Ok(new { Itens = itensDTO, Total = total });
    }

    [HttpPost("adicionarCarrinho/{produtoId}")]
    public async Task<IActionResult> AdicionarItemAoCarrinho(int produtoId)
    {
        var produto = await _context.Produtos.FirstOrDefaultAsync(p => p.Id == produtoId);
        if (produto == null)
        {
            return NotFound("Produto não encontrado.");
        }

        var carrinhoCompra = await GetCarrinhoCompraAsync();
        await carrinhoCompra.AdicionarAoCarrinho(produto);

        var itens = await carrinhoCompra.GetCarrinhoCompraItens();
        var itensDTO = _mapper.Map<List<CarrinhoCompraDTO>>(itens);
        return Ok(new { Itens = itensDTO, Total = await carrinhoCompra.GetCarrinhoCompraTotal() });
    }

    [HttpDelete("removerCarrinho/{produtoId}")]
    public async Task<IActionResult> RemoverItemDoCarrinho(int produtoId)
    {
        var produto = await _context.Produtos.FirstOrDefaultAsync(p => p.Id == produtoId);
        if (produto == null)
        {
            return NotFound("Produto não encontrado.");
        }

        var carrinhoCompra = await GetCarrinhoCompraAsync();
        carrinhoCompra.RemoverDoCarrinho(produto);

        return Ok();
    }

    [HttpPost("limpar")]
    public async Task<IActionResult> LimparCarrinho()
    {
        var carrinhoCompra = await GetCarrinhoCompraAsync();
        await carrinhoCompra.LimparCarrinho();

        return Ok();
    }

}
