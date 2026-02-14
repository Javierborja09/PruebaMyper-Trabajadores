using Proyecto.Models;
using Proyecto.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proyecto.Negocio.Services.Interfaces
{
    public interface IDistritoService
    {
        Task<bool> Insertar(Distrito modelo);
        Task<bool> Actualizar(Distrito modelo);
        Task<bool> Eliminar(int id);
        Task<Distrito> ObtenerPorId(int id);
        Task<IQueryable<Distrito>> obtenerTodo();


        Task<DistritoVM> ObtenerDetalleCompleto(int id);
    }
}
