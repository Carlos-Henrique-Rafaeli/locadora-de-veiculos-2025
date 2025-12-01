using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LocadoraDeVeiculos.Infraestrutura.Orm.Migrations
{
    /// <inheritdoc />
    public partial class Add_ConfigPreco : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TBConfiguracaoPreco",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Gasolina = table.Column<decimal>(type: "decimal(18,2)", nullable: false, defaultValue: 6.99m),
                    Etanol = table.Column<decimal>(type: "decimal(18,2)", nullable: false, defaultValue: 4.99m),
                    Diesel = table.Column<decimal>(type: "decimal(18,2)", nullable: false, defaultValue: 5.99m),
                    UsuarioId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TBConfiguracaoPreco", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TBConfiguracaoPreco_AspNetUsers_UsuarioId",
                        column: x => x.UsuarioId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_TBConfiguracaoPreco_UsuarioId",
                table: "TBConfiguracaoPreco",
                column: "UsuarioId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TBConfiguracaoPreco");
        }
    }
}
