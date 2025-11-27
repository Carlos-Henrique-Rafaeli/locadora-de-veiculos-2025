using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LocadoraDeVeiculos.Infraestrutura.Orm.Migrations
{
    /// <inheritdoc />
    public partial class Add_PlanoCobranca : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TBPlanoCobranca",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TipoPlano = table.Column<int>(type: "int", nullable: false),
                    GrupoVeiculoId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ValorDiario = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    ValorKm = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    KmIncluso = table.Column<int>(type: "int", nullable: true),
                    ValorKmExcedente = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    ValorFixo = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    UsuarioId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TBPlanoCobranca", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TBPlanoCobranca_AspNetUsers_UsuarioId",
                        column: x => x.UsuarioId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_TBPlanoCobranca_TBGrupoVeiculos_GrupoVeiculoId",
                        column: x => x.GrupoVeiculoId,
                        principalTable: "TBGrupoVeiculos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TBPlanoCobranca_GrupoVeiculoId",
                table: "TBPlanoCobranca",
                column: "GrupoVeiculoId");

            migrationBuilder.CreateIndex(
                name: "IX_TBPlanoCobranca_UsuarioId",
                table: "TBPlanoCobranca",
                column: "UsuarioId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TBPlanoCobranca");
        }
    }
}
