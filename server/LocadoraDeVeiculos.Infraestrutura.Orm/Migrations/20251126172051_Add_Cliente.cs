using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LocadoraDeVeiculos.Infraestrutura.Orm.Migrations
{
    /// <inheritdoc />
    public partial class Add_Cliente : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TBPessoaJuridica",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Cnpj = table.Column<string>(type: "nvarchar(18)", maxLength: 18, nullable: false),
                    CondutorId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UsuarioId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Nome = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Telefone = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: false),
                    Endereco = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TBPessoaJuridica", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TBPessoaJuridica_AspNetUsers_UsuarioId",
                        column: x => x.UsuarioId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_TBPessoaJuridica_TBCondutor_CondutorId",
                        column: x => x.CondutorId,
                        principalTable: "TBCondutor",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TBPessoaFisica",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Cpf = table.Column<string>(type: "nvarchar(14)", maxLength: 14, nullable: false),
                    Rg = table.Column<string>(type: "nvarchar(12)", maxLength: 12, nullable: false),
                    Cnh = table.Column<string>(type: "nvarchar(11)", maxLength: 11, nullable: false),
                    PessoaJuridicaId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UsuarioId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Nome = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Telefone = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: false),
                    Endereco = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TBPessoaFisica", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TBPessoaFisica_AspNetUsers_UsuarioId",
                        column: x => x.UsuarioId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_TBPessoaFisica_TBPessoaJuridica_PessoaJuridicaId",
                        column: x => x.PessoaJuridicaId,
                        principalTable: "TBPessoaJuridica",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_TBPessoaFisica_PessoaJuridicaId",
                table: "TBPessoaFisica",
                column: "PessoaJuridicaId",
                unique: true,
                filter: "[PessoaJuridicaId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_TBPessoaFisica_UsuarioId",
                table: "TBPessoaFisica",
                column: "UsuarioId");

            migrationBuilder.CreateIndex(
                name: "IX_TBPessoaJuridica_CondutorId",
                table: "TBPessoaJuridica",
                column: "CondutorId");

            migrationBuilder.CreateIndex(
                name: "IX_TBPessoaJuridica_UsuarioId",
                table: "TBPessoaJuridica",
                column: "UsuarioId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TBPessoaFisica");

            migrationBuilder.DropTable(
                name: "TBPessoaJuridica");
        }
    }
}
