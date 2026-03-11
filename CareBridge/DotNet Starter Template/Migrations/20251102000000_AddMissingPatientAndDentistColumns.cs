using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CareBridge.Migrations
{
    /// <inheritdoc />
    public class AddMissingPatientAndDentistColumns : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Add missing columns to Patients only if they don't exist (idempotent; safe on rerun)
            var addIfNotExists = @"
IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID('Patients') AND name = 'AllowPayLater')
    ALTER TABLE Patients ADD AllowPayLater bit NOT NULL DEFAULT 0;
IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID('Patients') AND name = 'CreditLimit')
    ALTER TABLE Patients ADD CreditLimit decimal(18,2) NULL;
IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID('Patients') AND name = 'EmergencyContactName')
    ALTER TABLE Patients ADD EmergencyContactName nvarchar(200) NULL;
IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID('Patients') AND name = 'EmergencyContactPhone')
    ALTER TABLE Patients ADD EmergencyContactPhone nvarchar(50) NULL;
IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID('Patients') AND name = 'UpdatedAt')
    ALTER TABLE Patients ADD UpdatedAt datetime2 NULL;

IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID('Dentists') AND name = 'IsActive')
    ALTER TABLE Dentists ADD IsActive bit NOT NULL DEFAULT 1;
IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID('Dentists') AND name = 'UpdatedAt')
    ALTER TABLE Dentists ADD UpdatedAt datetime2 NULL;

IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID('Appointments') AND name = 'ProcedureId')
    ALTER TABLE Appointments ADD ProcedureId int NULL;
IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID('Appointments') AND name = 'TotalAmount')
    ALTER TABLE Appointments ADD TotalAmount decimal(18,2) NOT NULL DEFAULT 0;
IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID('Appointments') AND name = 'UpdatedAt')
    ALTER TABLE Appointments ADD UpdatedAt datetime2 NULL;

IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID('Procedures') AND name = 'IsActive')
    ALTER TABLE Procedures ADD IsActive bit NOT NULL DEFAULT 1;
IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID('Invoices') AND name = 'UpdatedAt')
    ALTER TABLE Invoices ADD UpdatedAt datetime2 NULL;
";
            migrationBuilder.Sql(addIfNotExists);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(name: "AllowPayLater", table: "Patients");
            migrationBuilder.DropColumn(name: "CreditLimit", table: "Patients");
            migrationBuilder.DropColumn(name: "EmergencyContactName", table: "Patients");
            migrationBuilder.DropColumn(name: "EmergencyContactPhone", table: "Patients");
            migrationBuilder.DropColumn(name: "UpdatedAt", table: "Patients");
            migrationBuilder.DropColumn(name: "IsActive", table: "Dentists");
            migrationBuilder.DropColumn(name: "UpdatedAt", table: "Dentists");
            migrationBuilder.DropColumn(name: "ProcedureId", table: "Appointments");
            migrationBuilder.DropColumn(name: "TotalAmount", table: "Appointments");
            migrationBuilder.DropColumn(name: "UpdatedAt", table: "Appointments");
            migrationBuilder.Sql("IF EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID('Procedures') AND name = 'IsActive') ALTER TABLE Procedures DROP COLUMN IsActive;");
            migrationBuilder.Sql("IF EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID('Invoices') AND name = 'UpdatedAt') ALTER TABLE Invoices DROP COLUMN UpdatedAt;");
        }
    }
}
