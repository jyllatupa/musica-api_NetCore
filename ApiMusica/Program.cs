using Microsoft.EntityFrameworkCore;
using ApiMusica.Models;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//configurando DbContext con la cadena conexion sql en program
builder.Services.AddDbContext<MusicaContext>(opt => opt.UseSqlServer(builder.Configuration.GetConnectionString("cadenaSQL")));
//------------------------------------------------
builder.Services.AddControllers().AddJsonOptions(opt =>
{
    opt.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
});
//------------------------------------------------

//ACTIVAR CORS para que pueda ser usado en cualquier dominio
var misReglasCORS = "ReglasCORS";
builder.Services.AddCors(opt =>
{
    opt.AddPolicy(name: misReglasCORS, builder =>
    {
        builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
    });
});
//---------------------------------------------------------------------------------------

var app = builder.Build();

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
    
//}

app.UseSwagger();
app.UseSwaggerUI();

//ACTIVANDO CORS
app.UseCors(misReglasCORS);
//------------------------------------------------------------------------------------

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
