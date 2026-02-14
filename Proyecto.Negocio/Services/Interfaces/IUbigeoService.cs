using Proyecto.Models;
using Proyecto.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proyecto.Negocio.Services.Interfaces
{
    public interface IUbigeoService
    {
        Task<List<Departamento>> ObtenerDepartamentos();
        Task<List<Provincia>> ObtenerProvinciasPorDepartamento(int idDepartamento);
        Task<List<Distrito>> ObtenerDistritosPorProvincia(int idProvincia);
        List<string> ObtenerTiposDocumento();

        Task<DistritoVM> ObtenerDistritoPorId(int idDistrito);
    }
}
