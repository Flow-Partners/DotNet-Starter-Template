# .NET Starter Template

A comprehensive .NET 8 Web API starter template with authentication, authorization, logging, and complete CRUD operations for users, roles, and permissions.

## 🚀 Features

### Core Features
- **.NET 8** Web API
- **Entity Framework Core** with SQL Server
- **ASP.NET Core Identity** for authentication
- **JWT Bearer Authentication**
- **Serilog** structured logging
- **Swagger/OpenAPI** documentation
- **Repository Pattern** implementation
- **Service Layer** architecture
- **AutoMapper** for object mapping
- **Health Checks** for monitoring

### Authentication & Authorization
- JWT token-based authentication
- Role-based access control (RBAC)
- Permission-based authorization
- User management with roles and permissions
- Password policies and validation

### Logging & Monitoring
- Structured logging with Serilog
- Console, File, and Database logging
- Request/Response logging middleware
- Error handling middleware
- Health check endpoints

### API Features
- RESTful API design
- Pagination support
- Input validation
- Error handling
- CORS configuration
- API versioning ready

## 📁 Project Structure

```
DotNet Starter Template/
├── Controllers/           # API Controllers
│   ├── AuthController.cs
│   ├── UsersController.cs
│   ├── RolesController.cs
│   └── PermissionsController.cs
├── Models/
│   ├── Entities/          # Database entities
│   ├── DTOs/              # Data Transfer Objects
│   ├── ViewModels/        # API response models
│   └── Common/            # Shared models
├── Services/
│   ├── Interfaces/        # Service contracts
│   └── Implementations/   # Service implementations
├── Repositories/
│   ├── Interfaces/        # Repository contracts
│   └── Implementations/   # Repository implementations
├── Data/
│   └── ApplicationDbContext.cs
├── Middleware/
│   ├── ErrorHandlingMiddleware.cs
│   └── RequestLoggingMiddleware.cs
└── Program.cs
```

## 🛠️ Getting Started

### Prerequisites
- .NET 8 SDK
- SQL Server (LocalDB or full instance)
- Visual Studio 2022 or VS Code

### Installation

1. **Clone the repository**
   ```bash
   git clone <repository-url>
   cd "DotNet Starter Template"
   ```

2. **Update connection string**
   Edit `appsettings.json` and update the connection string:
   ```json
   {
     "ConnectionStrings": {
       "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=DotNetStarterTemplateDb;Trusted_Connection=true;MultipleActiveResultSets=true"
     }
   }
   ```

3. **Run the application**
   ```bash
   dotnet run
   ```

4. **Access the API**
   - API: `http://localhost:5000`
   - Swagger UI: `http://localhost:5000/swagger`
   - Health Check: `http://localhost:5000/health`

## 🔐 Default Credentials

- **Email:** `superadmin@example.com`
- **Password:** `SuperAdmin123!`

## 📚 API Endpoints

### Authentication
- `POST /api/auth/login` - User login
- `POST /api/auth/register` - User registration
- `GET /api/auth/profile` - Get user profile
- `POST /api/auth/logout` - User logout
- `POST /api/auth/change-password` - Change password

### Users Management
- `GET /api/users` - List all users
- `GET /api/users/{id}` - Get user by ID
- `POST /api/users` - Create new user
- `PUT /api/users/{id}` - Update user
- `DELETE /api/users/{id}` - Delete user
- `PUT /api/users/{id}/activate` - Activate/deactivate user
- `POST /api/users/{id}/reset-password` - Reset user password
- `GET /api/users/{id}/roles` - Get user roles
- `POST /api/users/{id}/roles/{roleId}` - Assign role to user
- `DELETE /api/users/{id}/roles/{roleId}` - Remove role from user
- `GET /api/users/{id}/permissions` - Get user permissions

