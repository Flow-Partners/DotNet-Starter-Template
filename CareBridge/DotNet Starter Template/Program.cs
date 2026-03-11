using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Serilog;
using CareBridge.Data;
using CareBridge.Models.Entities;
using CareBridge.Repositories.Interfaces;
using CareBridge.Repositories.Implementations;
using CareBridge.Services.Interfaces;
using CareBridge.Services.Implementations;
using CareBridge.Middleware;

// Configure Serilog
Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(new ConfigurationBuilder()
        .AddJsonFile("appsettings.json")
        .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production"}.json", optional: true)
        .Build())
    .CreateLogger();

try
{
    Log.Information("Starting web application");

    var builder = WebApplication.CreateBuilder(args);

    // Add Serilog
    builder.Host.UseSerilog();

    // Add services to the container
    builder.Services.AddControllers();

    // Add CORS
    builder.Services.AddCors(options =>
    {
        options.AddPolicy("AllowAll", policy =>
        {
            policy.AllowAnyOrigin()
                  .AllowAnyMethod()
                  .AllowAnyHeader();
        });
    });

    // Add Entity Framework
    builder.Services.AddDbContext<ApplicationDbContext>(options =>
        options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

    // Add Identity
    builder.Services.AddIdentity<User, Role>(options =>
    {
        // Password settings
        options.Password.RequireDigit = true;
        options.Password.RequireLowercase = true;
        options.Password.RequireNonAlphanumeric = true;
        options.Password.RequireUppercase = true;
        options.Password.RequiredLength = 8;
        options.Password.RequiredUniqueChars = 1;

        // Lockout settings
        options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
        options.Lockout.MaxFailedAccessAttempts = 5;
        options.Lockout.AllowedForNewUsers = true;

        // User settings
        options.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
        options.User.RequireUniqueEmail = true;
    })
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

    // Add JWT Authentication
    var jwtSettings = builder.Configuration.GetSection("JwtSettings");
    var secretKey = jwtSettings["SecretKey"] ?? "YourSuperSecretKeyThatIsAtLeast32CharactersLong!";
    var issuer = jwtSettings["Issuer"] ?? "DotNetStarterTemplate";
    var audience = jwtSettings["Audience"] ?? "DotNetStarterTemplate";

    builder.Services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = issuer,
            ValidAudience = audience,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey)),
            ClockSkew = TimeSpan.Zero
        };
    });

    // Register repositories
    builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
    builder.Services.AddScoped<IUserRepository, UserRepository>();
    builder.Services.AddScoped<IRoleRepository, RoleRepository>();
    builder.Services.AddScoped<IPermissionRepository, PermissionRepository>();
    builder.Services.AddScoped<IUserRoleRepository, UserRoleRepository>();
    builder.Services.AddScoped<IRolePermissionRepository, RolePermissionRepository>();

    // Register services
    builder.Services.AddScoped<IAuthService, AuthService>();
    builder.Services.AddScoped<IRoleService, RoleService>();
    builder.Services.AddScoped<IPermissionService, PermissionService>();
    builder.Services.AddScoped<IUserService, UserService>();

    // Add AutoMapper
    builder.Services.AddAutoMapper(typeof(Program));

    // Add Health Checks
    builder.Services.AddHealthChecks();

    // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen(c =>
    {
        c.SwaggerDoc("v1", new() { Title = "Care Bridge (Dental) API", Version = "v1" });
        
        // Add JWT authentication to Swagger
        c.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
        {
            Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
            Name = "Authorization",
            In = Microsoft.OpenApi.Models.ParameterLocation.Header,
            Type = Microsoft.OpenApi.Models.SecuritySchemeType.ApiKey,
            Scheme = "Bearer"
        });

        c.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
        {
            {
                new Microsoft.OpenApi.Models.OpenApiSecurityScheme
                {
                    Reference = new Microsoft.OpenApi.Models.OpenApiReference
                    {
                        Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                        Id = "Bearer"
                    }
                },
                Array.Empty<string>()
            }
        });
    });

    var app = builder.Build();

    // Configure the HTTP request pipeline
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    // Add middleware
    app.UseMiddleware<ErrorHandlingMiddleware>();
    app.UseMiddleware<RequestLoggingMiddleware>();

    app.UseCors("AllowAll");

    app.UseHttpsRedirection();

    app.UseAuthentication();
    app.UseAuthorization();

    app.MapControllers();

    // Map health checks
    app.MapHealthChecks("/health");

    // Ensure database is migrated and Patient table has required columns
    using (var scope = app.Services.CreateScope())
    {
        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        try
        {
            context.Database.Migrate();
        }
        catch (Exception ex)
        {
            Log.Warning(ex, "Database migration failed, trying to ensure database exists");
            context.Database.EnsureCreated();
        }

        // Ensure Patients and Dentists tables have all required columns (idempotent; fixes older schema)
        try
        {
            var ensureColumns = @"
IF EXISTS (SELECT 1 FROM sys.tables WHERE name = 'Patients')
BEGIN
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
END
IF EXISTS (SELECT 1 FROM sys.tables WHERE name = 'Dentists')
BEGIN
    IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID('Dentists') AND name = 'IsActive')
        ALTER TABLE Dentists ADD IsActive bit NOT NULL DEFAULT 1;
    IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID('Dentists') AND name = 'UpdatedAt')
        ALTER TABLE Dentists ADD UpdatedAt datetime2 NULL;
