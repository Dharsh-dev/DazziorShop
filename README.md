# DazziorShop

An e-commerce **REST API** built with **ASP.NET Core (.NET 9)**, Entity
Framework Core and SQL Server. It manages products, brands and categories, with
user accounts and JWT login.

## Features

- CRUD for products, brands and categories
- Product filtering by category and brand, and pagination
- Registration and login with ASP.NET Core Identity and JWT
- Role-based access (`ADMIN`, `CUSTOMER`)
- API versioning (`v1` and `v2`)
- Swagger UI
- FluentValidation, AutoMapper and Serilog logging
- Exception handling middleware

## Project structure

| Project | What it holds |
|---|---|
| `DazziorShop.Domain` | Models and repository contracts |
| `DazziorShop.Application` | Services, DTOs, validation and mapping |
| `DazziorShop.Infrastructure` | Database context, migrations and repositories |
| `DazziorShop.Web` | Controllers, middleware and startup |

## Setup

You need the .NET 9 SDK and SQL Server running locally.

```powershell
git clone https://github.com/Dharsh-dev/DazziorShop.git
cd DazziorShop
```

### 1. Set the JWT signing key

The key is a secret, so it is not stored in the code. Create a random one and
save it with .NET user secrets:

```powershell
$bytes = New-Object byte[] 48
[System.Security.Cryptography.RandomNumberGenerator]::Create().GetBytes($bytes)
dotnet user-secrets set "JwtSettings:Key" ([Convert]::ToBase64String($bytes)) --project DazziorShop.Web
```

On a server, set the environment variable `JwtSettings__Key` instead. The key
must be at least 32 characters.

### 2. Check the database connection

The default connection in `DazziorShop.Web/appsettings.json` uses the local SQL
Server with Windows login. Change it if your server is different.

### 3. Run

```powershell
dotnet run --project DazziorShop.Web --launch-profile http
```

The app applies the database migrations and adds sample brands on startup. Open
Swagger at http://localhost:5145/swagger.

## Endpoints

| Method | URL | Login |
|---|---|---|
| POST | `/api/v1/User/Register` | No |
| POST | `/api/v1/User/Login` | No |
| GET | `/api/v1/Product` | Yes |
| POST | `/api/v1/Product/GetPagination` | ADMIN |
| GET | `/api/v1/Product/Filter?categoryId=&brandId=` | No |
| GET | `/api/v1/Product/Details?id=` | No |
| POST, PUT, DELETE | `/api/v1/Product` | No |
| GET, POST, PUT, DELETE | `/api/v1/Category` (details: `/Details?id=`) | Yes |
| GET | `/api/v1/Brand/all`, `/api/v1/Brand/Details?id=` | No |
| POST, PUT, DELETE | `/api/v1/Brand` | No |
| GET | `/api/v2/brand/all`, `/api/v2/brand/Details/{id}` | No |

To call a protected endpoint in Swagger, log in, press **Authorize** and enter
`Bearer <token>`.

This is a learning project: new accounts currently receive the `ADMIN` role.
