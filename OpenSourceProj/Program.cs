using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using OpenSourceProj.DataAccess;
using OpenSourceProj.DateFilterGenericRepo.GenericRepoService;
using OpenSourceProj.DateFilterGenericRepo.IGenericService;
using OpenSourceProj.DbContextInfo;
using OpenSourceProj.HostedBackGroundService;
using OpenSourceProj.Modals;
using OpenSourceProj.Repositorys;
using Serilog;
using System.Configuration;
using System.Text;
using Microsoft.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
//Connection string Initialization 
builder.Services.AddDbContext<DbContextFile>(opts => opts.UseSqlServer(builder.Configuration.GetConnectionString("DbConnectionString")));
builder.Services.AddControllers();
//DI
builder.Services.AddScoped<IUserLoginAppService, UserLoginAppService>();
builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));

builder.Services.AddScoped<IJwtService, JwtService>();
// 1. Add CORS Policy

// Register the hosted service
//builder.Services.AddHostedService<ScheduledTaskService>();


builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllOrigins", builder =>
    {
        builder.AllowAnyOrigin()
               .AllowAnyMethod()
               .AllowAnyHeader();
    });
});


// 2. Add Authentication & Authorization
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        // Set to true in production
        options.RequireHttpsMetadata = false;

        // Store the token to use later if needed
        options.SaveToken = true;

        // Validate token expiration and signature
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidIssuer = builder.Configuration["JwtConfig:Issuer"],  // "localhost" or your value
            ValidAudience = builder.Configuration["JwtConfig:Audience"],  // "localhost" or your value
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration.GetValue<string>("JwtConfig:Key"))),
            ClockSkew = TimeSpan.FromMinutes(5)  // Default clock skew, can adjust based on need
        };
    });

//Configure Authorization
builder.Services.AddAuthorization();

//Serilog
// Configure Serilog
string ? logDirectory  = builder.Configuration.GetValue<string>("LogsInternalPath");

Log.Logger = new LoggerConfiguration()
    //.WriteTo.Console()      // Log to console
    .WriteTo.File(Path.Combine(logDirectory, ".txt"), rollingInterval: RollingInterval.Day)  // Log to file (optional)
    .CreateLogger();

// Use Serilog for logging in the app
builder.Logging.ClearProviders();  // Optional: clear default logging providers
builder.Logging.AddSerilog();

//Global Execption
builder.Services.AddExceptionHandler<GlobalExecption>();
//builder.Services.AddProblemDetails(); // Enables structured error responses

var app = builder.Build();

//Global Execption
app.UseExceptionHandler(_ => { });
app.UseCors("AllowAllOrigins");
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.Run();
