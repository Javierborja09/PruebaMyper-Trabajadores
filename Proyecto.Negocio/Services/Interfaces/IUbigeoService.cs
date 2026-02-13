using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Proyecto.Negocio.Services.Interfaces
{
    public interface IUbigeoService
    {
        List<SelectListItem> ObtenerTiposDocumento();
        Task<List<SelectListItem>> ObtenerDepartamentos();
        Task<List<SelectListItem>> ObtenerProvinciasPorDepartamento(int idDepartamento);
        Task<List<SelectListItem>> ObtenerDistritosPorProvincia(int idProvincia);
    }
}
