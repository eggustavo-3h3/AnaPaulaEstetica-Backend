using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Estetica.Easy.Migrations
{
    /// <inheritdoc />
    public partial class AjusteProdutoImagem : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Imagem",
                table: "TB_Produto",
                type: "longtext",
                nullable: false,
                defaultValue: "")
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Imagem",
                table: "TB_Produto",
                newName: "ProdutoImagens");

            migrationBuilder.CreateTable(
                name: "TB_ProdutoImagem",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    ProdutoId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Imagem = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TB_ProdutoImagem", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TB_ProdutoImagem_TB_Produto_ProdutoId",
                        column: x => x.ProdutoId,
                        principalTable: "TB_Produto",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_TB_ProdutoImagem_ProdutoId",
                table: "TB_ProdutoImagem",
                column: "ProdutoId",
                unique: true);
        }
    }
}
