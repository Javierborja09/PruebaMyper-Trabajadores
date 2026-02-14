using Microsoft.EntityFrameworkCore;
using Proyecto.Datos.Repositories.Interfaces;
using Proyecto.Models;
using Proyecto.Models.ViewModels;
using Proyecto.Negocio.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Proyecto.Negocio.Services
{
    public class UbigeoService : IUbigeoService
    {
        private readonly IGenericRepository<Departamento> _depaRepo;
        private readonly IGenericRepository<Provincia> _provRepo;
        private readonly IDistritoRepository _distRepo;

        public UbigeoService(
            IGenericRepository<Departamento> depaRepo,
            IGenericRepository<Provincia> provRepo,
            IDistritoRepository distRepo)
        {
            _depaRepo = depaRepo;
            _provRepo = provRepo;
            _distRepo = distRepo;
        }

        public async Task<List<Departamento>> ObtenerDepartamentos()
        {
            IQueryable<Departamento> query = await _depaRepo.obtenerTodo();
            return await query
                .OrderBy(d => d.Nombre)
                .ToListAsync();
        }

        public async Task<List<Provincia>> ObtenerProvinciasPorDepartamento(int idDepartamento)
        {
            IQueryable<Provincia> query = await _provRepo.obtenerTodo();
            return await query
                .Where(p => p.IdDepartamento == idDepartamento)
                .OrderBy(p => p.Nombre)
                .ToListAsync();
        }

        public async Task<List<Distrito>> ObtenerDistritosPorProvincia(int idProvincia)
        {
            IQueryable<Distrito> query = await _distRepo.obtenerTodo();
            return await query
                .Where(d => d.IdProvincia == idProvincia)
                .OrderBy(d => d.Nombre)
                .ToListAsync();
        }

        public async Task<DistritoVM> ObtenerDistritoPorId(int idDistrito)
        {
            return await _distRepo.ObtenerDetalleCompleto(idDistrito);
        }

        public List<string> ObtenerTiposDocumento()
        {
            return new List<string> { "DNI", "CE", "PASAPORTE" };
        }
    }
}
