using AutoMapper;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using Newtonsoft.Json;

using ProdutosAPI.DTOs.PedidosDTO;
using ProdutosAPI.Repositories.Interfaces;

using X.PagedList;

namespace ProdutosAPI.Admin;

[Route("api/v1/[controller]")]
[ApiController]
[Authorize]
public class AdminPedidosController : ControllerBase
{
    private readonly IUnitOfWork _uof;
    private readonly IMapper _mapper;

    public AdminPedidosController(IUnitOfWork uof, IMapper mapper)
    {
        _uof = uof;
        _mapper = mapper;
    }

    [HttpGet("GetPedidos")]
    public async Task<ActionResult<IPagedList<PedidoDTO>>> Get([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
    {
        var pedidos = await _uof.PedidoRepository.GetPedidos(pageNumber, pageSize);

        // Mapear os pedidos para DTOs
        var pedidosDTO = _mapper.Map<List<PedidoDTO>>(pedidos);

        // Adicionar a data de envio formatada aos pedidos DTO
        foreach (var pedidoDTO in pedidosDTO)
        {
            pedidoDTO.PedidoEnviado = pedidoDTO.PedidoEnviado;
        }

        Response.Headers.Append("X-Pagination", JsonConvert.SerializeObject(new
        {
            pedidos.TotalItemCount,
            pedidos.PageCount,
            pedidos.PageNumber,
            pedidos.PageSize,
            pedidos.HasPreviousPage,
            pedidos.HasNextPage
        }));

        return Ok(pedidosDTO);
    }


    [HttpGet("AdminGetPedidos/{id:int:min(1)}")]
    public async Task<ActionResult<PedidoDTO>> Get(int id)
    {

        var pedido = await _uof.PedidoRepository.GetPedidoDetalhesByIdAsync(id);

        if (pedido == null)
        {
            return NotFound("Pedido não encontrado");
        }

        var pedidoDto = _mapper.Map<PedidoDTO>(pedido);

        return Ok(pedidoDto);
    }

    [HttpGet("AdminClientePedido/{nomeCliente}")]
    public async Task<ActionResult<IPagedList<PedidoDTO>>> GetPedidosByNomeCliente(string nomeCliente, [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
    {
        var pedidos = await _uof.PedidoRepository.GetPedidosByNomeCliente(nomeCliente, pageNumber, pageSize);

        if (pedidos == null || !pedidos.Any())
        {
            return NotFound($"Nenhum cliente encontrado com o nome '{nomeCliente}'");
        }

        Response.Headers.Append("X-Pagination", JsonConvert.SerializeObject(new
        {
            pedidos.TotalItemCount,
            pedidos.PageCount,
            pedidos.PageNumber,
            pedidos.PageSize,
            pedidos.HasPreviousPage,
            pedidos.HasNextPage
        }));

        return Ok(pedidos);
    }


    [HttpDelete("AdminExcluirPedido/{id}")]
    public async Task<ActionResult<PedidoDTO>> Delete(int id)
    {
        var pedido = await _uof.PedidoRepository.GetById(p => p.Id == id);

        if (pedido == null)
        {
            return NotFound("Pedido não encontrado");
        }

        _uof.PedidoRepository.Delete(pedido);
        await _uof.Commit();

        var pedidoDto = _mapper.Map<PedidoDTO>(pedido);

        return Ok(pedidoDto);
    }

    [HttpPost("AdminExcluirTodosPedidos")]
    public async Task<ActionResult> ExcluirTodosPedidos()
    {
        try
        {
            await _uof.PedidoRepository.DeleteAll();
            await _uof.Commit();
            return NoContent();
        }
        catch (Exception ex)
        {
            return StatusCode(500, "Erro ao excluir todos os pedidos " + ex.Message);
        }
    }


}
