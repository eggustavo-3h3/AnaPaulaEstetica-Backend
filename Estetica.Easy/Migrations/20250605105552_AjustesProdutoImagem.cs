using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Estetica.Easy.Migrations
{
    public partial class AjustesProdutoImagem : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {

            // Adiciona a foreign key ProdutoId -> TB_Produto(Id)
            migrationBuilder.AddForeignKey(
                name: "FK_TB_ProdutoImagem_TB_Produto_ProdutoId",
                table: "TB_ProdutoImagem",
                column: "ProdutoId",
                principalTable: "TB_Produto",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Remove foreign key criada no Up
            migrationBuilder.DropForeignKey(
                name: "FK_TB_ProdutoImagem_TB_Produto_ProdutoId",
                table: "TB_ProdutoImagem");

            // Remove índice criado no Up
            migrationBuilder.DropIndex(
                name: "IX_TB_ProdutoImagem_ProdutoId",
                table: "TB_ProdutoImagem");
        }
    }
}
