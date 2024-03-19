using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ParqueDiversao.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class MinhaPrimeiraMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Setores",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nome = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    QtdAtracoes = table.Column<int>(type: "int", nullable: true),
                    LucroTotal = table.Column<decimal>(type: "decimal(18,5)", precision: 18, scale: 5, nullable: true),
                    QtdAtracoesQuebradas = table.Column<int>(type: "int", nullable: true),
                    QtdAtracoesAtivas = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Setores", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Atracoes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nome = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Ativa = table.Column<bool>(type: "bit", nullable: false),
                    Preco = table.Column<decimal>(type: "decimal(18,5)", precision: 18, scale: 5, nullable: false),
                    UltimaManutencao = table.Column<DateTime>(type: "datetime2", nullable: true),
                    SetorId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Atracoes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Atracoes_Setores_SetorId",
                        column: x => x.SetorId,
                        principalTable: "Setores",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Barraquinhas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nome = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ValorObtido = table.Column<decimal>(type: "decimal(18,5)", precision: 18, scale: 5, nullable: true),
                    Custo = table.Column<decimal>(type: "decimal(18,5)", precision: 18, scale: 5, nullable: true),
                    SetorId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Barraquinhas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Barraquinhas_Setores_SetorId",
                        column: x => x.SetorId,
                        principalTable: "Setores",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "InfosAtracao",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EntradasVendidas = table.Column<int>(type: "int", nullable: true),
                    ValorObtido = table.Column<decimal>(type: "decimal(18,5)", precision: 18, scale: 5, nullable: true),
                    Custo = table.Column<decimal>(type: "decimal(18,5)", precision: 18, scale: 5, nullable: true),
                    AtracaoId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InfosAtracao", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InfosAtracao_Atracoes_AtracaoId",
                        column: x => x.AtracaoId,
                        principalTable: "Atracoes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Atracoes_SetorId",
                table: "Atracoes",
                column: "SetorId");

            migrationBuilder.CreateIndex(
                name: "IX_Barraquinhas_SetorId",
                table: "Barraquinhas",
                column: "SetorId");

            migrationBuilder.CreateIndex(
                name: "IX_InfosAtracao_AtracaoId",
                table: "InfosAtracao",
                column: "AtracaoId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Barraquinhas");

            migrationBuilder.DropTable(
                name: "InfosAtracao");

            migrationBuilder.DropTable(
                name: "Atracoes");

            migrationBuilder.DropTable(
                name: "Setores");
        }
    }
}
