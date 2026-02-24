using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DotNet_Starter_Template.Migrations
{
    /// <inheritdoc />
    public partial class AddPrintersPrintJobsPrintingTemplates : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Printers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    ConnectionInfo = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    PrinterType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Printers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PrintingTemplates",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TemplateType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PrintingTemplates", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PrintJobs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PrinterId = table.Column<int>(type: "int", nullable: false),
                    PrintingTemplateId = table.Column<int>(type: "int", nullable: false),
                    RequestedById = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: true),
                    Status = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    JobData = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Copies = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    StartedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CompletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ErrorMessage = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PrintJobs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PrintJobs_AspNetUsers_RequestedById",
                        column: x => x.RequestedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_PrintJobs_Printers_PrinterId",
                        column: x => x.PrinterId,
                        principalTable: "Printers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PrintJobs_PrintingTemplates_PrintingTemplateId",
                        column: x => x.PrintingTemplateId,
                        principalTable: "PrintingTemplates",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "1",
                column: "CreatedAt",
                value: new DateTime(2026, 2, 23, 21, 23, 56, 8, DateTimeKind.Utc).AddTicks(9490));

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "2",
                column: "CreatedAt",
                value: new DateTime(2026, 2, 23, 21, 23, 56, 8, DateTimeKind.Utc).AddTicks(9550));

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "3",
                column: "CreatedAt",
                value: new DateTime(2026, 2, 23, 21, 23, 56, 8, DateTimeKind.Utc).AddTicks(9560));

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "4",
                column: "CreatedAt",
                value: new DateTime(2026, 2, 23, 21, 23, 56, 8, DateTimeKind.Utc).AddTicks(9560));

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "5",
                column: "CreatedAt",
                value: new DateTime(2026, 2, 23, 21, 23, 56, 8, DateTimeKind.Utc).AddTicks(9570));

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "1",
                columns: new[] { "ConcurrencyStamp", "CreatedAt", "PasswordHash", "SecurityStamp" },
                values: new object[] { "3ddc4f7d-1ef0-4ab4-aaac-47ce6d5e62ed", new DateTime(2026, 2, 23, 21, 23, 56, 8, DateTimeKind.Utc).AddTicks(9660), "AQAAAAIAAYagAAAAEGG2dNGVLIp/MmRH+4X+/V06IJy1f2wrbG/cJxPq14ODexp4ps5Kcakf8QCzqqI3aQ==", "c74cdab2-d48a-47ef-88b2-03b021927e41" });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2026, 2, 23, 21, 23, 56, 8, DateTimeKind.Utc).AddTicks(9320));

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2026, 2, 23, 21, 23, 56, 8, DateTimeKind.Utc).AddTicks(9330));

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2026, 2, 23, 21, 23, 56, 8, DateTimeKind.Utc).AddTicks(9330));

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 4,
                column: "CreatedAt",
                value: new DateTime(2026, 2, 23, 21, 23, 56, 8, DateTimeKind.Utc).AddTicks(9330));

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 5,
                column: "CreatedAt",
                value: new DateTime(2026, 2, 23, 21, 23, 56, 8, DateTimeKind.Utc).AddTicks(9330));

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 6,
                column: "CreatedAt",
                value: new DateTime(2026, 2, 23, 21, 23, 56, 8, DateTimeKind.Utc).AddTicks(9340));

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 7,
                column: "CreatedAt",
                value: new DateTime(2026, 2, 23, 21, 23, 56, 8, DateTimeKind.Utc).AddTicks(9340));

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 8,
                column: "CreatedAt",
                value: new DateTime(2026, 2, 23, 21, 23, 56, 8, DateTimeKind.Utc).AddTicks(9340));

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 9,
                column: "CreatedAt",
                value: new DateTime(2026, 2, 23, 21, 23, 56, 8, DateTimeKind.Utc).AddTicks(9340));

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 10,
                column: "CreatedAt",
                value: new DateTime(2026, 2, 23, 21, 23, 56, 8, DateTimeKind.Utc).AddTicks(9340));

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 11,
                column: "CreatedAt",
                value: new DateTime(2026, 2, 23, 21, 23, 56, 8, DateTimeKind.Utc).AddTicks(9340));

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 12,
                column: "CreatedAt",
                value: new DateTime(2026, 2, 23, 21, 23, 56, 8, DateTimeKind.Utc).AddTicks(9350));

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 13,
                column: "CreatedAt",
                value: new DateTime(2026, 2, 23, 21, 23, 56, 8, DateTimeKind.Utc).AddTicks(9350));

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 14,
                column: "CreatedAt",
                value: new DateTime(2026, 2, 23, 21, 23, 56, 8, DateTimeKind.Utc).AddTicks(9350));

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 15,
                column: "CreatedAt",
                value: new DateTime(2026, 2, 23, 21, 23, 56, 8, DateTimeKind.Utc).AddTicks(9420));

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 16,
                column: "CreatedAt",
                value: new DateTime(2026, 2, 23, 21, 23, 56, 8, DateTimeKind.Utc).AddTicks(9420));

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 17,
                column: "CreatedAt",
                value: new DateTime(2026, 2, 23, 21, 23, 56, 8, DateTimeKind.Utc).AddTicks(9420));

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 18,
                column: "CreatedAt",
                value: new DateTime(2026, 2, 23, 21, 23, 56, 8, DateTimeKind.Utc).AddTicks(9430));

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 19,
                column: "CreatedAt",
                value: new DateTime(2026, 2, 23, 21, 23, 56, 8, DateTimeKind.Utc).AddTicks(9430));

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 20,
                column: "CreatedAt",
                value: new DateTime(2026, 2, 23, 21, 23, 56, 8, DateTimeKind.Utc).AddTicks(9430));

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 21,
                column: "CreatedAt",
                value: new DateTime(2026, 2, 23, 21, 23, 56, 8, DateTimeKind.Utc).AddTicks(9430));

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 22,
                column: "CreatedAt",
                value: new DateTime(2026, 2, 23, 21, 23, 56, 8, DateTimeKind.Utc).AddTicks(9430));

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 23,
                column: "CreatedAt",
                value: new DateTime(2026, 2, 23, 21, 23, 56, 8, DateTimeKind.Utc).AddTicks(9430));

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 24,
                column: "CreatedAt",
                value: new DateTime(2026, 2, 23, 21, 23, 56, 8, DateTimeKind.Utc).AddTicks(9430));

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 25,
                column: "CreatedAt",
                value: new DateTime(2026, 2, 23, 21, 23, 56, 8, DateTimeKind.Utc).AddTicks(9440));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { 1, "1" },
                column: "AssignedAt",
                value: new DateTime(2026, 2, 23, 21, 23, 56, 47, DateTimeKind.Utc).AddTicks(9410));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { 2, "1" },
                column: "AssignedAt",
                value: new DateTime(2026, 2, 23, 21, 23, 56, 47, DateTimeKind.Utc).AddTicks(9420));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { 3, "1" },
                column: "AssignedAt",
                value: new DateTime(2026, 2, 23, 21, 23, 56, 47, DateTimeKind.Utc).AddTicks(9420));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { 4, "1" },
                column: "AssignedAt",
                value: new DateTime(2026, 2, 23, 21, 23, 56, 47, DateTimeKind.Utc).AddTicks(9420));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { 5, "1" },
                column: "AssignedAt",
                value: new DateTime(2026, 2, 23, 21, 23, 56, 47, DateTimeKind.Utc).AddTicks(9420));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { 6, "1" },
                column: "AssignedAt",
                value: new DateTime(2026, 2, 23, 21, 23, 56, 47, DateTimeKind.Utc).AddTicks(9420));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { 7, "1" },
                column: "AssignedAt",
                value: new DateTime(2026, 2, 23, 21, 23, 56, 47, DateTimeKind.Utc).AddTicks(9420));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { 8, "1" },
                column: "AssignedAt",
                value: new DateTime(2026, 2, 23, 21, 23, 56, 47, DateTimeKind.Utc).AddTicks(9420));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { 9, "1" },
                column: "AssignedAt",
                value: new DateTime(2026, 2, 23, 21, 23, 56, 47, DateTimeKind.Utc).AddTicks(9440));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { 10, "1" },
                column: "AssignedAt",
                value: new DateTime(2026, 2, 23, 21, 23, 56, 47, DateTimeKind.Utc).AddTicks(9450));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { 11, "1" },
                column: "AssignedAt",
                value: new DateTime(2026, 2, 23, 21, 23, 56, 47, DateTimeKind.Utc).AddTicks(9450));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { 12, "1" },
                column: "AssignedAt",
                value: new DateTime(2026, 2, 23, 21, 23, 56, 47, DateTimeKind.Utc).AddTicks(9450));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { 13, "1" },
                column: "AssignedAt",
                value: new DateTime(2026, 2, 23, 21, 23, 56, 47, DateTimeKind.Utc).AddTicks(9450));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { 14, "1" },
                column: "AssignedAt",
                value: new DateTime(2026, 2, 23, 21, 23, 56, 47, DateTimeKind.Utc).AddTicks(9450));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { 15, "1" },
                column: "AssignedAt",
                value: new DateTime(2026, 2, 23, 21, 23, 56, 47, DateTimeKind.Utc).AddTicks(9450));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { 16, "1" },
                column: "AssignedAt",
                value: new DateTime(2026, 2, 23, 21, 23, 56, 47, DateTimeKind.Utc).AddTicks(9450));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { 17, "1" },
                column: "AssignedAt",
                value: new DateTime(2026, 2, 23, 21, 23, 56, 47, DateTimeKind.Utc).AddTicks(9450));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { 18, "1" },
                column: "AssignedAt",
                value: new DateTime(2026, 2, 23, 21, 23, 56, 47, DateTimeKind.Utc).AddTicks(9450));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { 19, "1" },
                column: "AssignedAt",
                value: new DateTime(2026, 2, 23, 21, 23, 56, 47, DateTimeKind.Utc).AddTicks(9450));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { 20, "1" },
                column: "AssignedAt",
                value: new DateTime(2026, 2, 23, 21, 23, 56, 47, DateTimeKind.Utc).AddTicks(9450));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { 21, "1" },
                column: "AssignedAt",
                value: new DateTime(2026, 2, 23, 21, 23, 56, 47, DateTimeKind.Utc).AddTicks(9450));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { 22, "1" },
                column: "AssignedAt",
                value: new DateTime(2026, 2, 23, 21, 23, 56, 47, DateTimeKind.Utc).AddTicks(9450));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { 23, "1" },
                column: "AssignedAt",
                value: new DateTime(2026, 2, 23, 21, 23, 56, 47, DateTimeKind.Utc).AddTicks(9450));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { 24, "1" },
                column: "AssignedAt",
                value: new DateTime(2026, 2, 23, 21, 23, 56, 47, DateTimeKind.Utc).AddTicks(9450));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { 25, "1" },
                column: "AssignedAt",
                value: new DateTime(2026, 2, 23, 21, 23, 56, 47, DateTimeKind.Utc).AddTicks(9450));

            migrationBuilder.UpdateData(
                table: "UserRole",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "1", "1" },
                column: "AssignedAt",
                value: new DateTime(2026, 2, 23, 21, 23, 56, 47, DateTimeKind.Utc).AddTicks(9320));

            migrationBuilder.CreateIndex(
                name: "IX_PrintJobs_PrinterId",
                table: "PrintJobs",
                column: "PrinterId");

            migrationBuilder.CreateIndex(
                name: "IX_PrintJobs_PrintingTemplateId",
                table: "PrintJobs",
                column: "PrintingTemplateId");

            migrationBuilder.CreateIndex(
                name: "IX_PrintJobs_RequestedById",
                table: "PrintJobs",
                column: "RequestedById");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PrintJobs");

            migrationBuilder.DropTable(
                name: "Printers");

            migrationBuilder.DropTable(
                name: "PrintingTemplates");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "1",
                column: "CreatedAt",
                value: new DateTime(2025, 10, 29, 23, 0, 10, 247, DateTimeKind.Utc).AddTicks(216));

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "2",
                column: "CreatedAt",
                value: new DateTime(2025, 10, 29, 23, 0, 10, 247, DateTimeKind.Utc).AddTicks(284));

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "3",
                column: "CreatedAt",
                value: new DateTime(2025, 10, 29, 23, 0, 10, 247, DateTimeKind.Utc).AddTicks(288));

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "4",
                column: "CreatedAt",
                value: new DateTime(2025, 10, 29, 23, 0, 10, 247, DateTimeKind.Utc).AddTicks(307));

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "5",
                column: "CreatedAt",
                value: new DateTime(2025, 10, 29, 23, 0, 10, 247, DateTimeKind.Utc).AddTicks(310));

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "1",
                columns: new[] { "ConcurrencyStamp", "CreatedAt", "PasswordHash", "SecurityStamp" },
                values: new object[] { "382dd3bc-083b-4794-9dc8-1663573719af", new DateTime(2025, 10, 29, 23, 0, 10, 247, DateTimeKind.Utc).AddTicks(428), "AQAAAAIAAYagAAAAENVYyWaV5FFa2AZZAc1Q2/c/p1wFkyKaAs12dBBPeTMrKhcMkeqVAhPjugLQfvyc9Q==", "f3dea786-1dea-45fe-9427-19206fadb16f" });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 29, 23, 0, 10, 247, DateTimeKind.Utc).AddTicks(1));

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 29, 23, 0, 10, 247, DateTimeKind.Utc).AddTicks(10));

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 29, 23, 0, 10, 247, DateTimeKind.Utc).AddTicks(13));

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 4,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 29, 23, 0, 10, 247, DateTimeKind.Utc).AddTicks(15));

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 5,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 29, 23, 0, 10, 247, DateTimeKind.Utc).AddTicks(17));

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 6,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 29, 23, 0, 10, 247, DateTimeKind.Utc).AddTicks(20));

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 7,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 29, 23, 0, 10, 247, DateTimeKind.Utc).AddTicks(89));

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 8,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 29, 23, 0, 10, 247, DateTimeKind.Utc).AddTicks(92));

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 9,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 29, 23, 0, 10, 247, DateTimeKind.Utc).AddTicks(93));

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 10,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 29, 23, 0, 10, 247, DateTimeKind.Utc).AddTicks(97));

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 11,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 29, 23, 0, 10, 247, DateTimeKind.Utc).AddTicks(98));

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 12,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 29, 23, 0, 10, 247, DateTimeKind.Utc).AddTicks(100));

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 13,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 29, 23, 0, 10, 247, DateTimeKind.Utc).AddTicks(101));

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 14,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 29, 23, 0, 10, 247, DateTimeKind.Utc).AddTicks(103));

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 15,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 29, 23, 0, 10, 247, DateTimeKind.Utc).AddTicks(105));

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 16,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 29, 23, 0, 10, 247, DateTimeKind.Utc).AddTicks(106));

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 17,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 29, 23, 0, 10, 247, DateTimeKind.Utc).AddTicks(108));

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 18,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 29, 23, 0, 10, 247, DateTimeKind.Utc).AddTicks(111));

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 19,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 29, 23, 0, 10, 247, DateTimeKind.Utc).AddTicks(113));

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 20,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 29, 23, 0, 10, 247, DateTimeKind.Utc).AddTicks(114));

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 21,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 29, 23, 0, 10, 247, DateTimeKind.Utc).AddTicks(116));

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 22,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 29, 23, 0, 10, 247, DateTimeKind.Utc).AddTicks(117));

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 23,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 29, 23, 0, 10, 247, DateTimeKind.Utc).AddTicks(119));

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 24,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 29, 23, 0, 10, 247, DateTimeKind.Utc).AddTicks(120));

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 25,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 29, 23, 0, 10, 247, DateTimeKind.Utc).AddTicks(122));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { 1, "1" },
                column: "AssignedAt",
                value: new DateTime(2025, 10, 29, 23, 0, 10, 326, DateTimeKind.Utc).AddTicks(9921));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { 2, "1" },
                column: "AssignedAt",
                value: new DateTime(2025, 10, 29, 23, 0, 10, 326, DateTimeKind.Utc).AddTicks(9926));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { 3, "1" },
                column: "AssignedAt",
                value: new DateTime(2025, 10, 29, 23, 0, 10, 326, DateTimeKind.Utc).AddTicks(9928));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { 4, "1" },
                column: "AssignedAt",
                value: new DateTime(2025, 10, 29, 23, 0, 10, 326, DateTimeKind.Utc).AddTicks(9929));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { 5, "1" },
                column: "AssignedAt",
                value: new DateTime(2025, 10, 29, 23, 0, 10, 326, DateTimeKind.Utc).AddTicks(9930));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { 6, "1" },
                column: "AssignedAt",
                value: new DateTime(2025, 10, 29, 23, 0, 10, 326, DateTimeKind.Utc).AddTicks(9930));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { 7, "1" },
                column: "AssignedAt",
                value: new DateTime(2025, 10, 29, 23, 0, 10, 326, DateTimeKind.Utc).AddTicks(9931));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { 8, "1" },
                column: "AssignedAt",
                value: new DateTime(2025, 10, 29, 23, 0, 10, 326, DateTimeKind.Utc).AddTicks(9931));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { 9, "1" },
                column: "AssignedAt",
                value: new DateTime(2025, 10, 29, 23, 0, 10, 326, DateTimeKind.Utc).AddTicks(9933));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { 10, "1" },
                column: "AssignedAt",
                value: new DateTime(2025, 10, 29, 23, 0, 10, 326, DateTimeKind.Utc).AddTicks(9933));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { 11, "1" },
                column: "AssignedAt",
                value: new DateTime(2025, 10, 29, 23, 0, 10, 326, DateTimeKind.Utc).AddTicks(9934));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { 12, "1" },
                column: "AssignedAt",
                value: new DateTime(2025, 10, 29, 23, 0, 10, 326, DateTimeKind.Utc).AddTicks(9935));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { 13, "1" },
                column: "AssignedAt",
                value: new DateTime(2025, 10, 29, 23, 0, 10, 326, DateTimeKind.Utc).AddTicks(9936));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { 14, "1" },
                column: "AssignedAt",
                value: new DateTime(2025, 10, 29, 23, 0, 10, 326, DateTimeKind.Utc).AddTicks(9937));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { 15, "1" },
                column: "AssignedAt",
                value: new DateTime(2025, 10, 29, 23, 0, 10, 326, DateTimeKind.Utc).AddTicks(9937));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { 16, "1" },
                column: "AssignedAt",
                value: new DateTime(2025, 10, 29, 23, 0, 10, 326, DateTimeKind.Utc).AddTicks(9938));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { 17, "1" },
                column: "AssignedAt",
                value: new DateTime(2025, 10, 29, 23, 0, 10, 326, DateTimeKind.Utc).AddTicks(9939));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { 18, "1" },
                column: "AssignedAt",
                value: new DateTime(2025, 10, 29, 23, 0, 10, 326, DateTimeKind.Utc).AddTicks(9940));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { 19, "1" },
                column: "AssignedAt",
                value: new DateTime(2025, 10, 29, 23, 0, 10, 326, DateTimeKind.Utc).AddTicks(9940));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { 20, "1" },
                column: "AssignedAt",
                value: new DateTime(2025, 10, 29, 23, 0, 10, 326, DateTimeKind.Utc).AddTicks(9941));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { 21, "1" },
                column: "AssignedAt",
                value: new DateTime(2025, 10, 29, 23, 0, 10, 326, DateTimeKind.Utc).AddTicks(9941));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { 22, "1" },
                column: "AssignedAt",
                value: new DateTime(2025, 10, 29, 23, 0, 10, 326, DateTimeKind.Utc).AddTicks(9942));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { 23, "1" },
                column: "AssignedAt",
                value: new DateTime(2025, 10, 29, 23, 0, 10, 326, DateTimeKind.Utc).AddTicks(9942));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { 24, "1" },
                column: "AssignedAt",
                value: new DateTime(2025, 10, 29, 23, 0, 10, 326, DateTimeKind.Utc).AddTicks(9943));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { 25, "1" },
                column: "AssignedAt",
                value: new DateTime(2025, 10, 29, 23, 0, 10, 326, DateTimeKind.Utc).AddTicks(9943));

            migrationBuilder.UpdateData(
                table: "UserRole",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "1", "1" },
                column: "AssignedAt",
                value: new DateTime(2025, 10, 29, 23, 0, 10, 326, DateTimeKind.Utc).AddTicks(9822));
        }
    }
}
