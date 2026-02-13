using Proyecto.Models;
using Proyecto.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proyecto.Datos.Repositories.Interfaces
{
    public interface ITrabajadoreRepository : IGenericRepository<Trabajadore>
    {
        Task<List<TrabajadorVM>> ListarTrabajadores();
        Task<List<TrabajadorVM>> ListarTrabajadoresPorSexo(char? sexo);
    }
}
