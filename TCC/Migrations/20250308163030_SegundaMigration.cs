using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TCC.Migrations
{
    /// <inheritdoc />
    public partial class SegundaMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "TB_Categoria",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Descricao = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TB_Categoria", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "TB_Configuracao",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    TempoParaCancelamento = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TB_Configuracao", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "TB_Usuario",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Nome = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Email = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Perfil = table.Column<int>(type: "int", nullable: false),
                    Senha = table.Column<string>(type: "varchar(250)", maxLength: 250, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TB_Usuario", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "TB_Produto",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Descricao = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CategoriaId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Tempo = table.Column<int>(type: "int", nullable: false),
                    Preco = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TB_Produto", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TB_Produto_TB_Categoria_CategoriaId",
                        column: x => x.CategoriaId,
                        principalTable: "TB_Categoria",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "TB_Agendamento",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Usuarioid = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    DataHoraInicial = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    DataHoraFinal = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TB_Agendamento", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TB_Agendamento_TB_Usuario_Usuarioid",
                        column: x => x.Usuarioid,
                        principalTable: "TB_Usuario",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "TB_ProdutoImagem",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    ProdutoId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Imagem = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: false)
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

            migrationBuilder.CreateTable(
                name: "TB_AgendamentoProduto",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    AgendamentoId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    ProdutoId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TB_AgendamentoProduto", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TB_AgendamentoProduto_TB_Agendamento_AgendamentoId",
                        column: x => x.AgendamentoId,
                        principalTable: "TB_Agendamento",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TB_AgendamentoProduto_TB_Produto_ProdutoId",
                        column: x => x.ProdutoId,
                        principalTable: "TB_Produto",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_TB_Agendamento_Usuarioid",
                table: "TB_Agendamento",
                column: "Usuarioid");

            migrationBuilder.CreateIndex(
                name: "IX_TB_AgendamentoProduto_AgendamentoId",
                table: "TB_AgendamentoProduto",
                column: "AgendamentoId");

            migrationBuilder.CreateIndex(
                name: "IX_TB_AgendamentoProduto_ProdutoId",
                table: "TB_AgendamentoProduto",
                column: "ProdutoId");

            migrationBuilder.CreateIndex(
                name: "IX_TB_Produto_CategoriaId",
                table: "TB_Produto",
                column: "CategoriaId");

            migrationBuilder.CreateIndex(
                name: "IX_TB_ProdutoImagem_ProdutoId",
                table: "TB_ProdutoImagem",
                column: "ProdutoId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TB_AgendamentoProduto");

            migrationBuilder.DropTable(
                name: "TB_Configuracao");

            migrationBuilder.DropTable(
                name: "TB_ProdutoImagem");

            migrationBuilder.DropTable(
                name: "TB_Agendamento");

            migrationBuilder.DropTable(
                name: "TB_Produto");

            migrationBuilder.DropTable(
                name: "TB_Usuario");

            migrationBuilder.DropTable(
                name: "TB_Categoria");
        }
    }
}
