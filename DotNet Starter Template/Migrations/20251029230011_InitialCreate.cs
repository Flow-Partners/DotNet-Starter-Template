using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace DotNet_Starter_Template.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsSystemRole = table.Column<bool>(type: "bit", nullable: false),
                    Priority = table.Column<int>(type: "int", nullable: false),
                    ParentRoleId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Color = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Icon = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Category = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    LastLoginAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ProfilePicture = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    DateOfBirth = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Gender = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    Address = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    City = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    State = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Country = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    PostalCode = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Timezone = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Language = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    Theme = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    UserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SecurityStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "bit", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "bit", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Permissions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Module = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Action = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Category = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Group = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    SubModule = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Feature = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Component = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Page = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Section = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsSystemPermission = table.Column<bool>(type: "bit", nullable: false),
                    IsDeprecated = table.Column<bool>(type: "bit", nullable: false),
                    ReplacementId = table.Column<int>(type: "int", nullable: true),
                    Level = table.Column<int>(type: "int", nullable: false),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Scope = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Priority = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Permissions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderKey = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserRole",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    AssignedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    AssignedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ExpiryDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsPrimary = table.Column<bool>(type: "bit", nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserRole", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_UserRole_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserRole_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RolePermissions",
                columns: table => new
                {
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    PermissionId = table.Column<int>(type: "int", nullable: false),
                    AssignedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    AssignedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ExpiryDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    Conditions = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Notes = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RolePermissions", x => new { x.RoleId, x.PermissionId });
                    table.ForeignKey(
                        name: "FK_RolePermissions_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RolePermissions_Permissions_PermissionId",
                        column: x => x.PermissionId,
                        principalTable: "Permissions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "Category", "Color", "ConcurrencyStamp", "CreatedAt", "Description", "Icon", "IsActive", "IsSystemRole", "Name", "NormalizedName", "ParentRoleId", "Priority", "UpdatedAt" },
                values: new object[,]
                {
                    { "1", "System", null, null, new DateTime(2025, 10, 29, 23, 0, 10, 247, DateTimeKind.Utc).AddTicks(216), "Full system access", null, true, true, "SuperAdmin", "SUPERADMIN", null, 1, null },
                    { "2", "Admin", null, null, new DateTime(2025, 10, 29, 23, 0, 10, 247, DateTimeKind.Utc).AddTicks(284), "Administrative access", null, true, true, "Admin", "ADMIN", null, 2, null },
                    { "3", "Management", null, null, new DateTime(2025, 10, 29, 23, 0, 10, 247, DateTimeKind.Utc).AddTicks(288), "Management access", null, true, true, "Manager", "MANAGER", null, 3, null },
                    { "4", "User", null, null, new DateTime(2025, 10, 29, 23, 0, 10, 247, DateTimeKind.Utc).AddTicks(307), "Basic user access", null, true, true, "User", "USER", null, 4, null },
                    { "5", "Guest", null, null, new DateTime(2025, 10, 29, 23, 0, 10, 247, DateTimeKind.Utc).AddTicks(310), "Limited guest access", null, true, true, "Guest", "GUEST", null, 5, null }
                });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "Address", "City", "ConcurrencyStamp", "Country", "CreatedAt", "DateOfBirth", "Email", "EmailConfirmed", "FirstName", "Gender", "IsActive", "Language", "LastLoginAt", "LastName", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "PostalCode", "ProfilePicture", "SecurityStamp", "State", "Theme", "Timezone", "TwoFactorEnabled", "UpdatedAt", "UserName" },
                values: new object[] { "1", 0, null, null, "382dd3bc-083b-4794-9dc8-1663573719af", null, new DateTime(2025, 10, 29, 23, 0, 10, 247, DateTimeKind.Utc).AddTicks(428), null, "superadmin@example.com", true, "Super", null, true, "en", null, "Admin", false, null, "SUPERADMIN@EXAMPLE.COM", "SUPERADMIN@EXAMPLE.COM", "AQAAAAIAAYagAAAAENVYyWaV5FFa2AZZAc1Q2/c/p1wFkyKaAs12dBBPeTMrKhcMkeqVAhPjugLQfvyc9Q==", null, false, null, null, "f3dea786-1dea-45fe-9427-19206fadb16f", null, "light", null, false, null, "superadmin@example.com" });

            migrationBuilder.InsertData(
                table: "Permissions",
                columns: new[] { "Id", "Action", "Category", "Component", "CreatedAt", "Description", "Feature", "Group", "IsActive", "IsDeprecated", "IsSystemPermission", "Level", "Module", "Name", "Page", "Priority", "ReplacementId", "Scope", "Section", "SubModule", "Type" },
                values: new object[,]
                {
                    { 1, "Create", "CRUD", null, new DateTime(2025, 10, 29, 23, 0, 10, 247, DateTimeKind.Utc).AddTicks(1), "Create users", null, null, true, false, false, 1, "Users", "Users.Create", null, 0, null, "Global", null, null, "Functional" },
                    { 2, "Read", "CRUD", null, new DateTime(2025, 10, 29, 23, 0, 10, 247, DateTimeKind.Utc).AddTicks(10), "View users", null, null, true, false, false, 1, "Users", "Users.Read", null, 0, null, "Global", null, null, "Functional" },
                    { 3, "Update", "CRUD", null, new DateTime(2025, 10, 29, 23, 0, 10, 247, DateTimeKind.Utc).AddTicks(13), "Update users", null, null, true, false, false, 1, "Users", "Users.Update", null, 0, null, "Global", null, null, "Functional" },
                    { 4, "Delete", "CRUD", null, new DateTime(2025, 10, 29, 23, 0, 10, 247, DateTimeKind.Utc).AddTicks(15), "Delete users", null, null, true, false, false, 1, "Users", "Users.Delete", null, 0, null, "Global", null, null, "Functional" },
                    { 5, "Activate", "Admin", null, new DateTime(2025, 10, 29, 23, 0, 10, 247, DateTimeKind.Utc).AddTicks(17), "Activate/deactivate users", null, null, true, false, false, 1, "Users", "Users.Activate", null, 0, null, "Global", null, null, "Functional" },
                    { 6, "ResetPassword", "Admin", null, new DateTime(2025, 10, 29, 23, 0, 10, 247, DateTimeKind.Utc).AddTicks(20), "Reset user passwords", null, null, true, false, false, 1, "Users", "Users.ResetPassword", null, 0, null, "Global", null, null, "Functional" },
                    { 7, "AssignRoles", "Admin", null, new DateTime(2025, 10, 29, 23, 0, 10, 247, DateTimeKind.Utc).AddTicks(89), "Assign roles to users", null, null, true, false, false, 1, "Users", "Users.AssignRoles", null, 0, null, "Global", null, null, "Functional" },
                    { 8, "ViewAuditLog", "Audit", null, new DateTime(2025, 10, 29, 23, 0, 10, 247, DateTimeKind.Utc).AddTicks(92), "View user audit logs", null, null, true, false, false, 1, "Users", "Users.ViewAuditLog", null, 0, null, "Global", null, null, "Functional" },
                    { 9, "Create", "CRUD", null, new DateTime(2025, 10, 29, 23, 0, 10, 247, DateTimeKind.Utc).AddTicks(93), "Create roles", null, null, true, false, false, 1, "Roles", "Roles.Create", null, 0, null, "Global", null, null, "Functional" },
                    { 10, "Read", "CRUD", null, new DateTime(2025, 10, 29, 23, 0, 10, 247, DateTimeKind.Utc).AddTicks(97), "View roles", null, null, true, false, false, 1, "Roles", "Roles.Read", null, 0, null, "Global", null, null, "Functional" },
                    { 11, "Update", "CRUD", null, new DateTime(2025, 10, 29, 23, 0, 10, 247, DateTimeKind.Utc).AddTicks(98), "Update roles", null, null, true, false, false, 1, "Roles", "Roles.Update", null, 0, null, "Global", null, null, "Functional" },
                    { 12, "Delete", "CRUD", null, new DateTime(2025, 10, 29, 23, 0, 10, 247, DateTimeKind.Utc).AddTicks(100), "Delete roles", null, null, true, false, false, 1, "Roles", "Roles.Delete", null, 0, null, "Global", null, null, "Functional" },
                    { 13, "AssignPermissions", "Admin", null, new DateTime(2025, 10, 29, 23, 0, 10, 247, DateTimeKind.Utc).AddTicks(101), "Assign permissions to roles", null, null, true, false, false, 1, "Roles", "Roles.AssignPermissions", null, 0, null, "Global", null, null, "Functional" },
                    { 14, "AssignUsers", "Admin", null, new DateTime(2025, 10, 29, 23, 0, 10, 247, DateTimeKind.Utc).AddTicks(103), "Assign users to roles", null, null, true, false, false, 1, "Roles", "Roles.AssignUsers", null, 0, null, "Global", null, null, "Functional" },
                    { 15, "Create", "CRUD", null, new DateTime(2025, 10, 29, 23, 0, 10, 247, DateTimeKind.Utc).AddTicks(105), "Create permissions", null, null, true, false, false, 1, "Permissions", "Permissions.Create", null, 0, null, "Global", null, null, "Functional" },
                    { 16, "Read", "CRUD", null, new DateTime(2025, 10, 29, 23, 0, 10, 247, DateTimeKind.Utc).AddTicks(106), "View permissions", null, null, true, false, false, 1, "Permissions", "Permissions.Read", null, 0, null, "Global", null, null, "Functional" },
                    { 17, "Update", "CRUD", null, new DateTime(2025, 10, 29, 23, 0, 10, 247, DateTimeKind.Utc).AddTicks(108), "Update permissions", null, null, true, false, false, 1, "Permissions", "Permissions.Update", null, 0, null, "Global", null, null, "Functional" },
                    { 18, "Delete", "CRUD", null, new DateTime(2025, 10, 29, 23, 0, 10, 247, DateTimeKind.Utc).AddTicks(111), "Delete permissions", null, null, true, false, false, 1, "Permissions", "Permissions.Delete", null, 0, null, "Global", null, null, "Functional" },
                    { 19, "Assign", "Admin", null, new DateTime(2025, 10, 29, 23, 0, 10, 247, DateTimeKind.Utc).AddTicks(113), "Assign permissions to roles", null, null, true, false, false, 1, "Permissions", "Permissions.Assign", null, 0, null, "Global", null, null, "Functional" },
                    { 20, "Settings", "Admin", null, new DateTime(2025, 10, 29, 23, 0, 10, 247, DateTimeKind.Utc).AddTicks(114), "Manage system settings", null, null, true, false, false, 1, "System", "System.Settings", null, 0, null, "Global", null, null, "Functional" },
                    { 21, "Backup", "Admin", null, new DateTime(2025, 10, 29, 23, 0, 10, 247, DateTimeKind.Utc).AddTicks(116), "Create system backups", null, null, true, false, false, 1, "System", "System.Backup", null, 0, null, "Global", null, null, "Functional" },
                    { 22, "Restore", "Admin", null, new DateTime(2025, 10, 29, 23, 0, 10, 247, DateTimeKind.Utc).AddTicks(117), "Restore system backups", null, null, true, false, false, 1, "System", "System.Restore", null, 0, null, "Global", null, null, "Functional" },
                    { 23, "Logs", "Admin", null, new DateTime(2025, 10, 29, 23, 0, 10, 247, DateTimeKind.Utc).AddTicks(119), "View system logs", null, null, true, false, false, 1, "System", "System.Logs", null, 0, null, "Global", null, null, "Functional" },
                    { 24, "Monitoring", "Admin", null, new DateTime(2025, 10, 29, 23, 0, 10, 247, DateTimeKind.Utc).AddTicks(120), "System monitoring access", null, null, true, false, false, 1, "System", "System.Monitoring", null, 0, null, "Global", null, null, "Functional" },
                    { 25, "Maintenance", "Admin", null, new DateTime(2025, 10, 29, 23, 0, 10, 247, DateTimeKind.Utc).AddTicks(122), "System maintenance access", null, null, true, false, false, 1, "System", "System.Maintenance", null, 0, null, "Global", null, null, "Functional" }
                });

            migrationBuilder.InsertData(
                table: "RolePermissions",
                columns: new[] { "PermissionId", "RoleId", "AssignedAt", "AssignedBy", "Conditions", "ExpiryDate", "IsActive", "Notes" },
                values: new object[,]
                {
                    { 1, "1", new DateTime(2025, 10, 29, 23, 0, 10, 326, DateTimeKind.Utc).AddTicks(9921), "System", null, null, true, null },
                    { 2, "1", new DateTime(2025, 10, 29, 23, 0, 10, 326, DateTimeKind.Utc).AddTicks(9926), "System", null, null, true, null },
                    { 3, "1", new DateTime(2025, 10, 29, 23, 0, 10, 326, DateTimeKind.Utc).AddTicks(9928), "System", null, null, true, null },
                    { 4, "1", new DateTime(2025, 10, 29, 23, 0, 10, 326, DateTimeKind.Utc).AddTicks(9929), "System", null, null, true, null },
                    { 5, "1", new DateTime(2025, 10, 29, 23, 0, 10, 326, DateTimeKind.Utc).AddTicks(9930), "System", null, null, true, null },
                    { 6, "1", new DateTime(2025, 10, 29, 23, 0, 10, 326, DateTimeKind.Utc).AddTicks(9930), "System", null, null, true, null },
                    { 7, "1", new DateTime(2025, 10, 29, 23, 0, 10, 326, DateTimeKind.Utc).AddTicks(9931), "System", null, null, true, null },
                    { 8, "1", new DateTime(2025, 10, 29, 23, 0, 10, 326, DateTimeKind.Utc).AddTicks(9931), "System", null, null, true, null },
                    { 9, "1", new DateTime(2025, 10, 29, 23, 0, 10, 326, DateTimeKind.Utc).AddTicks(9933), "System", null, null, true, null },
                    { 10, "1", new DateTime(2025, 10, 29, 23, 0, 10, 326, DateTimeKind.Utc).AddTicks(9933), "System", null, null, true, null },
                    { 11, "1", new DateTime(2025, 10, 29, 23, 0, 10, 326, DateTimeKind.Utc).AddTicks(9934), "System", null, null, true, null },
                    { 12, "1", new DateTime(2025, 10, 29, 23, 0, 10, 326, DateTimeKind.Utc).AddTicks(9935), "System", null, null, true, null },
                    { 13, "1", new DateTime(2025, 10, 29, 23, 0, 10, 326, DateTimeKind.Utc).AddTicks(9936), "System", null, null, true, null },
                    { 14, "1", new DateTime(2025, 10, 29, 23, 0, 10, 326, DateTimeKind.Utc).AddTicks(9937), "System", null, null, true, null },
                    { 15, "1", new DateTime(2025, 10, 29, 23, 0, 10, 326, DateTimeKind.Utc).AddTicks(9937), "System", null, null, true, null },
                    { 16, "1", new DateTime(2025, 10, 29, 23, 0, 10, 326, DateTimeKind.Utc).AddTicks(9938), "System", null, null, true, null },
                    { 17, "1", new DateTime(2025, 10, 29, 23, 0, 10, 326, DateTimeKind.Utc).AddTicks(9939), "System", null, null, true, null },
                    { 18, "1", new DateTime(2025, 10, 29, 23, 0, 10, 326, DateTimeKind.Utc).AddTicks(9940), "System", null, null, true, null },
                    { 19, "1", new DateTime(2025, 10, 29, 23, 0, 10, 326, DateTimeKind.Utc).AddTicks(9940), "System", null, null, true, null },
                    { 20, "1", new DateTime(2025, 10, 29, 23, 0, 10, 326, DateTimeKind.Utc).AddTicks(9941), "System", null, null, true, null },
                    { 21, "1", new DateTime(2025, 10, 29, 23, 0, 10, 326, DateTimeKind.Utc).AddTicks(9941), "System", null, null, true, null },
                    { 22, "1", new DateTime(2025, 10, 29, 23, 0, 10, 326, DateTimeKind.Utc).AddTicks(9942), "System", null, null, true, null },
                    { 23, "1", new DateTime(2025, 10, 29, 23, 0, 10, 326, DateTimeKind.Utc).AddTicks(9942), "System", null, null, true, null },
                    { 24, "1", new DateTime(2025, 10, 29, 23, 0, 10, 326, DateTimeKind.Utc).AddTicks(9943), "System", null, null, true, null },
                    { 25, "1", new DateTime(2025, 10, 29, 23, 0, 10, 326, DateTimeKind.Utc).AddTicks(9943), "System", null, null, true, null }
                });

            migrationBuilder.InsertData(
                table: "UserRole",
                columns: new[] { "RoleId", "UserId", "AssignedAt", "AssignedBy", "ExpiryDate", "IsActive", "IsPrimary", "Notes" },
                values: new object[] { "1", "1", new DateTime(2025, 10, 29, 23, 0, 10, 326, DateTimeKind.Utc).AddTicks(9822), "System", null, true, true, null });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true,
                filter: "[NormalizedName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true,
                filter: "[NormalizedUserName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_RolePermissions_PermissionId",
                table: "RolePermissions",
                column: "PermissionId");

            migrationBuilder.CreateIndex(
                name: "IX_UserRole_RoleId",
                table: "UserRole",
                column: "RoleId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "RolePermissions");

            migrationBuilder.DropTable(
                name: "UserRole");

            migrationBuilder.DropTable(
                name: "Permissions");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "AspNetUsers");
        }
    }
}
