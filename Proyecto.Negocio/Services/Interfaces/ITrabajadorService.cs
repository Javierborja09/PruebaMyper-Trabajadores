using Proyecto.Models;
using Proyecto.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proyecto.Negocio.Services.Interfaces
{
    public interface ITrabajadoreService
    {
        Task<bool> Insertar(Trabajadore modelo);
        Task<bool> Actualizar(Trabajadore modelo);
        Task<bool> Eliminar(int id);
        Task<Trabajadore> ObtenerPorId(int id);

        // Métodos especializados para la vista
        // Estos llamarán a los procedimientos almacenados que creamos en el repositorio
        Task<List<TrabajadorVM>> ListarTodos();
        Task<List<TrabajadorVM>> ListarPorSexo(char? sexo);
    }
}
