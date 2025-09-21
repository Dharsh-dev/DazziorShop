using Asp.Versioning;
using Asp.Versioning.ApiExplorer;
using Asp.Versioning.Conventions;
using DazziorShop.Application;
using DazziorShop.Application.ApplicationConstants;
using DazziorShop.Application.Common;
using DazziorShop.Application.Services.Interface;
using DazziorShop.Infrastructure;
using DazziorShop.Infrastructure.Common;
using DazziorShop.Infrastructure.Dbcontexts;
using DazziorShop.Web.MiddleWare;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Net;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// -------------------- Services -------------------- //
builder.Services.AddApplicationServices();
builder.Services.AddInfrastructureServices();

builder.Services.AddCors(options => options.AddPolicy("AllowAll", policy =>
    policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader()));

// -------------------- Database -------------------- //
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
{
    options.SignIn.RequireConfirmedEmail = false;
    options.User.RequireUniqueEmail = true;
}).AddEntityFrameworkStores<ApplicationDbContext>();

// -------------------- API Versioning -------------------- //
builder.Services.AddApiVersioning(options =>
{
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.DefaultApiVersion = new ApiVersion(1, 0);
    options.ReportApiVersions = true;
})
.AddApiExplorer(options =>
{
    options.GroupNameFormat = "'v'VVV";
    options.SubstituteApiVersionInUrl = true;
});

// -------------------- Controllers -------------------- //
builder.Services.AddControllers(options =>
{
    options.CacheProfiles.Add("Default", new CacheProfile { Duration = 30 });
});
builder.Services.AddEndpointsApiExplorer();

// -------------------- JWT Authentication -------------------- //
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        ValidateIssuer = true,
        ValidateAudience = true,
        ClockSkew = TimeSpan.Zero,
        ValidIssuer = builder.Configuration["JwtSettings:Issuer"],
        ValidAudience = builder.Configuration["JwtSettings:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JwtSettings:Key"]))
    };
});

// -------------------- Swagger -------------------- //
builder.Services.AddSwaggerGen();

//{
//    options.SwaggerDoc("v1", new OpenApiInfo { Title = "Dazzior API", Version = "v1" });
//    options.SwaggerDoc("v2", new OpenApiInfo { Title = "Dazzior API", Version = "v2" });
//});


// ------------------ Configure Swagger to use API versioni ---------------//
builder.Services.ConfigureOptions<ConfigureSwaggerOptions>();

// -------------------- Database Seeding -------------------- //
static async void UpdateDatabaseAsync(IHost host)
{
    using var scope = host.Services.CreateScope();
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<ApplicationDbContext>();
        if (context.Database.IsSqlServer())
            context.Database.Migrate();

        await SeedData.SeedDataAsync(context);
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occured while migrating or seeding the database");
    }
}

// -------------------- Build App -------------------- //
var app = builder.Build();

app.UseMiddleware<ExceptionMiddleware>();

UpdateDatabaseAsync(app);
await SeedData.SeedRoles(app.Services);

// -------------------- Swagger UI -------------------- //
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        var provider = app.Services.GetRequiredService<IApiVersionDescriptionProvider>();
        foreach (var description in provider.ApiVersionDescriptions)
        {
            options.SwaggerEndpoint(
             $"/swagger/{description.GroupName}/swagger.json",
             $"Dazzior API v{description.ApiVersion}"
             );
        }
    });
}

// -------------------- Middleware -------------------- //
app.UseCors("AllowAll");
//app.UseMiddleware<ExceptionMiddleware>();
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

// -------------------- Controllers -------------------- //
app.MapControllers();

app.Run();

