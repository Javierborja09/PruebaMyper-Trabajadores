
using Microsoft.EntityFrameworkCore;
using Proyecto.Datos.DataContext;
using Proyecto.Datos.Repositories;
using Proyecto.Datos.Repositories.Interfaces;
using Proyecto.Models;
using Proyecto.Negocio.Services;
using Proyecto.Negocio.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Registramos los repositorios de la capa Datos
builder.Services.AddScoped<ITrabajadorRepository, TrabajadorRepository>();
builder.Services.AddScoped<IGenericRepository<Departamento>, DepartamentoRepository>();
builder.Services.AddScoped<IGenericRepository<Provincia>, ProvinciaRepository>();
builder.Services.AddScoped<IDistritoRepository, DistritoRepository>();

// Registramos los servicios de la capa services
builder.Services.AddScoped<ITrabajadorService, TrabajadorService>();
builder.Services.AddScoped<IUbigeoService, UbigeoService>();

builder.Services.AddControllersWithViews();
builder.Services.AddAuthorization();

// Aca configuramos la base de datos
var connectionString = builder.Configuration.GetConnectionString("BaseDeDatosTrabajadores");

if (string.IsNullOrWhiteSpace(connectionString))
{
    throw new InvalidOperationException("La cadena de conexión 'BaseDeDatosTrabajadores' no está configurada correctamente.");
}

builder.Services.AddDbContext<TrabajadoresPruebaContext>(opciones =>
{
    opciones.UseSqlServer(connectionString);
});


var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}

app.UseStaticFiles();
app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Trabajador}/{action=Index}/{id?}");

app.Run();