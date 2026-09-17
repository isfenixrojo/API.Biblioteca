using API.Biblioteca.Datos;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

#region Area de servicios 
builder.Services.AddAutoMapper(typeof(Program));

builder.Services.AddControllers().AddNewtonsoftJson();

builder.Services.AddDbContext<ApplicationDbContext>(opcions => opcions.UseSqlServer("name=ConnBiblioteca"));


#endregion Area de servicios

var app = builder.Build();
#region Area de Middleware
app.MapControllers();
#endregion Area de Middleware

app.Run();
