using AutoMapper;

using ProdutosAPI.DTOs.DimensoesDTO;
using ProdutosAPI.DTOs.PedidosDTO;
using ProdutosAPI.DTOs.ProdutosDTO;
using ProdutosAPI.Models;

namespace ProdutosAPI.DTOs.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {

        CreateMap<CarrinhoCompraItem, CarrinhoCompraDTO>()
            .ForMember(dest => dest.Produto, opt => opt.MapFrom(src => src.Produto))
            .ReverseMap();

        CreateMap<CarrinhoCompraItem, PedidoDetalhe>()
          .ForMember(dest => dest.Preco, opt => opt.MapFrom(src => src.Produto.Preco))
          .ForMember(dest => dest.ProdutoId, opt => opt.MapFrom(src => src.Produto.Id))
          .ForMember(dest => dest.Quantidade, opt => opt.MapFrom(src => src.Quantidade));

        
        CreateMap<Pedido, PedidoDTO>()
      .ForMember(dest => dest.PedidoEnviado, opt => opt.MapFrom(src => src.PedidoEnviado.ToString("dd/MM/yyyy HH:mm")))
       .ForMember(dest => dest.Caixas, opt => opt.MapFrom(src =>
        src.PedidoItens
            .Where(pi => pi.Produto.Dimensoes != null) // Filtra produtos que têm dimensões válidas
            .GroupBy(pi => GetCaixaIdForProduto(pi.Produto)) // Agrupa pelos IDs das caixas
            .Where(g => g.Key != 0) // Ignora grupos sem caixa (ID 0)
            .Select(g => new CaixaResponseDTO
            {
                CaixaId = "Caixa " + g.Key,
                Produtos = g.Select(pi => pi.Produto.Nome).ToList() // Obtém os nomes dos produtos
            })
            .ToList()))
       .ForMember(dest => dest.Nome, opt => opt.MapFrom(src => src.Nome)) // Mapeia o Nome
       .ForMember(dest => dest.TotalItensPedido, opt => opt.MapFrom(src => src.PedidoItens.Count)) // Total de itens no pedido
       .ForMember(dest => dest.PedidoTotal, opt => opt.MapFrom(src => src.PedidoTotal)) // Mapeia o total do pedido
       .ReverseMap();




        CreateMap<PedidoDetalhe, PedidoDetalheDTO>()
            .ForMember(dest => dest.Produto, opt => opt.MapFrom(src => src.Produto));

        CreateMap<Pedido, CriarPedidoDTO>().ReverseMap();

        CreateMap<Produto, ProdutoDTO>()
            .ForMember(dest => dest.Dimensoes, opt => opt.MapFrom(src => src.Dimensoes))
            .ReverseMap();

        CreateMap<Produto, CriarProdutoDTO>()
            .ForMember(dest => dest.Dimensoes, opt => opt.MapFrom(src => src.Dimensoes))
            .ReverseMap();


        CreateMap<Produto, AtualizarProdutoDTO>()
            .ForMember(dest => dest.Dimensoes, opt => opt.MapFrom(src => src.Dimensoes))
            .ReverseMap();

        CreateMap<Dimensoes, DimensaoDTO>().ReverseMap();

        CreateMap<Caixa, CaixaResponseDTO>().ReverseMap();
    }


    private int GetCaixaIdForProduto(Produto produto)
    {
        if (produto.Dimensoes == null)
        {
            throw new ArgumentException("As dimensões do produto não podem ser nulas.");
        }

        // Lógica para determinar a caixa com base nas dimensões
        if (produto.Dimensoes.Altura <= 30 && produto.Dimensoes.Largura <= 40 && produto.Dimensoes.Comprimento <= 80)
            return 1; // Caixa 1

        else if (produto.Dimensoes.Altura <= 80 && produto.Dimensoes.Largura <= 50 && produto.Dimensoes.Comprimento <= 40)
            return 2; // Caixa 2

        else if (produto.Dimensoes.Altura <= 50 && produto.Dimensoes.Largura <= 80 && produto.Dimensoes.Comprimento <= 60)
            return 3; // Caixa 3

        // Retornar 0 ou outra lógica para produtos que não se encaixam em nenhuma caixa
        return 0; // Nenhuma caixa disponível
    }




}
