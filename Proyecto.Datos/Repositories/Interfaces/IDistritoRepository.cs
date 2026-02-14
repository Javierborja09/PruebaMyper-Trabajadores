using Proyecto.Models;
using Proyecto.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proyecto.Datos.Repositories.Interfaces
{
    public interface IDistritoRepository : IGenericRepository<Distrito>
    {
        Task<DistritoVM> ObtenerDetalleCompleto(int idDistrito);
    }
}
