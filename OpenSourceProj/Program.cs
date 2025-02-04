using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using OpenSourceProj.DataAccess;
using OpenSourceProj.DbContextInfo;
using OpenSourceProj.Modals;
using OpenSourceProj.Repositorys;
using Serilog;
using System.Configuration;
using System.Text;

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
builder.Services.AddScoped<IJwtService, JwtService>();
//Enable Cors
builder.Services.AddCors(options =>
{  
        options.AddDefaultPolicy(builder =>
        {
            builder.AllowAnyOrigin()
                .AllowAnyHeader()
                .AllowAnyMethod();
        });   
});

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


//Jwt Configuration
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(x =>
{
    x.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer="localhost",
        ValidAudience="localhost",
        IssuerSigningKey= new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration.GetValue<string>("JwtConfig:Key"))),
        ClockSkew=TimeSpan.Zero
    };
});


var app = builder.Build();

//Global Execption
app.UseExceptionHandler(_ => { });


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
app.UseCors(options => options.AllowAnyOrigin().AllowAnyHeader().AllowAnyHeader());
app.Run();
