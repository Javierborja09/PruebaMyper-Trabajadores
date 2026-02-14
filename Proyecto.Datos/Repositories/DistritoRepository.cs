using Proyecto.Datos.DataContext;
using Proyecto.Datos.Repositories.Interfaces;
using Proyecto.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Proyecto.Models.ViewModels;

namespace Proyecto.Datos.Repositories
{
    public class DistritoRepository : IDistritoRepository

    {

        private readonly TrabajadoresPruebaContext _dbcontext;
        public DistritoRepository(TrabajadoresPruebaContext context)
        {
            _dbcontext = context;
        }

        public async Task<bool> Actualizar(Distrito modelo)
        {
            var distrito = await _dbcontext.Distritos.FindAsync(modelo.IdDistrito);
            if (distrito == null)
                return false;


            _dbcontext.Entry(distrito).CurrentValues.SetValues(modelo);
            await _dbcontext.SaveChangesAsync();
            return true;
        }

        public async Task<bool> Eliminar(int id)
        {
            var distrito = await _dbcontext.Distritos.FindAsync(id);
            if (distrito == null)
                return false;
            _dbcontext.Distritos.Remove(distrito);
            await _dbcontext.SaveChangesAsync();


            return true;
        }

        public async Task<bool> Insertar(Distrito modelo)
        {
            var distrito = await _dbcontext.Distritos.FirstOrDefaultAsync(d => d.IdDistrito == modelo.IdDistrito);
            if (distrito != null)
                return false;


            await _dbcontext.Distritos.AddAsync(modelo);
            await _dbcontext.SaveChangesAsync();
            return true;
        }

        public async Task<Distrito> ObtenerPorId(int id)
        {
            return await _dbcontext.Distritos.FindAsync(id);
        }

        public async Task<IQueryable<Distrito>> obtenerTodo()
        {
            // Aca usamos .AsNoTracking() porque esta es una operación de solo lectura (GET).
            // Esto desactiva el seguimiento de cambios de Entity Framework, lo que 
            // reduce el consumo de memoria y mejora drásticamente el rendimiento 
            // al no tener que mantener una copia de las entidades en el Change Tracker.
            return await Task.FromResult(_dbcontext.Distritos.AsNoTracking());
        }

        // Aca Creamos 'ObtenerDetalleCompleto' aquí porque, honestamente, es donde debe estar. 
        // No queremos que el Trabajador se ande preocupando por cómo se conectan las provincias 
        // o los departamentos; su única tarea es saber su ID de distrito.
        // Al hacerlo así, mantenemos la lógica de ubicación en un solo lugar. Así, si mañana 
        // necesitas mostrar una dirección en cualquier otra parte del sistema, ya tendremos 
        // el trabajo hecho, ordenado y sin repetir código. Es separar las cosas para que 
        // el proyecto no se vuelva un caos cuando crezca.
        public async Task<DistritoVM> ObtenerDetalleCompleto(int idDistrito)
        {
            var resultado = await _dbcontext.Set<DistritoVM>()
                .FromSqlRaw("EXEC sp_ObtenerDetalleDistrito @IdDistrito = {0}", idDistrito)
                .ToListAsync();

            return resultado.FirstOrDefault();
        }
    }
}
