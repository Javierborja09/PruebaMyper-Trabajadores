using Microsoft.EntityFrameworkCore;
using Proyecto.Datos.DataContext;
using Proyecto.Datos.Repositories.Interfaces;
using Proyecto.Models;
using Proyecto.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proyecto.Datos.Repositories
{
    public class TrabajadorRepository : ITrabajadorRepository
    {
        private readonly TrabajadoresPruebaContext _dbcontext;
        public TrabajadorRepository(TrabajadoresPruebaContext context)
        {
            _dbcontext = context;
        }


        public async Task<bool> Actualizar(Trabajadore modelo)
        {
            var trabajador = await _dbcontext.Trabajadores.FindAsync(modelo.IdTrabajador);
            if (trabajador == null)
                return false;


            _dbcontext.Entry(trabajador).CurrentValues.SetValues(modelo);
            await _dbcontext.SaveChangesAsync();
            return true;
        }

        public async Task<bool> Eliminar(int id)
        {
            var trabajador = await _dbcontext.Trabajadores.FindAsync(id);
            if (trabajador == null)
                return false;
            _dbcontext.Trabajadores.Remove(trabajador);
            await _dbcontext.SaveChangesAsync();

            return true;
        }

        public async Task<bool> Insertar(Trabajadore modelo)
        {
            var trabajador = await _dbcontext.Trabajadores.FirstOrDefaultAsync(t =>t.IdTrabajador == modelo.IdTrabajador);
            if (trabajador != null)
                return false;


            await _dbcontext.Trabajadores.AddAsync(modelo);
            await _dbcontext.SaveChangesAsync();
            return true;
        }

        public async Task<Trabajadore> ObtenerPorId(int id)
        {
            return await _dbcontext.Trabajadores.FindAsync(id);
        }

        public async Task<IQueryable<Trabajadore>> obtenerTodo()
        {
            // Aca usamos .AsNoTracking() porque esta es una operación de solo lectura (GET).
            // Esto desactiva el seguimiento de cambios de Entity Framework, lo que 
            // reduce el consumo de memoria y mejora drásticamente el rendimiento 
            // al no tener que mantener una copia de las entidades en el Change Tracker.
            return await Task.FromResult(_dbcontext.Trabajadores.AsNoTracking());
        }


        // Metodos para consumir procedimientos para tener la logica mas ordenada ya que desde aca se tendria que realizar ya que tiene acceso directo al _dbcontext
        public async Task<List<TrabajadorVM>> ListarTrabajadores()
        {

            // Esto nos retornara todos los trabajadores ya que no le pasamos el parametro sexo por ende lo mandaria null
            return await _dbcontext.Set<TrabajadorVM>()
                .FromSqlRaw("EXEC sp_ListarTrabajadores")
                .ToListAsync();
        }
        public async Task<List<TrabajadorVM>> ListarTrabajadoresPorSexo(char? sexo)
        {
            // Esto nos retornara todos los trabajadores pero con el filtro de sexo en este caso si se le manda F o M se le retornara dependiendo
            return await _dbcontext.Set<TrabajadorVM>()
                .FromSqlRaw("EXEC sp_ListarTrabajadores @Sexo = {0}", sexo)
                .ToListAsync();
        }

        // metodo para validaciones  y no duplicar trabajadores con la misma id
        public async Task<Trabajadore?> ObtenerPorDocumento(string tipoDocumento, string numeroDocumento)
        {
            return await _dbcontext.Trabajadores
                .FirstOrDefaultAsync(t => t.TipoDocumento == tipoDocumento
                                       && t.NumeroDocumento == numeroDocumento);
        }

    }


}

