function inicializarEventosUbigeo() {
    const selectores = {
        dep: $('#cboDepartamento'),
        prov: $('#cboProvincia'),
        dist: $('#cboDistrito')
    };

    // Aca capturamos el evento en caso se seleccione el un un item del combobox de departamento
    selectores.dep.on('change', function () {
        const depId = $(this).val();

        // Lo Primero que haremos sera limpiar los selectores vinculados
        selectores.prov.html('<option value="">-- Seleccione Provincia --</option>').prop('disabled', true);
        selectores.dist.html('<option value="">-- Seleccione Distrito --</option>').prop('disabled', true);

        if (depId) {
            fetch(`/Trabajador/obtenerProvincias?idDepartamento=${depId}`)
                .then(res => res.json())
                .then(data => {
                    if (data && data.length > 0) {
                        data.forEach(p => {
                            // Cargamos los nombres de las provincias que retorna el metodo en formato json
                            selectores.prov.append(`<option value="${p.id}">${p.nombre}</option>`);
                        });
                        selectores.prov.prop('disabled', false);
                    }
                })
                .catch(err => console.error("Error en provincias:", err));
        }
    });

    // aca haremos lo mismo que en departamento pero para provincias 
    selectores.prov.on('change', function () {
        const provId = $(this).val();

        // Limpiaremos distrito 
        selectores.dist.html('<option value="">-- Seleccione Distrito --</option>').prop('disabled', true);

        if (provId) {
            fetch(`/Trabajador/obtenerDistritos?idProvincia=${provId}`)
                .then(res => res.json())
                .then(data => {
                    if (data && data.length > 0) {
                        data.forEach(d => {
                            // Cargamos los nombres de los distritos que retorna el metodo en formato json
                            selectores.dist.append(`<option value="${d.id}">${d.nombre}</option>`);
                        });
                        selectores.dist.prop('disabled', false);
                    }
                })
                .catch(err => console.error("Error en distritos:", err));
        }
    });
}