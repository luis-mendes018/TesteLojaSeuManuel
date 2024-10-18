using AutoMapper;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using Newtonsoft.Json;

using ProdutosAPI.DTOs.ProdutosDTO;
using ProdutosAPI.Models;
using ProdutosAPI.Repositories.Interfaces;

using X.PagedList;

namespace ProdutosAPI.Admin;

[Route("api/v1/[controller]")]
[ApiController]
[Authorize]
public class AdminProdutosController : ControllerBase
{
    private readonly IUnitOfWork _uof;
    private readonly IMapper _mapper;

    public AdminProdutosController(IUnitOfWork uof, IMapper mapper)
    {
        _uof = uof;
        _mapper = mapper;
    }


    [HttpGet("AdminGetProdutos")]
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
    [HttpGet("AdminGetProdutos/{id}", Name = "AdminObterProduto")]
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

    [HttpGet("AdminProdutoNome/{nome}")]
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


    [HttpPost("AdminAdicionarProduto")]
    public async Task<ActionResult> Post([FromBody] CriarProdutoDTO criarProdutoDto)
    {
        var produto = _mapper.Map<Produto>(criarProdutoDto);

        _uof.ProdutoRepository.Add(produto);
        await _uof.Commit();

        var produtoDTO = _mapper.Map<ProdutoDTO>(produto);

        return new CreatedAtRouteResult("AdminObterProduto",
            new { id = produto.Id }, produtoDTO);
    }


    // api/produtos/1
    [HttpPut("AdminEditarProduto/{id}")]
    public async Task<ActionResult> Put(int id, [FromBody] AtualizarProdutoDTO atualizarProdutoDto)
    {
        
        var produto = await _uof.ProdutoRepository.GetById(p => p.Id == id);

        if (produto == null)
        {
            return NotFound("Produto não encontrado");
        }


        _mapper.Map(atualizarProdutoDto, produto);


        _uof.ProdutoRepository.Update(produto);


        await _uof.Commit();

        return Ok(produto);
    }

    //api/produtos/1
    [HttpDelete("AdminDeletarProduto/{id}")]
    public async Task<ActionResult<ProdutoDTO>> Delete(int id)
    {
        var produto = await _uof.ProdutoRepository.GetById(p => p.Id == id);

        if (produto == null)
        {
            return NotFound();
        }

        _uof.ProdutoRepository.Delete(produto);
        await _uof.Commit();

        var produtoDto = _mapper.Map<ProdutoDTO>(produto);

        return Ok(produtoDto);
    }
}