END
IF EXISTS (SELECT 1 FROM sys.tables WHERE name = 'Appointments')
BEGIN
    IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID('Appointments') AND name = 'ProcedureId')
        ALTER TABLE Appointments ADD ProcedureId int NULL;
    IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID('Appointments') AND name = 'TotalAmount')
        ALTER TABLE Appointments ADD TotalAmount decimal(18,2) NOT NULL DEFAULT 0;
    IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID('Appointments') AND name = 'UpdatedAt')
        ALTER TABLE Appointments ADD UpdatedAt datetime2 NULL;
END
IF EXISTS (SELECT 1 FROM sys.tables WHERE name = 'Procedures')
BEGIN
    IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID('Procedures') AND name = 'IsActive')
        ALTER TABLE Procedures ADD IsActive bit NOT NULL DEFAULT 1;
END
IF EXISTS (SELECT 1 FROM sys.tables WHERE name = 'Invoices')
BEGIN
    IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID('Invoices') AND name = 'UpdatedAt')
        ALTER TABLE Invoices ADD UpdatedAt datetime2 NULL;
END
";
            context.Database.ExecuteSqlRaw(ensureColumns);

            // Create Procedures table if missing (e.g. migration not applied)
            var createProceduresIfMissing = @"
IF NOT EXISTS (SELECT 1 FROM sys.tables WHERE name = 'Procedures')
BEGIN
    CREATE TABLE [Procedures] (
        [Id] int NOT NULL IDENTITY(1,1),
        [Name] nvarchar(200) NOT NULL,
        [Code] nvarchar(20) NOT NULL,
        [DefaultPrice] decimal(18,2) NOT NULL,
        [Category] nvarchar(100) NULL,
        [Description] nvarchar(max) NULL,
        [IsActive] bit NOT NULL DEFAULT 1,
        [CreatedAt] datetime2 NOT NULL,
        CONSTRAINT [PK_Procedures] PRIMARY KEY ([Id])
    );
END
";
            context.Database.ExecuteSqlRaw(createProceduresIfMissing);
        }
        catch (Exception ex)
        {
            Log.Warning(ex, "Ensure dental table columns step failed (tables may not exist yet)");
        }

        // Seed default procedures via raw SQL (avoids separate connection / EF query; uses same connection as above)
        try
        {
            var seedProcedures = @"
IF EXISTS (SELECT 1 FROM sys.tables WHERE name = 'Procedures')
AND NOT EXISTS (SELECT 1 FROM [Procedures])
BEGIN
  SET NOCOUNT ON;
  INSERT INTO [Procedures] (Name, Code, DefaultPrice, Category, Description, IsActive, CreatedAt) VALUES
  (N'Checkup', N'CHK', 75, N'Preventive', NULL, 1, GETUTCDATE()),
  (N'Cleaning', N'CLN', 120, N'Preventive', NULL, 1, GETUTCDATE()),
  (N'Filling', N'FIL', 150, N'Restorative', NULL, 1, GETUTCDATE()),
  (N'Root Canal', N'RCT', 800, N'Endodontic', NULL, 1, GETUTCDATE()),
  (N'Extraction', N'EXT', 200, N'Oral Surgery', NULL, 1, GETUTCDATE());
END
";
            context.Database.ExecuteSqlRaw(seedProcedures);
        }
        catch (Exception ex)
        {
            Log.Warning(ex, "Ensure default procedures step failed");
        }
    }

    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Application terminated unexpectedly");
}
finally
{
    Log.CloseAndFlush();
}
