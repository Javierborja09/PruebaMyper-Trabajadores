// Función para abrir el modal de Registro
function abrirModalRegistro() {
    fetch('/Trabajador/Crear')
        .then(response => {
            if (!response.ok) throw new Error("Error al cargar el formulario");
            return response.text();
        })
        .then(html => {
            document.getElementById('modalContainer').innerHTML = html;
            inicializarEventosUbigeo();

            const modal = new bootstrap.Modal(document.getElementById('modalTrabajador'));
            modal.show();
        })
        .catch(err => {
            Swal.fire({
                title: 'Error',
                text: 'No se pudo cargar el formulario de registro. Inténtelo más tarde.',
                icon: 'error',
                confirmButtonColor: '#d33'
            });
        });
}

// Función para abrir el modal de Edicion
function abrirModalEdicion(id) {
    fetch(`/Trabajador/Editar/${id}`)
        .then(response => {
            if (!response.ok) throw new Error("Error al cargar los datos del trabajador");
            return response.text();
        })
        .then(html => {
            document.getElementById('modalContainer').innerHTML = html;
            inicializarEventosUbigeo();

            const modal = new bootstrap.Modal(document.getElementById('modalTrabajador'));
            modal.show();
        })
        .catch(err => {
            Swal.fire({
                title: 'Error',
                text: 'Hubo un problema al obtener los datos del trabajador.',
                icon: 'error',
                confirmButtonColor: '#d33'
            });
        });
}

// Función para eliminar
function confirmarEliminacionTrabajador(id) {
    Swal.fire({
        title: '¿Estás seguro?',
        text: "El registro del trabajador será eliminado permanentemente.",
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#d33',
        cancelButtonColor: '#6c757d',
        confirmButtonText: '<i class="bi bi-trash-fill"></i> Sí, eliminar',
        cancelButtonText: 'Cancelar',
        reverseButtons: true
    }).then((result) => {
        if (result.isConfirmed) {

            // Creamos un FormData para enviar el ID y el Token de validación
            const data = new FormData();
            data.append("id", id);
            // Capturamos el token generado por ASP.NET en la vista
            data.append("__RequestVerificationToken", document.querySelector('input[name="__RequestVerificationToken"]').value);

            // Realizamos la petición POST al controlador
            fetch(`/Trabajador/Eliminar`, {
                method: 'POST',
                body: data
            })
                .then(response => {
                    if (response.ok) {
                        Swal.fire({
                            title: '¡Eliminado!',
                            text: 'El registro ha sido borrado con éxito.',
                            icon: 'success',
                            timer: 1500,
                            showConfirmButton: false
                        }).then(() => {
                            location.reload(); // Recargamos para ver los cambios
                        });
                    } else {
                        throw new Error("Error en el servidor");
                    }
                })
                .catch(err => {
                    Swal.fire('Error', 'No se pudo eliminar el trabajador.', 'error');
                });
        }
    });
}