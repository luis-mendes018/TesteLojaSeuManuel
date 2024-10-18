using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProdutosAPI.Migrations
{
    /// <inheritdoc />
    public partial class Adicaodacolunacaixasnatebelapedido : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PedidoId",
                table: "Caixas",
                type: "int",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Caixas",
                keyColumn: "Id",
                keyValue: 1,
                column: "PedidoId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Caixas",
                keyColumn: "Id",
                keyValue: 2,
                column: "PedidoId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Caixas",
                keyColumn: "Id",
                keyValue: 3,
                column: "PedidoId",
                value: null);

            migrationBuilder.CreateIndex(
                name: "IX_Caixas_PedidoId",
                table: "Caixas",
                column: "PedidoId");

            migrationBuilder.AddForeignKey(
                name: "FK_Caixas_Pedidos_PedidoId",
                table: "Caixas",
                column: "PedidoId",
                principalTable: "Pedidos",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Caixas_Pedidos_PedidoId",
                table: "Caixas");

            migrationBuilder.DropIndex(
                name: "IX_Caixas_PedidoId",
                table: "Caixas");

            migrationBuilder.DropColumn(
                name: "PedidoId",
                table: "Caixas");
        }
    }
}
