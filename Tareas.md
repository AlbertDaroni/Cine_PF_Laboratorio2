### TAREAS PENDIENTES
1. **Archivo README.md obligatorio que contenga**:
- Descripción general del proyecto y el problema que resuelve.
Matriz de Evidencias de Requerimientos Mínimos *(Tabla que certifique el cumplimiento de cada punto)*.

2. **Estilos**:
- Dejar todo bonito.

3. **Edición de salas de un cine**:
- Mostrar cada sala de un cine en su vista "Detalles", y mostrar las butacas correspondientes de cada sala.
- Se debe poder agregar, quitar y modificar las salas de un cine.
- Se debe poder agregar, quitar y modificar las butacas de una sala.

4. **Actualización de estados**:
- Si la fecha y la hora de una función ya pasó, actualizar todas las tablas correspondientes *(Butacas, DetalleReserva, Reservas, Funciones)*.

<hr>

### TAREAS FINALIZADAS
1. **Modelo de Datos y Persistencia**:
- Al menos 6 clases/tablas principales relacionadas entre sí. 
- Debe incluir al menos una relación 1:N y una relación N:M, o una entidad que dependa de otras dos.

2. **Seguridad, Autenticación y Roles**:
- Login/Logout funcional.
- Seguridad en el backend y manejo de roles *(ej. Administrador, Empleado, Cliente)*.
- Al menos una funcionalidad relevante restringida exclusivamente por rol.
- Gestión de Avatar personalizado en el perfil de usuario.

3. **Manejo de Archivos**:
- Procesamiento y almacenamiento de archivos/imágenes en el servidor, adicional al avatar del usuario.

4. **Paginado por Servidor (Server-Side Pagination)**:
- Todos los listados principales deben contar con paginado servido desde el backend *(se debe solicitar y recibir únicamente la página actual por petición)*.
Quedan exceptuadas solo tablas maestras muy pequeñas.

5. **Detalles del pago y de la reserva**:
- Agregar un botón en la vista "Indice" de Reserva para mostrar los detalles completos de esa reserva:
    - Nombre del cine.
    - Número de sala.
    - Butaca/s reservada/s.
    - Fecha del pago.
    - Importe.
    - Estado del pago.
    - Estado de la reserva.
    - Estado de la función.
    - Hora de la función.
    - Datos de la película.

6. **Búsqueda Asíncrona de Entidades Relacionadas**:
- Al seleccionar entidades relacionadas en un formulario *(ej. seleccionar un Cliente al crear una Orden)*, debe hacerse mediante búsqueda
asíncrona vía AJAX en el servidor. Está prohibido traer todos los registros de la base de datos para llenar un `<select>` masivo.

7. **Web API REST con JWT**:
- Implementación de endpoints de Web API autenticados mediante JSON Web Tokens (JWT). Si la aplicación web no consume esta
API de manera nativa, se debe adjuntar la colección de pruebas correspondiente.