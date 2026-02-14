using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.DotNet.Scaffolding.Shared.CodeModifier.CodeChange;
using Proyecto.Models;
using Proyecto.Models.ViewModels;
using Proyecto.Negocio.Services.Interfaces;

namespace Proyecto.Web.Controllers
{
    public class TrabajadorController : Controller
    {

        // Primero cargaremos las variables de inyección de servicios para la logica de trabajadores, ubigeo y acceso a carpetas fisicas para las imagenes que abra
        // en este caso aplicamos private por que solo la usaremos en este controllador no con clases externas
        private readonly ITrabajadorService _trabajadorService;
        private readonly IUbigeoService _ubigeoService;
        private readonly IWebHostEnvironment _env;


        // recibimos e inicializamos los servicios necesarios para el controlador
        public TrabajadorController(
            ITrabajadorService trabajadoreService,
            IUbigeoService ubigeoService,
            IWebHostEnvironment env)
        {
            // Asignamos las dependencias inyectadas a nuestras variables globales
            _trabajadorService = trabajadoreService;
            _ubigeoService = ubigeoService;
            _env = env;
        }


        // Esto sera el motor del controlador ya que lo que hace es recibir el sexo que le pasare por parametro 
        // ya que en mi index.js al selecionar el combobox en el filtro mandara a este metodo recargando la pagina
        // y seguidamente se filtrara en caso sea null retornara la lista de todos los trabajadores 
        // en caso no lo sea verificara el sexo y en base a eso hara la consulta al metodo del service
        // vale recalcar que tanto listar todos y listar por sexo usan el mismo procedimiento ya que podria haber creado
        // un metodo solo en el service que sirva para estas 2 funcioones pero como quiero que se tenga orden y entendimiento
        // lo decidi hacer asi
        public async Task<IActionResult> Index(string sexo)
        {
            List<TrabajadorVM> trabajadores;
            if (string.IsNullOrEmpty(sexo))
                trabajadores = await _trabajadorService.ListarTodos();
            else
                trabajadores = await _trabajadorService.ListarPorSexo(char.Parse(sexo));

            // aca retornamos ala vista un viewBag  con el sexo selecionado para el filtro seguidamere de los trabajadores que usamos un viewmodel para evitar exponer
            // nuestro entityframework y mostrar solo lo nosotros queremos

            ViewBag.SexoSeleccionado = sexo;
            return View(trabajadores);
        }

        // Ahora empezaremos con los metodos que son la logica del sistema para eliminar, crear, editar trabajadores


        // en este caso  al metodo le podremos tipo  [HttpGet] ya que como tal nos retornara una vista
        [HttpGet]
        public async Task<IActionResult> Crear()
        {
            // Mapeamos los Tipos de Documento (List<string> -> SelectListItem)
            ViewBag.TiposDocumento = _ubigeoService.ObtenerTiposDocumento()
                .Select(t => new SelectListItem { Value = t, Text = t });

            // Mapeamos los Departamentos (List<Departamento> -> SelectListItem)
            var departamentos = await _ubigeoService.ObtenerDepartamentos();
            ViewBag.Departamentos = departamentos.Select(d => new SelectListItem
            {
                Value = d.IdDepartamento.ToString(),
                Text = d.Nombre
            });

            return PartialView("_Create", new Trabajadore());
        }

        // En este caso  al metodo le podremos tipo  [HttpPost] ya que recibiremos datos del usuario
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Crear(Trabajadore modelo, IFormFile fotoFile)
        {
            //Verificamos en caso foto sea diferente a null o sea mayor a 0 significa que el usuario si inserto una foto y la almacenamos en le carpeta uploads
            if (fotoFile != null && fotoFile.Length > 0)
            {
                string nombreArchivo = Guid.NewGuid().ToString() + Path.GetExtension(fotoFile.FileName);
                string rutaCarga = Path.Combine(_env.WebRootPath, "uploads", nombreArchivo);

                using (var stream = new FileStream(rutaCarga, FileMode.Create))
                {
                    await fotoFile.CopyToAsync(stream);
                }
                modelo.FotoRuta = nombreArchivo;
            }

            //Seguidamente llamamos al Insertar que tenemos en nuestro service para registrar al trabajador
            await _trabajadorService.Insertar(modelo);

            // y redirigimos a la vista Principal
            return RedirectToAction(nameof(Index));
        }


        // en este caso  al metodo le podremos tipo  [HttpGet] ya que como tal nos retornara una vista
        [HttpGet]
        public async Task<IActionResult> Editar(int id)
        {
            // Primeramente Buscaremos al trabajador en Caso No exista Retornamos error
            var modelo = await _trabajadorService.ObtenerPorId(id);
            if (modelo == null) return NotFound();

            // Aquí es donde usamos el metodo que creamos en el DistritoService
            var ubigeoCompleto = await _ubigeoService.ObtenerDistritoPorId(modelo.IdDistrito ?? 0);

            int idProvActual = ubigeoCompleto.IdProvincia;
            int idDepaActual = ubigeoCompleto.IdDepartamento;

            // llenamos los combos deacuerdo ala informcion del trabajador
            ViewBag.TiposDocumento = _ubigeoService.ObtenerTiposDocumento()
                .Select(t => new SelectListItem { Value = t, Text = t, Selected = t == modelo.TipoDocumento });

            // Departamentos: Todos, pero marcamos el que reconstruimos
            var departamentos = await _ubigeoService.ObtenerDepartamentos();
            ViewBag.IdDepartamento = new SelectList(departamentos, "IdDepartamento", "Nombre", idDepaActual);

            // Provincias: Solo las que pertenecen al departamento 
            var provincias = await _ubigeoService.ObtenerProvinciasPorDepartamento(idDepaActual);
            ViewBag.IdProvincia = new SelectList(provincias, "IdProvincia", "Nombre", idProvActual);

            // Distritos: Solo los que pertenecen a la provincia 
            var distritos = await _ubigeoService.ObtenerDistritosPorProvincia(idProvActual);
            ViewBag.IdDistrito = new SelectList(distritos, "IdDistrito", "Nombre", modelo.IdDistrito);

            return PartialView("_Edit", modelo);
        }

