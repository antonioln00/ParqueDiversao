using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;
using ParqueDiversao.Context;
using ParqueDiversao.Services;

var builder = WebApplication.CreateBuilder(args);



builder.Services.AddControllers().AddJsonOptions(e => e.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<ApplicationDbContext>(e => e.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddScoped<SetorService>();

var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
