using Proyecto.Datos.DataContext;
using Proyecto.Datos.Repositories.Interfaces;
using Proyecto.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Proyecto.Datos.Repositories
{
    public class DistritoRepository : IGenericRepository<Distrito>

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
    }
}
