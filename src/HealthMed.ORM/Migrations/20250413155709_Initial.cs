using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HealthMed.ORM.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Administradores",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Nome = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    CriadoEm = table.Column<DateTime>(type: "datetime2(7)", nullable: false),
                    AtualizadoEm = table.Column<DateTime>(type: "datetime2(7)", nullable: true),
                    Apagado = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Administradores", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Especialidades",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Nome = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    CriadoEm = table.Column<DateTime>(type: "datetime2(7)", nullable: false),
                    AtualizadoEm = table.Column<DateTime>(type: "datetime2(7)", nullable: true),
                    Apagado = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Especialidades", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Usuarios",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Senha = table.Column<string>(type: "varchar(max)", nullable: false),
                    LoginLiberado = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    TipoUsuario = table.Column<int>(type: "int", nullable: false),
                    CriadoEm = table.Column<DateTime>(type: "datetime2(7)", nullable: false),
                    AtualizadoEm = table.Column<DateTime>(type: "datetime2(7)", nullable: true),
                    Apagado = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Usuarios", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Medicos",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Nome = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Crm = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ValorConsulta = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    EspecialidadeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UsuarioId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CriadoEm = table.Column<DateTime>(type: "datetime2(7)", nullable: false),
                    AtualizadoEm = table.Column<DateTime>(type: "datetime2(7)", nullable: true),
                    Apagado = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Medicos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Medicos_Especialidades_EspecialidadeId",
                        column: x => x.EspecialidadeId,
                        principalTable: "Especialidades",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Medicos_Usuarios_UsuarioId",
                        column: x => x.UsuarioId,
                        principalTable: "Usuarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Pacientes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Nome = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Cpf = table.Column<string>(type: "nvarchar(11)", maxLength: 11, nullable: false),
                    UsuarioId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CriadoEm = table.Column<DateTime>(type: "datetime2(7)", nullable: false),
                    AtualizadoEm = table.Column<DateTime>(type: "datetime2(7)", nullable: true),
                    Apagado = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pacientes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Pacientes_Usuarios_UsuarioId",
                        column: x => x.UsuarioId,
                        principalTable: "Usuarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DisponibilidadeMedico",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MedicoId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DiaSemana = table.Column<int>(type: "int", nullable: false),
                    HoraInicio = table.Column<int>(type: "int", nullable: false),
                    HoraFim = table.Column<int>(type: "int", nullable: false),
                    CriadoEm = table.Column<DateTime>(type: "datetime2(7)", nullable: false),
                    AtualizadoEm = table.Column<DateTime>(type: "datetime2(7)", nullable: true),
                    Apagado = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DisponibilidadeMedico", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DisponibilidadeMedico_Medicos_MedicoId",
                        column: x => x.MedicoId,
                        principalTable: "Medicos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Consultas",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PacienteId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MedicoId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Horario = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    JustificativaCancelamento = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Valor = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    CriadoEm = table.Column<DateTime>(type: "datetime2(7)", nullable: false),
                    AtualizadoEm = table.Column<DateTime>(type: "datetime2(7)", nullable: true),
                    Apagado = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Consultas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Consultas_Medicos_MedicoId",
                        column: x => x.MedicoId,
                        principalTable: "Medicos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Consultas_Pacientes_PacienteId",
                        column: x => x.PacienteId,
                        principalTable: "Pacientes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Administrador_Email",
                table: "Administradores",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Consultas_MedicoId",
                table: "Consultas",
                column: "MedicoId");

            migrationBuilder.CreateIndex(
                name: "IX_Consultas_PacienteId",
                table: "Consultas",
                column: "PacienteId");

            migrationBuilder.CreateIndex(
                name: "IX_DisponibilidadeMedico_MedicoId",
                table: "DisponibilidadeMedico",
                column: "MedicoId");

            migrationBuilder.CreateIndex(
                name: "IX_Especialidade_Nome",
                table: "Especialidades",
                column: "Nome");

            migrationBuilder.CreateIndex(
                name: "IX_Medico_Crm",
                table: "Medicos",
                column: "Crm",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Medico_Email",
                table: "Medicos",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Medico_EspecialidadeId",
                table: "Medicos",
                column: "EspecialidadeId");

            migrationBuilder.CreateIndex(
                name: "IX_Medico_Nome",
                table: "Medicos",
                column: "Nome");

            migrationBuilder.CreateIndex(
                name: "IX_Medicos_UsuarioId",
                table: "Medicos",
                column: "UsuarioId");

            migrationBuilder.CreateIndex(
                name: "IX_Paciente_Cpf",
                table: "Pacientes",
                column: "Cpf",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Paciente_Email",
                table: "Pacientes",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Pacientes_UsuarioId",
                table: "Pacientes",
                column: "UsuarioId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Administradores");

            migrationBuilder.DropTable(
                name: "Consultas");

            migrationBuilder.DropTable(
                name: "DisponibilidadeMedico");

            migrationBuilder.DropTable(
                name: "Pacientes");

            migrationBuilder.DropTable(
                name: "Medicos");

            migrationBuilder.DropTable(
                name: "Especialidades");

            migrationBuilder.DropTable(
                name: "Usuarios");
        }
    }
}
