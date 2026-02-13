using Proyecto.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proyecto.Negocio.Services.Interfaces
{
    public interface IDepartamentoService
    {
        Task<bool> Insertar(Departamento modelo);
        Task<bool> Actualizar(Departamento modelo);
        Task<bool> Eliminar(int id);
        Task<Departamento> ObtenerPorId(int id);
        Task<IQueryable<Departamento>> obtenerTodo();

    }
}
