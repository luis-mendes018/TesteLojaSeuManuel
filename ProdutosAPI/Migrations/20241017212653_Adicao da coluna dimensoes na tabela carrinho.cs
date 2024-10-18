using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProdutosAPI.Migrations
{
    /// <inheritdoc />
    public partial class Adicaodacolunadimensoesnatabelacarrinho : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DimensoesId",
                table: "CarrinhoCompraItens",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_CarrinhoCompraItens_DimensoesId",
                table: "CarrinhoCompraItens",
                column: "DimensoesId");

            migrationBuilder.AddForeignKey(
                name: "FK_CarrinhoCompraItens_Dimensoes_DimensoesId",
                table: "CarrinhoCompraItens",
                column: "DimensoesId",
                principalTable: "Dimensoes",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CarrinhoCompraItens_Dimensoes_DimensoesId",
                table: "CarrinhoCompraItens");

            migrationBuilder.DropIndex(
                name: "IX_CarrinhoCompraItens_DimensoesId",
                table: "CarrinhoCompraItens");

            migrationBuilder.DropColumn(
                name: "DimensoesId",
                table: "CarrinhoCompraItens");
        }
    }
}
