
using System.Text;
using EclinicBackend.Data;
using EclinicBackend.Helpers;
using EclinicBackend.Services;
using EclinicBackend.Services.AppointmentService;
using EclinicBackend.Services.BedService;
using EclinicBackend.Services.EmailService;
using EclinicBackend.Services.ImageRecordService;
using EclinicBackend.Services.InpatientService;
using EclinicBackend.Services.LabTestService;
using EclinicBackend.Services.MedicationService;
using EclinicBackend.Services.PatientService;
using EclinicBackend.Services.PractitionerAvailabilityService;
using EclinicBackend.Services.PractitionerScheduleService;
using EclinicBackend.Services.PractitionerService;
using EclinicBackend.Services.PrescriptionService;
using EclinicBackend.Services.VisitRecordService;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Npgsql.EntityFrameworkCore.PostgreSQL;
using EclinicBackend.Hubs;
using EclinicBackend.Enums;
using EclinicBackend.Services.UserLogHistoryService;
using EclinicBackend;
using EclinicBackend.Services.AuthService;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHttpContextAccessor();

// Add SignalR
// builder.Services.AddSignalR(); todo

var connectionString = builder.Configuration.GetConnectionString("PostgresConnection") ?? throw new InvalidOperationException("Connection string 'PostgresConnection' not found.");

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(connectionString));


builder.Services.AddAutoMapper(typeof(MappingProfile));

// todo
// builder.Services.AddScoped<IPatientService, PatientService>();
// builder.Services.AddScoped<IPractitionerService, PractitionerService>();
// builder.Services.AddScoped<IPractitionerAvailabilityService, PractitionerAvailabilityService>();
// builder.Services.AddScoped<IVisitRecordService, VisitRecordService>();
// builder.Services.AddScoped<IAppointmentService, AppointmentService>();
// builder.Services.AddScoped<IMedicationService, MedicationService>();
// builder.Services.AddScoped<ILabTestService, LabTestService>();
// builder.Services.AddScoped<IImageRecordService, ImageRecordService>();
// builder.Services.AddScoped<IInpatientService, InpatientService>();
// builder.Services.AddScoped<IPrescriptionService, PrescriptionService>();
// builder.Services.AddScoped<IPractitionerScheduleService, PractitionerScheduleService>();
// builder.Services.AddScoped<IEmailService, EmailService>();
builder.Services.AddScoped<IUserService, UserService>();
// builder.Services.AddScoped<IAuthService, AuthService>();
// builder.Services.AddScoped<IBedService, BedService>();
// builder.Services.AddScoped<IChatService, ChatService>();
// builder.Services.AddScoped<IUserLogHistoryService, UserLogHistoryService>();

var allowedOrigins = builder.Configuration["WEBSITE_CORS_ALLOWED_ORIGINS"] ?? throw new InvalidOperationException("variable 'WEBSITE_CORS_ALLOWED_ORIGINS' not found.");

var _emailServer = builder.Configuration.GetSection("EmailConfiguration:Server").Value
               ?? throw new InvalidOperationException("Email server configuration is missing");

var _emailFrom = builder.Configuration.GetSection("EmailConfiguration:From").Value
            ?? throw new InvalidOperationException("Email from address is missing");

var _emailPassword = builder.Configuration.GetSection("EmailConfiguration:Password").Value
         ?? throw new InvalidOperationException("Email password is missing");

var _emailPort = builder.Configuration.GetSection("EmailConfiguration:Port").Value
      ?? throw new InvalidOperationException("Email port is missing");

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

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHttpContextAccessor();


var authBuilder = builder.Services.AddAuthorizationBuilder();

authBuilder.AddPolicy("MedicalStaff", policy =>
    policy.RequireRole(
        UserRole.Practitioner.ToString(),
        UserRole.Nurse.ToString(),
        UserRole.Admin.ToString()
    ));

// Add JWT Authentication
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.
                GetBytes(builder.Configuration["SECRETS_TOKEN"] ?? "")),
            ValidateIssuer = false,
            ValidateAudience = false,
        };

        // Configure the JWT Bearer Events for SignalR
        options.Events = new JwtBearerEvents
        {
            OnMessageReceived = context =>
            {
                var accessToken = context.Request.Query["access_token"];
                var path = context.HttpContext.Request.Path;

                if (!string.IsNullOrEmpty(accessToken) && path.StartsWithSegments("/chatHub"))
                {
                    context.Token = accessToken;
                }
                return Task.CompletedTask;
            }
        };
    }); ;



var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


app.UseCors("AllowConfiguredOrigins");

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.UseMiddleware<ErrorHandlerMiddleware>();
app.MapControllers();

// app.MapHub<ChatHub>("/chatHub"); todo3

app.Run();


// original version
// var builder = WebApplication.CreateBuilder(args);

// // Add services to the container.

// builder.Services.AddControllers();
// // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
// builder.Services.AddEndpointsApiExplorer();
// builder.Services.AddSwaggerGen();

// var app = builder.Build();

// // Configure the HTTP request pipeline.
// if (app.Environment.IsDevelopment())
// {
//     app.UseSwagger();
//     app.UseSwaggerUI();
// }

// app.UseHttpsRedirection();

// app.UseAuthorization();

// app.MapControllers();

// app.Run();
