using System.Security.Claims;
using AutoMapper;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using ProdutosAPI.Context;
using ProdutosAPI.DTOs.PedidosDTO;
using ProdutosAPI.Models;
using ProdutosAPI.Repositories.Interfaces;

namespace ProdutosAPI.Controllers;

[Route("api/v1/[controller]")]
[ApiController]
public class PedidosController : ControllerBase
{
    private readonly IUnitOfWork _uof;
    private readonly IMapper _mapper;
    private readonly AppDbContext _context;

    public PedidosController(IUnitOfWork uof, IMapper mapper, AppDbContext context)
    {
        _uof = uof;
        _mapper = mapper;
        _context = context;
    }

    [Authorize(AuthenticationSchemes = "Bearer")]
    [HttpPost("processarPedido")]
    public async Task<ActionResult> Post([FromBody] CriarPedidoDTO pedidoDto)
    {
        int totalItensPedido = 0;
        decimal pedidoTotal = 0.0m;

        if (pedidoDto == null)
        {
            return BadRequest("Pedido inválido.");
        }

        // Obter o usuário autenticado
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (string.IsNullOrEmpty(userId))
        {
            return Unauthorized("Usuário não autenticado.");
        }

        // Criar a instância do carrinho de compras com o userId
        var carrinhoCompra = await CarrinhoCompra.CreateAsync(_context, userId);
        var itens = await carrinhoCompra.GetCarrinhoCompraItens();

        if (itens.Count == 0)
        {
            return BadRequest("Ops! Seu carrinho está vazio! Que tal incluir um produto?");
        }

        foreach (var item in itens)
        {
            // Busca o produto do banco de dados
            var produto = await _uof.ProdutoRepository.GetProdutoById(item.Produto.Id);

            if (produto == null)
            {
                return BadRequest($"Produto {item.Produto.Nome} não encontrado.");
            }

            totalItensPedido += item.Quantidade;
            pedidoTotal += item.Produto.Preco * item.Quantidade;
        }

        var pedido = _mapper.Map<Pedido>(pedidoDto);
        pedido.PedidoItens = _mapper.Map<List<PedidoDetalhe>>(itens);
        pedido.PedidoTotal = pedidoTotal;
        pedido.TotalItensPedido = totalItensPedido;
        pedido.PedidoEnviado = DateTime.Now;
        pedido.UsuarioId = userId;

        try
        {
            _uof.PedidoRepository.Add(pedido);

            // Salvando as alterações no contexto
            await _uof.Commit();

            // Limpa o carrinho após o pedido ser processado
            await carrinhoCompra.LimparCarrinhoPosPedido();

            var pedidoCriadoDTO = _mapper.Map<PedidoDTO>(pedido);
            return Ok(pedidoCriadoDTO);
        }
        catch (Exception ex)
        {
            var baseException = ex.GetBaseException();
            return StatusCode(500, "Ocorreu um erro ao criar o pedido: " + baseException.Message);
        }
    }

}
