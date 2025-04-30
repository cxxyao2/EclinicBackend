using EclinicBackend;
using EclinicBackend.Data;
using EclinicBackend.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddHttpContextAccessor();
var connectionString = builder.Configuration.GetConnectionString("PostgresConnection") ?? throw new InvalidOperationException("Connection string 'PostgresConnection' not found.");
var allowedOrigins = builder.Configuration["WEBSITE_CORS_ALLOWED_ORIGINS"] ?? throw new InvalidOperationException("variable 'WEBSITE_CORS_ALLOWED_ORIGINS' not found.");
var _emailServer = builder.Configuration.GetSection("EmailConfiguration:Server").Value
               ?? throw new InvalidOperationException("Email server configuration is missing");

var _emailFrom = builder.Configuration.GetSection("EmailConfiguration:From").Value
            ?? throw new InvalidOperationException("Email from address is missing");

var _emailPassword = builder.Configuration.GetSection("EmailConfiguration:Password").Value
         ?? throw new InvalidOperationException("Email password is missing");

var _emailPort = builder.Configuration.GetSection("EmailConfiguration:Port").Value
      ?? throw new InvalidOperationException("Email port is missing");

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(connectionString));


builder.Services.AddAutoMapper(typeof(MappingProfile));

builder.Services.AddScoped<IUserService, UserService>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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
