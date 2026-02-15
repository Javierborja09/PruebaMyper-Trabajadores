using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Proyecto.Models;
using Proyecto.Models.ViewModels;
using Proyecto.Negocio.Services.Interfaces;

public class TrabajadorController : Controller
{
    // Primero cargaremos las variables de inyección de servicios para la lógica de trabajadores, ubigeo 
    // y acceso a carpetas físicas para las imágenes que habrá.
    // En este caso aplicamos private porque solo las usaremos en este controlador, no en clases externas.
    private readonly ITrabajadorService _trabajadorService;
    private readonly IUbigeoService _ubigeoService;
    private readonly IWebHostEnvironment _env;

    // Recibimos e inicializamos los servicios necesarios para el controlador.
    public TrabajadorController(
        ITrabajadorService trabajadoreService,
        IUbigeoService ubigeoService,
        IWebHostEnvironment env)
    {
        // Asignamos las dependencias inyectadas a nuestras variables globales.
        _trabajadorService = trabajadoreService;
        _ubigeoService = ubigeoService;
        _env = env;
    }

    // Esto será el motor del controlador, ya que lo que hace es recibir el sexo que le pasaré por parámetro,
    // ya que en mi index.js, al seleccionar el combobox en el filtro, mandará a este método recargando la página.
    // Seguidamente se filtrará; en caso sea null, retornará la lista de todos los trabajadores.
    // En caso no lo sea, verificará el sexo y en base a eso hará la consulta al método del service.
    // Vale recalcar que tanto listar todos como listar por sexo usan el mismo procedimiento; podría haber creado
    // un método solo en el service que sirva para estas 2 funciones, pero como quiero que se tenga orden y entendimiento,
    // lo decidí hacer así.
    public async Task<IActionResult> Index(string sexo)
    {
        List<TrabajadorVM> trabajadores;
        if (string.IsNullOrEmpty(sexo))
            trabajadores = await _trabajadorService.ListarTodos();
        else
            trabajadores = await _trabajadorService.ListarPorSexo(char.Parse(sexo));

        // Aquí retornamos a la vista un ViewBag con el sexo seleccionado para el filtro, seguido de los trabajadores. 
        // Usamos un ViewModel para evitar exponer nuestro Entity Framework y mostrar solo lo que nosotros queremos.
        ViewBag.SexoSeleccionado = sexo;
        return View(trabajadores);
    }

    // Ahora empezaremos con los métodos que contienen la lógica del sistema para eliminar, crear y editar trabajadores.

    // En este caso, al método le pondremos el tipo [HttpGet] ya que, como tal, nos retornará una vista.
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

