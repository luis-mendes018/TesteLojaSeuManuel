using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ProdutosAPI.Migrations
{
    /// <inheritdoc />
    public partial class Adicionandoinformacoesnatabelacaixa : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Caixas",
                columns: new[] { "Id", "Altura", "Comprimento", "Largura" },
                values: new object[,]
                {
                    { 1, 30.0, 80.0, 40.0 },
                    { 2, 80.0, 40.0, 50.0 },
                    { 3, 50.0, 60.0, 80.0 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Caixas",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Caixas",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Caixas",
                keyColumn: "Id",
                keyValue: 3);
        }
    }
}
