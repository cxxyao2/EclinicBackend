using EclinicBackend;
using EclinicBackend.Data;
using EclinicBackend.Enums;
using EclinicBackend.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddHttpContextAccessor();
var connectionString = builder.Configuration.GetConnectionString("PostgresConnection") ?? throw new InvalidOperationException("Connection string 'PostgresConnection' not found.");
var allowedOrigins = builder.Configuration["WEBSITE_CORS_ALLOWED_ORIGINS"] ?? throw new InvalidOperationException("variable 'WEBSITE_CORS_ALLOWED_ORIGINS' not found.");
var origins = allowedOrigins
    .Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowConfiguredOrigins",
        builder => builder
            .WithOrigins(origins)
            .AllowAnyMethod()
            .AllowAnyHeader()
            .AllowCredentials()
    );
});

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(connectionString));


builder.Services.AddAutoMapper(typeof(MappingProfile));

builder.Services.AddScoped<IUserService, UserService>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var authBuilder = builder.Services.AddAuthorizationBuilder();

authBuilder.AddPolicy("MedicalStaff", policy =>
    policy.RequireRole(
        UserRole.Practitioner.ToString(),
        UserRole.Nurse.ToString(),
        UserRole.Admin.ToString()
    ));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