    // En este caso, al método le pondremos el tipo [HttpPost] ya que recibiremos datos del formulario.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Crear(Trabajadore modelo, IFormFile? fotoFile)
    {
        ModelState.Remove("FotoRuta");
        if (!ModelState.IsValid)
        {
            TempData["Mensaje"] = "Error: Faltan datos obligatorios o el formato es incorrecto.";
            TempData["TipoMensaje"] = "danger";
            return RedirectToAction(nameof(Index));
        }

        // Validamos que no exista un trabajador con el mismo tipo y número de documento.
        if (await _trabajadorService.ExisteDocumento(modelo.TipoDocumento, modelo.NumeroDocumento))
        {
            // Enviamos el error por TempData y regresamos al Index.
            TempData["Mensaje"] = $"Error: Ya existe un trabajador con {modelo.TipoDocumento}: {modelo.NumeroDocumento}";
            TempData["TipoMensaje"] = "danger";

            return RedirectToAction(nameof(Index));
        }

        // Verificamos si la foto es diferente a null o su tamaño es mayor a 0; esto significa que el usuario 
        // sí insertó una foto y la almacenamos en la carpeta uploads.
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

        // Seguidamente llamamos al Insertar que tenemos en nuestro service para registrar al trabajador.
        await _trabajadorService.Insertar(modelo);

        // Establecemos el mensaje de éxito en TempData para mostrarlo en el Index.
        TempData["Mensaje"] = "Trabajador registrado exitosamente";
        TempData["TipoMensaje"] = "success";

        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public async Task<IActionResult> Editar(int id)
    {
        // Primeramente buscaremos al trabajador; en caso de que no exista, retornamos un error.
        var modelo = await _trabajadorService.ObtenerPorId(id);
        if (modelo == null) return NotFound();

        // Aquí es donde usamos el método que creamos en el UbigeoService (antes DistritoService).
        var ubigeoCompleto = await _ubigeoService.ObtenerDistritoPorId(modelo.IdDistrito ?? 0);

        int idProvActual = ubigeoCompleto.IdProvincia;
        int idDepaActual = ubigeoCompleto.IdDepartamento;

        // Llenamos los combos de acuerdo a la información del trabajador.
        ViewBag.TiposDocumento = _ubigeoService.ObtenerTiposDocumento()
            .Select(t => new SelectListItem { Value = t, Text = t, Selected = t == modelo.TipoDocumento });

        // Departamentos: Todos, pero marcamos el que reconstruimos.
        var departamentos = await _ubigeoService.ObtenerDepartamentos();
        ViewBag.IdDepartamento = new SelectList(departamentos, "IdDepartamento", "Nombre", idDepaActual);

        // Provincias: Solo las que pertenecen al departamento.
        var provincias = await _ubigeoService.ObtenerProvinciasPorDepartamento(idDepaActual);
        ViewBag.IdProvincia = new SelectList(provincias, "IdProvincia", "Nombre", idProvActual);

        // Distritos: Solo los que pertenecen a la provincia.
        var distritos = await _ubigeoService.ObtenerDistritosPorProvincia(idProvActual);
        ViewBag.IdDistrito = new SelectList(distritos, "IdDistrito", "Nombre", modelo.IdDistrito);

        return PartialView("_Edit", modelo);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Editar(Trabajadore modelo, IFormFile? fotoFile)
    {
        ModelState.Remove("FotoRuta");
        if (!ModelState.IsValid)
        {
            TempData["Mensaje"] = "Error: Faltan datos obligatorios o el formato es incorrecto.";
            TempData["TipoMensaje"] = "danger";
            return RedirectToAction(nameof(Index));
        }

        var trabajadorExistente = await _trabajadorService.ObtenerPorId(modelo.IdTrabajador);
        if (trabajadorExistente == null) return NotFound();

        if (await _trabajadorService.ExisteDocumento(modelo.TipoDocumento, modelo.NumeroDocumento)
            && (trabajadorExistente.NumeroDocumento != modelo.NumeroDocumento || trabajadorExistente.TipoDocumento != modelo.TipoDocumento))
        {
            TempData["Mensaje"] = $"Ya existe otro trabajador con {modelo.TipoDocumento}: {modelo.NumeroDocumento}";
            TempData["TipoMensaje"] = "error";

            return RedirectToAction(nameof(Index));
        }

        if (fotoFile != null && fotoFile.Length > 0)
        {
            // Eliminamos la foto anterior físicamente para no llenar el servidor.
            EliminarArchivoFoto(trabajadorExistente.FotoRuta);

            string nombreArchivo = Guid.NewGuid().ToString() + Path.GetExtension(fotoFile.FileName);
            string rutaCarga = Path.Combine(_env.WebRootPath, "uploads", nombreArchivo);

            using (var stream = new FileStream(rutaCarga, FileMode.Create))
            {
                await fotoFile.CopyToAsync(stream);
            }
            modelo.FotoRuta = nombreArchivo;
        }
        else
        {
            modelo.FotoRuta = trabajadorExistente.FotoRuta;
        }

        await _trabajadorService.Actualizar(modelo);

        TempData["Mensaje"] = "Trabajador actualizado correctamente";
        TempData["TipoMensaje"] = "success";

        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public async Task<JsonResult> ObtenerProvincias(int idDepartamento)
    {
        // Llamamos al método de nuestro service pasándole como parámetro el idDepartamento.
        var provincias = await _ubigeoService.ObtenerProvinciasPorDepartamento(idDepartamento);

        var resultado = provincias.Select(p => new
        {
            id = p.IdProvincia,
            nombre = p.Nombre
        });
        return Json(resultado);
    }

    [HttpGet]
    public async Task<JsonResult> ObtenerDistritos(int idProvincia)
    {
        var distritos = await _ubigeoService.ObtenerDistritosPorProvincia(idProvincia);
        var resultado = distritos.Select(d => new
        {
            id = d.IdDistrito,
            nombre = d.Nombre
        });
        return Json(resultado);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Eliminar(int id)
    {
        // En esta ocasión decido obtener el ID ya que de esa manera podré obtener la ruta 
        // de la foto del trabajador, en caso de que tenga una, para eliminarla. También podría pasarla 
        // por parámetro captándola desde el index, pero para mantener el código limpio decido hacerlo así.
        // Si se ejecuta la eliminación, indicamos Ok(), que hace referencia a que se ejecutó correctamente.
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

    // Este método, como tal, es de utilidad. Podríamos tenerlo en otra clase, pero para facilitar 
    // el entendimiento decidí ponerlo en el controller. Sé que no es buena práctica tenerlo aquí, 
    // pero lo hice así por tratarse de una prueba técnica; en producción se aplicaría desde una clase de utilidades.
    private void EliminarArchivoFoto(string nombreFoto)
    {
        // Aquí verificamos que no sea la imagen por defecto para no borrarla.
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