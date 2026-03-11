using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CareBridge.Migrations
{
    /// <inheritdoc />
    public class AddPendingPaymentsAndDentistProcedures : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PendingPayments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    InvoiceId = table.Column<int>(type: "int", nullable: false),
                    PatientId = table.Column<int>(type: "int", nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    DueDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Status = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PaidAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PendingPayments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PendingPayments_Invoices_InvoiceId",
                        column: x => x.InvoiceId,
                        principalTable: "Invoices",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PendingPayments_Patients_PatientId",
                        column: x => x.PatientId,
                        principalTable: "Patients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "DentistProcedures",
                columns: table => new
                {
                    DentistId = table.Column<int>(type: "int", nullable: false),
                    ProcedureId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DentistProcedures", x => new { x.DentistId, x.ProcedureId });
                    table.ForeignKey(
                        name: "FK_DentistProcedures_Dentists_DentistId",
                        column: x => x.DentistId,
                        principalTable: "Dentists",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DentistProcedures_Procedures_ProcedureId",
                        column: x => x.ProcedureId,
                        principalTable: "Procedures",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(name: "IX_PendingPayments_InvoiceId", table: "PendingPayments", column: "InvoiceId");
            migrationBuilder.CreateIndex(name: "IX_PendingPayments_PatientId", table: "PendingPayments", column: "PatientId");
            migrationBuilder.CreateIndex(name: "IX_PendingPayments_Status", table: "PendingPayments", column: "Status");
            migrationBuilder.CreateIndex(name: "IX_DentistProcedures_ProcedureId", table: "DentistProcedures", column: "ProcedureId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(name: "PendingPayments");
            migrationBuilder.DropTable(name: "DentistProcedures");
        }
    }
}