        // En este caso  al metodo le podremos tipo  [HttpPost] ya que recibiremos datos del usuario
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Editar(Trabajadore modelo, IFormFile fotoFile)
        {

            // verificamos si el trabajador existe obteniendolo del metodo del service en caso no exista retornamos un error 
            var trabajadorExistente = await _trabajadorService.ObtenerPorId(modelo.IdTrabajador);
            if (trabajadorExistente == null) return NotFound();


            //aca hacemos lo mismo que el crear pero con algo adicional ya que llamamos al metodo que creamos EliminarArchivoFoto para que se elimine la foto antigua
            // y se ponga la foto nueva para evitar consumir almacenamiento
            if (fotoFile != null && fotoFile.Length > 0)
            {
                EliminarArchivoFoto(trabajadorExistente.FotoRuta);
                string nombreArchivo = Guid.NewGuid().ToString() + Path.GetExtension(fotoFile.FileName);
                string rutaCarga = Path.Combine(_env.WebRootPath, "uploads", nombreArchivo);

                using (var stream = new FileStream(rutaCarga, FileMode.Create))
                {
                    await fotoFile.CopyToAsync(stream);
                }
                // y restablecemos la ruta del archivo 
                modelo.FotoRuta = nombreArchivo;
            }

            //Seguidamente llamamos al Actualizar que tenemos en nuestro service para actualizar al trabajador
            await _trabajadorService.Actualizar(modelo);

            // redirigimos ala vista principal
            return RedirectToAction(nameof(Index));
        }


        // en este caso al metodo le podremos tipo [HttpGet] ya que como tal nos retornara en este caso un json con los datos 
        // aca podriamos ponerle filtros como autorizacion  para no poder acceder a esta ruta de forma facil
        // // pero como esto es una prueba no lo pusimos
        [HttpGet]
        public async Task<JsonResult> ObtenerProvincias(int idDepartamento)
        {
            // aca llamamos al metodo de nuestro service pasandole como parametro el idDepartamento
            var provincias = await _ubigeoService.ObtenerProvinciasPorDepartamento(idDepartamento);

            // con el resultado que obtenemos lo mandamos
            var resultado = provincias.Select(p => new
            {
                id = p.IdProvincia,
                nombre = p.Nombre
            });
            return Json(resultado);
        }
        // aca usamos metodo tipo [HttpGet] igual que el ObtenerProvincias por que nos retornara un json
        [HttpGet]
        public async Task<JsonResult> ObtenerDistritos(int idProvincia)
        {
            // aca llamamos al metodo de nuestro service pasandole como parametro el idProvincia
            var distritos = await _ubigeoService.ObtenerDistritosPorProvincia(idProvincia);
            // con el resultado que obtenemos lo mandamos
            var resultado = distritos.Select(d => new
            {
                id = d.IdDistrito,
                nombre = d.Nombre
            });
            return Json(resultado);
        }

        // aca usamos metodo tipo [HttpPost]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Eliminar(int id)
        {
            // primeramente en este ocasion decido obtener el id ya que de esa manera podre obtener la ruta
            //  de la foto del trabajador en caso tenga para eliminarla  tmb podria pasala por parametro
            // captandola del index la url de la foto pero para mantener el codigo limpio decido hacerlo haci 
            // y si se ejecuta la eliminacion indicamos ok que ahce referencia a que si se ejecuto correctamente
            var trabajador = await _trabajadorService.ObtenerPorId(id);
            if (trabajador != null)
            {
                bool eliminado = await _trabajadorService.Eliminar(id);

                if (eliminado)
                {
                    EliminarArchivoFoto(trabajador.FotoRuta);
                    return Ok();
                }
            }

            return BadRequest();
        }

        // este metodo como tal es de utilidad podriamos tenerlo en otra clase pero para dar antendimiento lo decidi ponerlo en el controller 
        // se que no es buena practica tenerlo aca pero  decidi hacerlo asi ya que es una prueba tecnica, en produccion se aplicaria desde una 
        // clase de utilidades
        private void EliminarArchivoFoto(string nombreFoto)
        {
            // aca verificamos que no sea la imagen default para que no la borre
            if (!string.IsNullOrEmpty(nombreFoto) && nombreFoto != "trabajador.webp")
            {
                string rutaArchivo = Path.Combine(_env.WebRootPath, "uploads", nombreFoto);
                if (System.IO.File.Exists(rutaArchivo))
                {
                    System.IO.File.Delete(rutaArchivo);
                }
            }
        }
    }


}