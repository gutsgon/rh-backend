using DotNetEnv;
using Microsoft.EntityFrameworkCore;
using Rh_Backend.Data;
using SeuProjeto.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Logging.ClearProviders();
builder.Logging.AddConsole();

Env.Load();
var connectionString = Environment.GetEnvironmentVariable("DATABASE_URL");

// Add services to the container.

// LEMBRARRRRRRRRRRRR DOS TESTESSSSSSS UNITÁRIOS
// LEMBRARRRRRRRRRRRR DOS TESTESSSSSSS INTEGRAÇÃO
// LEMBRARRRRRRRRRRRR DOS TESTESSSSSSS UNITÁRIOS

builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod());
});

builder.Services.AddAutoMapper(typeof(Program));
builder.Services.AddDbContext<AppDbContext>(options =>{ options.UseSqlServer(connectionString);});
builder.Services.AddApplicationServices();




var app = builder.Build();
app.UseMiddleware<ExceptionMiddleware>();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowAll");

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
