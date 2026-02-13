using Microsoft.EntityFrameworkCore;
using Proyecto.Datos.Repositories.Interfaces;
using Proyecto.Models;
using Proyecto.Negocio.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Proyecto.Negocio.Services
{
        public class UbigeoService : IUbigeoService
        {
            private readonly IGenericRepository<Departamento> _depaRepo;
            private readonly IGenericRepository<Provincia> _provRepo;
            private readonly IGenericRepository<Distrito> _distRepo;

            public UbigeoService(
                IGenericRepository<Departamento> depaRepo,
                IGenericRepository<Provincia> provRepo,
                IGenericRepository<Distrito> distRepo)
            {
                _depaRepo = depaRepo;
                _provRepo = provRepo;
                _distRepo = distRepo;
            }

            public async Task<List<SelectListItem>> ObtenerDepartamentos()
            {
                var query = await _depaRepo.obtenerTodo();
                return await query
                    //aca usamos OrderBy para listar deacuerdo alos nombres
                    .OrderBy(d => d.Nombre)  
                    .Select(d => new SelectListItem
                    {
                        Value = d.IdDepartamento.ToString(),
                        Text = d.Nombre
                    }).ToListAsync();
            }

            public async Task<List<SelectListItem>> ObtenerProvinciasPorDepartamento(int idDepartamento)
            {
                var query = await _provRepo.obtenerTodo();
                return await query
                    //aca usamos OrderBy para listar deacuerdo alos nombres
                    .Where(p => p.IdDepartamento == idDepartamento)
                    .OrderBy(p => p.Nombre) 
                    .Select(p => new SelectListItem
                    {
                        Value = p.IdProvincia.ToString(),
                        Text = p.Nombre
                    }).ToListAsync();
            }

            public async Task<List<SelectListItem>> ObtenerDistritosPorProvincia(int idProvincia)
            {
                var query = await _distRepo.obtenerTodo();
                return await query
                    //aca usamos OrderBy para listar deacuerdo alos nombres
                    .Where(d => d.IdProvincia == idProvincia)
                    .OrderBy(d => d.Nombre) 
                    .Select(d => new SelectListItem
                    {
                        Value = d.IdDistrito.ToString(),
                        Text = d.Nombre
                    }).ToListAsync();
            }

            public List<SelectListItem> ObtenerTiposDocumento()
            {
                return new List<SelectListItem>
            {
                new SelectListItem { Value = "DNI", Text = "DNI" },
                new SelectListItem { Value = "CE", Text = "Carnet de Extranjería" },
                new SelectListItem { Value = "PASAPORTE", Text = "Pasaporte" }
            };
            }
        }
    }
