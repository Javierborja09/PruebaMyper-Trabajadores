using Proyecto.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proyecto.Negocio.Services.Interfaces
{
    public interface IProvinciaService
    {
        Task<bool> Insertar(Provincia modelo);
        Task<bool> Actualizar(Provincia modelo);
        Task<bool> Eliminar(int id);
        Task<Provincia> ObtenerPorId(int id);
        Task<IQueryable<Provincia>> obtenerTodo();

    }
}
