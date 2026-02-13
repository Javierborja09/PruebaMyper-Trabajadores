using Proyecto.Datos.Repositories.Interfaces;
using Proyecto.Models;
using Proyecto.Negocio.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proyecto.Negocio.Services
{
    public class DistritoService : IDistritoService
    {
        private readonly IGenericRepository<Distrito> _repository;

        public DistritoService(IGenericRepository<Distrito> repository)
        {
            _repository = repository;
        }

        public async Task<bool> Insertar(Distrito modelo) => await _repository.Insertar(modelo);

        public async Task<bool> Actualizar(Distrito modelo) => await _repository.Actualizar(modelo);

        public async Task<bool> Eliminar(int id) => await _repository.Eliminar(id);

        public async Task<Distrito> ObtenerPorId(int id) => await _repository.ObtenerPorId(id);

        public async Task<IQueryable<Distrito>> obtenerTodo() => await _repository.obtenerTodo();
    }
}
