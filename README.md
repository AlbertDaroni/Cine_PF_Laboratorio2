# Cine_ProyectoFinal_Laboratorio2

1.  ## Narrativa del Proyecto y Descripción del Problema
    Datos del Grupo
    Integrantes:

* **Matias Martinez** - *matias.e.martinez1993@gmail.com* - (https://github.com/MatiasMartinez-22) - Discord: `matiasaitam2224188`
* **Alberto Daroni** - *albertodaroni@gmail.com* - (https://github.com/AlbertDaroni) - Discord: `white_shadow71717`
* **Jonatan Aguero** - *david.joni2401@gmail.com* - (https://github.com/davidjoni2401-sudo) - Discord: `jonatan`


  ## Título / Nombre del Proyecto
  * **Sistema Web de Gestión de Cine y Reservas**
  ## Temática elegida
  El proyecto consiste en el desarrollo de un sistema web para la gestión de un cine mediante una aplicación desarrollada con arquitectura MVC.
  El sistema permitirá administrar cines, salas, butacas, películas, géneros, funciones, usuarios, reservas y pagos. Además, contará con diferentes roles de acceso, permitiendo distinguir las acciones que puede realizar un administrador, un empleado y un usuario.
  ## Descripción de la Problemática
  La administración de un cine requiere manejar una gran cantidad de información relacionada con películas, salas, horarios de funciones, butacas, usuarios, reservas y pagos.
  Cuando esta información se administra de forma manual o mediante distintos sistemas separados, pueden producirse diferentes problemas, como reservas duplicadas, errores en la asignación de butacas, dificultad para conocer la disponibilidad de las salas, inconvenientes en la organización de las funciones y falta de control sobre los pagos realizados.
  También resulta necesario establecer distintos niveles de acceso dentro del sistema, ya que no todos los usuarios deben contar con los mismos permisos. Por ejemplo, un administrador debe poder gestionar la información general del sistema, mientras que un usuario común debe tener acceso principalmente a la consulta de películas, funciones y reservas.
  Por este motivo, se propone desarrollar un sistema web que centralice toda la información necesaria para el funcionamiento del cine y facilite su administración.
  ## Propuesta de Solución
  Se desarrollará una aplicación web utilizando el patrón MVC (Modelo-Vista-Controlador) para gestionar las distintas operaciones relacionadas con el funcionamiento de un cine.
  El sistema contará con tres roles principales:
* *- Administrador:* tendrá acceso a la gestión general del sistema, incluyendo usuarios, roles, cines, salas, butacas, películas, géneros, funciones, reservas y pagos.
* *- Empleado:* podrá realizar tareas relacionadas con la gestión diaria del cine, como consultar funciones, administrar reservas, verificar disponibilidad de butacas y registrar pagos.
* *- Usuario:* podrá consultar las películas y funciones disponibles, seleccionar una butaca y realizar una reserva.
  El sistema permitirá registrar películas y clasificarlas según su género, organizar las salas pertenecientes a cada cine y administrar las butacas disponibles en cada una.
  También permitirá crear funciones, indicando qué película se proyectará, en qué sala, qué día y a qué horario.
  A partir de esas funciones, los usuarios podrán realizar reservas de butacas y registrar el pago correspondiente.
  De esta manera, la aplicación permitirá centralizar la información del cine, reducir errores, mejorar la organización de las funciones y facilitar tanto la gestión interna como la experiencia de los usuarios.

2. ## Modelo de Datos y Diagrama Entidad-Relación
   # Entidades principales
   * **Usuario**
   Representa a las personas que utilizan el sistema. Contendrá los datos necesarios para identificar al usuario y permitir su acceso a la aplicación.
   Cada usuario tendrá asignado un rol.
   * **Rol**
   Representa los diferentes tipos de acceso dentro del sistema.
   Los roles principales serán:

- Administrador
- Empleado
- Usuario
  El rol determinará qué acciones puede realizar cada usuario.
  * **Cine**
  Representa cada establecimiento o sucursal registrada en el sistema.
  Un cine podrá contar con varias salas.
  * **Sala**
  Representa cada sala de proyección perteneciente a un cine.
  Cada sala tendrá una determinada capacidad y estará compuesta por diferentes butacas.
  * **Butaca**
  Representa cada asiento disponible dentro de una sala.
  Cada butaca pertenecerá a una sala determinada y podrá identificarse mediante fila y número.
  * **Película**
  Almacena la información correspondiente a las películas disponibles.
  Puede incluir datos como:
- Título
- Descripción
- Duración
- Clasificación
- Imagen
- Género
 * **Género**
  Permite clasificar las películas según su tipo.
  Por ejemplo:
- Acción
- Comedia
- Terror
- Drama
- Ciencia ficción
- Animación
  Un género podrá estar asociado a varias películas.
  * **Función**
  Representa una proyección de una película en una sala, en una fecha y horario determinados.
  Esta entidad permitirá organizar la cartelera del cine.
  Una función deberá estar relacionada con:
- Una película
- Una sala
- Una fecha
- Un horario
  De esta manera, una misma película podrá tener varias funciones en distintos días, horarios o salas.
  * **Reserva**
  Representa la reserva realizada por un usuario para asistir a una función determinada.
  La reserva estará relacionada con:
- Un usuario
- Una función
- Una butaca
  Esto permitirá controlar qué butacas ya fueron reservadas para cada función.
  * **Pago**
  Representa el registro del pago correspondiente a una reserva.
  Podrá almacenar información como:
- Importe
- Fecha del pago
- Medio de pago
- Estado del pago
  Cada pago estará asociado a una reserva.

3. ## Descripción de las Relaciones
   Las relaciones principales del sistema serán:

- Un Rol puede estar asignado a varios Usuarios, pero cada usuario tendrá un solo rol.
- Un Cine puede tener varias Salas, pero cada sala pertenecerá a un solo cine.
- Una Sala puede tener varias Butacas, pero cada butaca pertenecerá a una única sala.
- Un Género puede estar asociado a varias Películas, pero cada película tendrá un género.
- Una Película puede tener varias Funciones.
- Una Sala puede tener varias Funciones en distintos días y horarios.
- Cada Función corresponderá a una película y a una sala determinada.
- Un Usuario puede realizar varias Reservas.
- Una Función puede tener varias Reservas.
- Una Butaca puede aparecer en diferentes reservas siempre que correspondan a funciones distintas.
- Cada Reserva estará asociada a un usuario, una función y una butaca.
- Una Reserva podrá tener asociado un Pago.

4. ## Resumen de Roles, Entidades y Vistas
   Roles:
   Administrador, Empleado y Usuario.
   Entidades / Tablas:
   Usuario, Rol, Cine, Sala, Butaca, Película, Género, Función, Reserva y Pago.
   Vistas principales:
   Inicio, Usuarios, Roles, Cines, Salas, Butacas, Películas, Géneros, Funciones, Reservas y Pagos.
   Cada módulo contará con las vistas necesarias para consultar, crear, editar y eliminar registros de acuerdo con los permisos de cada rol.

5. ## DER -->
   <img width="869" height="1600" alt="DiagrameEntidadyRelaciondeCine" src="https://github.com/user-attachments/assets/c6857062-4598-4840-8fcb-ae7b4328d123" />