### Roles Management
- `GET /api/roles` - List all roles
- `GET /api/roles/{id}` - Get role by ID
- `POST /api/roles` - Create new role
- `PUT /api/roles/{id}` - Update role
- `DELETE /api/roles/{id}` - Delete role
- `PUT /api/roles/{id}/activate` - Activate/deactivate role
- `GET /api/roles/{id}/permissions` - Get role permissions
- `POST /api/roles/{id}/permissions/{permissionId}` - Assign permission to role
- `DELETE /api/roles/{id}/permissions/{permissionId}` - Remove permission from role
- `GET /api/roles/{id}/users` - Get users with role

### Permissions Management
- `GET /api/permissions` - List all permissions
- `GET /api/permissions/{id}` - Get permission by ID
- `POST /api/permissions` - Create new permission
- `PUT /api/permissions/{id}` - Update permission
- `DELETE /api/permissions/{id}` - Delete permission
- `PUT /api/permissions/{id}/activate` - Activate/deactivate permission
- `GET /api/permissions/{id}/roles` - Get roles with permission
- `GET /api/permissions/modules` - Get all modules
- `GET /api/permissions/actions` - Get all actions
- `GET /api/permissions/categories` - Get all categories

## 🔧 Configuration

### JWT Settings
```json
{
  "JwtSettings": {
    "SecretKey": "YourSuperSecretKeyThatIsAtLeast32CharactersLong!",
    "Issuer": "DotNetStarterTemplate",
    "Audience": "DotNetStarterTemplate",
    "ExpiryMinutes": 60,
    "RefreshTokenExpiryDays": 7
  }
}
```

### Serilog Configuration
```json
{
  "Serilog": {
    "MinimumLevel": {
      "Default": "Information",
      "Override": {
        "Microsoft": "Warning",
        "System": "Warning"
      }
    },
    "WriteTo": [
      {
        "Name": "Console"
      },
      {
        "Name": "File",
        "Args": {
          "path": "Logs/Information/log-.txt",
          "rollingInterval": "Day"
        }
      },
      {
        "Name": "MSSqlServer",
        "Args": {
          "connectionString": "...",
          "tableName": "Logs"
        }
      }
    ]
  }
}
```

## 🗄️ Database Schema

### Core Tables
- **AspNetUsers** - User accounts (extended with custom fields)
- **AspNetRoles** - User roles (extended with custom fields)
- **AspNetUserRoles** - User-role assignments
- **Permissions** - System permissions
- **RolePermissions** - Role-permission assignments
- **Logs** - Application logs (Serilog)

### Seeded Data
- **SuperAdmin** user with full access
- **System roles:** SuperAdmin, Admin, Manager, User, Guest
- **System permissions:** CRUD operations for Users, Roles, Permissions, System settings

## 🧪 Testing the API

### Using Swagger UI
1. Navigate to `http://localhost:5000/swagger`
2. Click "Authorize" and enter JWT token
3. Test endpoints interactively

### Using PowerShell
```powershell
# Login
$body = @{ Email = "superadmin@example.com"; Password = "SuperAdmin123!" } | ConvertTo-Json
$response = Invoke-RestMethod -Uri "http://localhost:5000/api/auth/login" -Method Post -Body $body -ContentType "application/json"
$token = $response.data.token

# Test protected endpoint
Invoke-RestMethod -Uri "http://localhost:5000/api/users" -Method Get -Headers @{ Authorization = "Bearer $token" }
```

## 📝 Development Notes

### Adding New Features
1. Create entity in `Models/Entities/`
2. Add repository interface and implementation
3. Create service interface and implementation
4. Add controller with CRUD operations
5. Register services in `Program.cs`

### Logging
- All requests and responses are automatically logged
- Errors are captured with stack traces
- Logs are written to console, files, and database

### Security
- JWT tokens expire after 60 minutes
- Password policies enforced
- CORS configured for development
- Sensitive headers are redacted in logs

## 🤝 Contributing

1. Fork the repository
2. Create a feature branch
3. Make your changes
4. Add tests if applicable
5. Submit a pull request

## 📄 License

This project is licensed under the MIT License.

## 🆘 Support

For questions or issues:
1. Check the logs in `Logs/` directory
2. Review the Swagger documentation
3. Check the health endpoint at `/health`
4. Create an issue in the repository

---

**Happy Coding! 🚀**

