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
    public class DepartamentoService : IDepartamentoService
    {
        private readonly IGenericRepository<Departamento> _repository;

        public DepartamentoService(IGenericRepository<Departamento> repository)
        {
            _repository = repository;
        }

        public async Task<bool> Insertar(Departamento modelo) => await _repository.Insertar(modelo);

        public async Task<bool> Actualizar(Departamento modelo) => await _repository.Actualizar(modelo);

        public async Task<bool> Eliminar(int id) => await _repository.Eliminar(id);

        public async Task<Departamento> ObtenerPorId(int id) => await _repository.ObtenerPorId(id);

        public async Task<IQueryable<Departamento>> obtenerTodo() => await _repository.obtenerTodo();
    }
}
