using AutoMapper;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using Newtonsoft.Json;

using ProdutosAPI.DTOs.ProdutosDTO;
using ProdutosAPI.Repositories.Interfaces;

using X.PagedList;

namespace ProdutosAPI.Controllers;

[Route("api/v1/[controller]")]
[ApiController]
public class ProdutosController : ControllerBase
{
    private readonly IUnitOfWork _uof;
    private readonly IMapper _mapper;

    public ProdutosController(IUnitOfWork uof, IMapper mapper)
    {
        _uof = uof;
        _mapper = mapper;
    }


    [HttpGet("GetProdutos")]
    public async Task<ActionResult<IEnumerable<ProdutoDTO>>> Get([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
    {
        var produtos = await _uof.ProdutoRepository.GetProdutos(pageNumber, pageSize);

        var produtosDto = _mapper.Map<List<ProdutoDTO>>(produtos);


        Response.Headers.Append("X-Pagination", JsonConvert.SerializeObject(new
        {
            produtos.TotalItemCount,
            produtos.PageCount,
            produtos.PageNumber,
            produtos.PageSize,
            produtos.HasPreviousPage,
            produtos.HasNextPage
        }));

        return Ok(produtosDto);
    }


    // api/produtos/1
    [HttpGet("GetProdutos/{id:int:min(1)}", Name = "ObterProduto")]
    public async Task<ActionResult<ProdutoDTO>> Get(int id)
    {

        var produto = await _uof.ProdutoRepository.GetProdutoById(id);

        if (produto == null)
        {
            return NotFound("Produto não encontrado!");
        }

        var produtoDto = _mapper.Map<ProdutoDTO>(produto);

        return Ok(produtoDto);
    }

    [HttpGet("ProdutoNome/{nome}")]
    public async Task<ActionResult<IPagedList<ProdutoDTO>>> GetProdutosPorNome(string nome, [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
    {
        var produtos = await _uof.ProdutoRepository.GetProdutosPorNome(nome, pageNumber, pageSize);

        if (produtos == null || !produtos.Any())
        {
            return NotFound("Nenhum produto encontrado com o nome fornecido!");
        }

        Response.Headers.Append("X-Pagination", JsonConvert.SerializeObject(new
        {
            produtos.TotalItemCount,
            produtos.PageCount,
            produtos.PageNumber,
            produtos.PageSize,
            produtos.HasPreviousPage,
            produtos.HasNextPage
        }));

        return Ok(produtos);
    }

}
