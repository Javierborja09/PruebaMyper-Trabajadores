using Proyecto.Datos.Repositories;
using Proyecto.Datos.Repositories.Interfaces;
using Proyecto.Models;
using Proyecto.Models.ViewModels;
using Proyecto.Negocio.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proyecto.Negocio.Services
{
    public class TrabajadorService : ITrabajadorService
    {
        private readonly ITrabajadorRepository _repository;

        public TrabajadorService(ITrabajadorRepository repository) => _repository = repository;

        public async Task<bool> Insertar(Trabajadore modelo) => await _repository.Insertar(modelo);

        public async Task<bool> Actualizar(Trabajadore modelo) => await _repository.Actualizar(modelo);

        public async Task<bool> Eliminar(int id) => await _repository.Eliminar(id);

        public async Task<Trabajadore> ObtenerPorId(int id) => await _repository.ObtenerPorId(id);

        public async Task<List<TrabajadorVM>> ListarTodos() => await _repository.ListarTrabajadores();

        public async Task<List<TrabajadorVM>> ListarPorSexo(char? sexo) => await _repository.ListarTrabajadoresPorSexo(sexo);
    }
}
