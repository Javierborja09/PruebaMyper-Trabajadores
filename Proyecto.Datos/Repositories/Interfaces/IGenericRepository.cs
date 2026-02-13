using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proyecto.Datos.Repositories.Interfaces
{

    //Creacion de logica de interfas generica para poder reutilizarla en diferentes Repositorys
    public interface IGenericRepository<TEntityModel> where TEntityModel : class
    {
        Task<bool> Insertar(TEntityModel modelo);
        Task<bool> Actualizar(TEntityModel modelo);

        Task<bool> Eliminar(int id);

        Task<TEntityModel> ObtenerPorId(int id);
        //Aca trabajaremos con IQueryable por que es obtenido directamente de la db no desde memoria
        Task<IQueryable<TEntityModel>> obtenerTodo();
    }
}
