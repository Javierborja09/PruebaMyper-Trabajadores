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
    public class DepartamentoRepository : IGenericRepository<Departamento>
    {

        private readonly TrabajadoresPruebaContext _dbcontext;
        public DepartamentoRepository(TrabajadoresPruebaContext context) {
            _dbcontext = context;
        }

        public async Task<bool> Actualizar(Departamento modelo)
        {
            var departamento = await _dbcontext.Departamentos.FindAsync(modelo.IdDepartamento);
            if (departamento == null)
                return false;


            _dbcontext.Entry(departamento).CurrentValues.SetValues(modelo);
            await _dbcontext.SaveChangesAsync();
            return true;
        }

        public async Task<bool> Eliminar(int id)
        {
            var departamento = await _dbcontext.Departamentos.FindAsync(id);
            if (departamento == null)
                return false;
            _dbcontext.Departamentos.Remove(departamento);
            await _dbcontext.SaveChangesAsync();

            return true;
        }

        public async Task<bool> Insertar(Departamento modelo)
        {
            var departamento = await _dbcontext.Departamentos.FirstOrDefaultAsync(d => d.IdDepartamento == modelo.IdDepartamento);
            if (departamento != null)
                return false;


            await _dbcontext.Departamentos.AddAsync(modelo);
            await _dbcontext.SaveChangesAsync();
            return true;
        }

        public async Task<Departamento> ObtenerPorId(int id)
        {
            return await _dbcontext.Departamentos.FindAsync(id);
        }

        public async Task<IQueryable<Departamento>> obtenerTodo()
        {
            // Aca usamos .AsNoTracking() porque esta es una operación de solo lectura (GET).
            // Esto desactiva el seguimiento de cambios de Entity Framework, lo que 
            // reduce el consumo de memoria y mejora drásticamente el rendimiento 
            // al no tener que mantener una copia de las entidades en el Change Tracker.
            return await Task.FromResult(_dbcontext.Departamentos.AsNoTracking());
        }
    }
}
