using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using CareBridge.Models.Entities;

namespace CareBridge.Data
{
    public class ApplicationDbContext : IdentityDbContext<User, Role, string>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<Permission> Permissions { get; set; }
        public DbSet<RolePermission> RolePermissions { get; set; }

        // Dental / Care Bridge entities
        public DbSet<Patient> Patients { get; set; } = null!;
        public DbSet<Dentist> Dentists { get; set; } = null!;
        public DbSet<Procedure> Procedures { get; set; } = null!;
        public DbSet<DentistProcedure> DentistProcedures { get; set; } = null!;
        public DbSet<Appointment> Appointments { get; set; } = null!;
        public DbSet<AppointmentImage> AppointmentImages { get; set; } = null!;
        public DbSet<Invoice> Invoices { get; set; } = null!;
        public DbSet<Payment> Payments { get; set; } = null!;
        public DbSet<PendingPayment> PendingPayments { get; set; } = null!;
        public DbSet<Prescription> Prescriptions { get; set; } = null!;
        public DbSet<PrescriptionItem> PrescriptionItems { get; set; } = null!;
        public DbSet<AppointmentReminder> AppointmentReminders { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Configure UserRole many-to-many
            builder.Entity<UserRole>()
                .HasKey(ur => new { ur.UserId, ur.RoleId });

            builder.Entity<UserRole>()
                .HasOne(ur => ur.User)
                .WithMany(u => u.UserRoles)
                .HasForeignKey(ur => ur.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<UserRole>()
                .HasOne(ur => ur.Role)
                .WithMany(r => r.UserRoles)
                .HasForeignKey(ur => ur.RoleId)
                .OnDelete(DeleteBehavior.Cascade);

            // Configure RolePermission many-to-many
            builder.Entity<RolePermission>()
                .HasKey(rp => new { rp.RoleId, rp.PermissionId });

            builder.Entity<RolePermission>()
                .HasOne(rp => rp.Role)
                .WithMany(r => r.RolePermissions)
                .HasForeignKey(rp => rp.RoleId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<RolePermission>()
                .HasOne(rp => rp.Permission)
                .WithMany(p => p.RolePermissions)
                .HasForeignKey(rp => rp.PermissionId)
                .OnDelete(DeleteBehavior.Cascade);

            // Dental / Care Bridge entity relationships
            builder.Entity<Appointment>()
                .HasOne(a => a.Patient)
                .WithMany(p => p.Appointments)
                .HasForeignKey(a => a.PatientId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Appointment>()
                .HasOne(a => a.Dentist)
                .WithMany(d => d.Appointments)
                .HasForeignKey(a => a.DentistId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Appointment>()
                .HasOne(a => a.Procedure)
                .WithMany(pr => pr.Appointments)
                .HasForeignKey(a => a.ProcedureId)
                .OnDelete(DeleteBehavior.SetNull);

            builder.Entity<AppointmentImage>()
                .HasOne(ai => ai.Appointment)
                .WithMany(a => a.AppointmentImages)
                .HasForeignKey(ai => ai.AppointmentId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Invoice>()
                .HasOne(i => i.Appointment)
                .WithOne(a => a.Invoice)
                .HasForeignKey<Invoice>(i => i.AppointmentId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Invoice>()
                .HasOne(i => i.Patient)
                .WithMany(p => p.Invoices)
                .HasForeignKey(i => i.PatientId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Invoice>()
                .HasOne(i => i.Dentist)
                .WithMany(d => d.Invoices)
                .HasForeignKey(i => i.DentistId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Invoice>()
                .HasIndex(i => i.AppointmentId)
                .IsUnique();

            builder.Entity<Payment>()
                .HasOne(p => p.Invoice)
                .WithMany(i => i.Payments)
                .HasForeignKey(p => p.InvoiceId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<PendingPayment>()
                .HasOne(pp => pp.Invoice)
                .WithMany(i => i.PendingPayments)
                .HasForeignKey(pp => pp.InvoiceId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<PendingPayment>()
                .HasOne(pp => pp.Patient)
                .WithMany(p => p.PendingPayments)
                .HasForeignKey(pp => pp.PatientId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<DentistProcedure>()
                .HasKey(dp => new { dp.DentistId, dp.ProcedureId });

            builder.Entity<DentistProcedure>()
                .HasOne(dp => dp.Dentist)
                .WithMany(d => d.DentistProcedures)
                .HasForeignKey(dp => dp.DentistId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<DentistProcedure>()
                .HasOne(dp => dp.Procedure)
                .WithMany(p => p.DentistProcedures)
                .HasForeignKey(dp => dp.ProcedureId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Prescription>()
                .HasOne(pr => pr.Appointment)
                .WithMany(a => a.Prescriptions)
                .HasForeignKey(pr => pr.AppointmentId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Prescription>()
                .HasOne(pr => pr.Patient)
                .WithMany()
                .HasForeignKey(pr => pr.PatientId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Prescription>()
                .HasOne(pr => pr.Dentist)
                .WithMany(d => d.Prescriptions)
                .HasForeignKey(pr => pr.DentistId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<PrescriptionItem>()
                .HasOne(pi => pi.Prescription)
                .WithMany(p => p.PrescriptionItems)
                .HasForeignKey(pi => pi.PrescriptionId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<AppointmentReminder>()
                .HasOne(ar => ar.Appointment)
                .WithMany(a => a.AppointmentReminders)
                .HasForeignKey(ar => ar.AppointmentId)
                .OnDelete(DeleteBehavior.Cascade);

            // Seed initial data
            SeedData(builder);
        }

        private void SeedData(ModelBuilder builder)
        {
            // Seed Permissions
            var permissions = new List<Permission>
            {
                new Permission { Id = 1, Name = "Users.Create", Description = "Create users", Module = "Users", Action = "Create", Category = "CRUD" },
                new Permission { Id = 2, Name = "Users.Read", Description = "View users", Module = "Users", Action = "Read", Category = "CRUD" },
                new Permission { Id = 3, Name = "Users.Update", Description = "Update users", Module = "Users", Action = "Update", Category = "CRUD" },
                new Permission { Id = 4, Name = "Users.Delete", Description = "Delete users", Module = "Users", Action = "Delete", Category = "CRUD" },
                new Permission { Id = 5, Name = "Users.Activate", Description = "Activate/deactivate users", Module = "Users", Action = "Activate", Category = "Admin" },
                new Permission { Id = 6, Name = "Users.ResetPassword", Description = "Reset user passwords", Module = "Users", Action = "ResetPassword", Category = "Admin" },
                new Permission { Id = 7, Name = "Users.AssignRoles", Description = "Assign roles to users", Module = "Users", Action = "AssignRoles", Category = "Admin" },
                new Permission { Id = 8, Name = "Users.ViewAuditLog", Description = "View user audit logs", Module = "Users", Action = "ViewAuditLog", Category = "Audit" },
                
                new Permission { Id = 9, Name = "Roles.Create", Description = "Create roles", Module = "Roles", Action = "Create", Category = "CRUD" },
                new Permission { Id = 10, Name = "Roles.Read", Description = "View roles", Module = "Roles", Action = "Read", Category = "CRUD" },
                new Permission { Id = 11, Name = "Roles.Update", Description = "Update roles", Module = "Roles", Action = "Update", Category = "CRUD" },
                new Permission { Id = 12, Name = "Roles.Delete", Description = "Delete roles", Module = "Roles", Action = "Delete", Category = "CRUD" },
                new Permission { Id = 13, Name = "Roles.AssignPermissions", Description = "Assign permissions to roles", Module = "Roles", Action = "AssignPermissions", Category = "Admin" },
                new Permission { Id = 14, Name = "Roles.AssignUsers", Description = "Assign users to roles", Module = "Roles", Action = "AssignUsers", Category = "Admin" },
                
                new Permission { Id = 15, Name = "Permissions.Create", Description = "Create permissions", Module = "Permissions", Action = "Create", Category = "CRUD" },
                new Permission { Id = 16, Name = "Permissions.Read", Description = "View permissions", Module = "Permissions", Action = "Read", Category = "CRUD" },
                new Permission { Id = 17, Name = "Permissions.Update", Description = "Update permissions", Module = "Permissions", Action = "Update", Category = "CRUD" },
                new Permission { Id = 18, Name = "Permissions.Delete", Description = "Delete permissions", Module = "Permissions", Action = "Delete", Category = "CRUD" },
                new Permission { Id = 19, Name = "Permissions.Assign", Description = "Assign permissions to roles", Module = "Permissions", Action = "Assign", Category = "Admin" },
                
                new Permission { Id = 20, Name = "System.Settings", Description = "Manage system settings", Module = "System", Action = "Settings", Category = "Admin" },
                new Permission { Id = 21, Name = "System.Backup", Description = "Create system backups", Module = "System", Action = "Backup", Category = "Admin" },
                new Permission { Id = 22, Name = "System.Restore", Description = "Restore system backups", Module = "System", Action = "Restore", Category = "Admin" },
                new Permission { Id = 23, Name = "System.Logs", Description = "View system logs", Module = "System", Action = "Logs", Category = "Admin" },
                new Permission { Id = 24, Name = "System.Monitoring", Description = "System monitoring access", Module = "System", Action = "Monitoring", Category = "Admin" },
                new Permission { Id = 25, Name = "System.Maintenance", Description = "System maintenance access", Module = "System", Action = "Maintenance", Category = "Admin" }
            };

            builder.Entity<Permission>().HasData(permissions);

            // Seed Roles
            var roles = new List<Role>
            {
                new Role { Id = "1", Name = "SuperAdmin", NormalizedName = "SUPERADMIN", Description = "Full system access", IsSystemRole = true, Priority = 1, Category = "System" },
                new Role { Id = "2", Name = "Admin", NormalizedName = "ADMIN", Description = "Administrative access", IsSystemRole = true, Priority = 2, Category = "Admin" },
                new Role { Id = "3", Name = "Manager", NormalizedName = "MANAGER", Description = "Management access", IsSystemRole = true, Priority = 3, Category = "Management" },
                new Role { Id = "4", Name = "User", NormalizedName = "USER", Description = "Basic user access", IsSystemRole = true, Priority = 4, Category = "User" },
                new Role { Id = "5", Name = "Guest", NormalizedName = "GUEST", Description = "Limited guest access", IsSystemRole = true, Priority = 5, Category = "Guest" }
            };

            builder.Entity<Role>().HasData(roles);

            // Seed SuperAdmin user
            var hasher = new Microsoft.AspNetCore.Identity.PasswordHasher<User>();
            var superAdmin = new User
            {
                Id = "1",
                UserName = "superadmin@example.com",
                NormalizedUserName = "SUPERADMIN@EXAMPLE.COM",
                Email = "superadmin@example.com",
                NormalizedEmail = "SUPERADMIN@EXAMPLE.COM",
                EmailConfirmed = true,
                FirstName = "Super",
                LastName = "Admin",
                SecurityStamp = Guid.NewGuid().ToString(),
                CreatedAt = DateTime.UtcNow,
                IsActive = true
            };
            superAdmin.PasswordHash = hasher.HashPassword(superAdmin, "SuperAdmin123!");

            builder.Entity<User>().HasData(superAdmin);

            // Assign SuperAdmin role to super admin user
            builder.Entity<UserRole>().HasData(
                new UserRole { UserId = "1", RoleId = "1", AssignedBy = "System", IsPrimary = true }
            );

            // Assign all permissions to SuperAdmin role
            var rolePermissions = permissions.Select(p => new RolePermission
            {
                RoleId = "1",
                PermissionId = p.Id,
                AssignedBy = "System"
            }).ToList();

            builder.Entity<RolePermission>().HasData(rolePermissions);
        }
    }
}
