using Proyecto.Datos.Repositories.Interfaces;
using Proyecto.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proyecto.Negocio.Services.Interfaces
{
    public class ProvinciaService : IProvinciaService
    {
        private readonly IGenericRepository<Provincia> _repository;

        public ProvinciaService(IGenericRepository<Provincia> repository)
        {
            _repository = repository;
        }

        public async Task<bool> Insertar(Provincia modelo) => await _repository.Insertar(modelo);

        public async Task<bool> Actualizar(Provincia modelo) => await _repository.Actualizar(modelo);

        public async Task<bool> Eliminar(int id) => await _repository.Eliminar(id);

        public async Task<Provincia> ObtenerPorId(int id) => await _repository.ObtenerPorId(id);

        public async Task<IQueryable<Provincia>> obtenerTodo() => await _repository.obtenerTodo();
    }
}
