using Microsoft.EntityFrameworkCore;
using Proyecto.Datos.DataContext;
using Proyecto.Datos.Repositories.Interfaces;
using Proyecto.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proyecto.Datos.Repositories
{
    public class ProvinciaRepository : IGenericRepository<Provincia>
    {

        private readonly TrabajadoresPruebaContext _dbcontext;
        public ProvinciaRepository(TrabajadoresPruebaContext context)
        {
            _dbcontext = context;
        }

        public async Task<bool>  Actualizar(Provincia modelo)
        {
            var provincia = await _dbcontext.Provincias.FindAsync(modelo.IdProvincia);
            if (provincia == null)
                return false;


            _dbcontext.Entry(provincia).CurrentValues.SetValues(modelo);
            await _dbcontext.SaveChangesAsync();
            return true;
        }

        public async Task<bool> Eliminar(int id)
        {
            var provincia = await _dbcontext.Provincias.FindAsync(id);
            if (provincia == null)
                return false;
            _dbcontext.Provincias.Remove(provincia);
            await _dbcontext.SaveChangesAsync();

            return true;
        }

        public async Task<bool> Insertar(Provincia modelo)
        {
            var provincia = await _dbcontext.Provincias.FirstOrDefaultAsync(d => d.IdProvincia == modelo.IdProvincia);
            if (provincia != null)
                return false;


            await _dbcontext.Provincias.AddAsync(modelo);
            await _dbcontext.SaveChangesAsync();
            return true;
        }

        public async Task<Provincia> ObtenerPorId(int id)
        {
            return await _dbcontext.Provincias.FindAsync(id);
        }

        public async Task<IQueryable<Provincia>> obtenerTodo()
        {
            // Aca usamos .AsNoTracking() porque esta es una operación de solo lectura (GET).
            // Esto desactiva el seguimiento de cambios de Entity Framework, lo que 
            // reduce el consumo de memoria y mejora drásticamente el rendimiento 
            // al no tener que mantener una copia de las entidades en el Change Tracker.
            return await Task.FromResult(_dbcontext.Provincias.AsNoTracking());
        }
    }
}
