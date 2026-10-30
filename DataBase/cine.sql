-- 1. TABLAS INDEPENDIENTES
CREATE TABLE roles (
    id INT AUTO_INCREMENT PRIMARY KEY,
    nombre VARCHAR(255) NOT NULL
);

CREATE TABLE cines (
    id INT AUTO_INCREMENT PRIMARY KEY,
    nombre VARCHAR(255) NOT NULL,
    direccion VARCHAR(255) NOT NULL,
    abierto TINYINT(1) NOT NULL DEFAULT 0
);

CREATE TABLE generos (
    id INT AUTO_INCREMENT PRIMARY KEY,
    nombre VARCHAR(50) NOT NULL
);

CREATE TABLE peliculas (
    id INT AUTO_INCREMENT PRIMARY KEY,
    titulo VARCHAR(255) NOT NULL,
    portada TEXT NOT NULL,
    clasificacion VARCHAR(10) NOT NULL,
    idioma VARCHAR(255) NOT NULL,
    duracion INT NOT NULL,
    estado TINYINT(1) NOT NULL DEFAULT 1
);

CREATE TABLE directores (
    id INT AUTO_INCREMENT PRIMARY KEY,
    nombre VARCHAR(255) NOT NULL,
    apellido VARCHAR(255) NOT NULL,
    nacionalidad VARCHAR(100) NULL
);

-- 2. TABLAS CON LLAVES FORÁNEAS BÁSICAS
CREATE TABLE usuarios (
    id INT AUTO_INCREMENT PRIMARY KEY,
    nombre VARCHAR(50) NOT NULL,
    apellido VARCHAR(50) NOT NULL,
    correo VARCHAR(255) NOT NULL UNIQUE,
    dni INT NOT NULL UNIQUE,
    foto_perfil TEXT NULL,
    contraseña VARCHAR(255) NOT NULL,
    password_hasheada TINYINT(1) NOT NULL DEFAULT 0,
    estado TINYINT(1) NOT NULL DEFAULT 1,
    rol_id INT NOT NULL,
    FOREIGN KEY (rol_id) REFERENCES roles(id) ON DELETE RESTRICT ON UPDATE CASCADE
);

CREATE TABLE salas (
    id INT AUTO_INCREMENT PRIMARY KEY,
    numero INT NOT NULL,
    tipo VARCHAR(255) NOT NULL,
    capacidad INT NOT NULL,
    estado TINYINT(1) NOT NULL DEFAULT 1,
    cine_id INT NOT NULL,
    FOREIGN KEY (cine_id) REFERENCES cines(id) ON DELETE CASCADE ON UPDATE CASCADE
);

CREATE TABLE butacas (
    id INT AUTO_INCREMENT PRIMARY KEY,
    numero VARCHAR(3) NOT NULL,
    ocupado TINYINT(1) NOT NULL DEFAULT 0,
    sala_id INT NOT NULL,
    FOREIGN KEY (sala_id) REFERENCES salas(id) ON DELETE CASCADE ON UPDATE CASCADE
);

-- 3. TABLAS INTERMEDIAS Y ESTRUCTURAS COMPLEJAS
CREATE TABLE pelicula_genero (
    pelicula_id INT NOT NULL,
    genero_id INT NOT NULL,
    PRIMARY KEY (pelicula_id, genero_id),
    FOREIGN KEY (pelicula_id) REFERENCES peliculas(id) ON DELETE CASCADE,
    FOREIGN KEY (genero_id) REFERENCES generos(id) ON DELETE CASCADE
);

CREATE TABLE pelicula_director (
    pelicula_id INT NOT NULL,
    director_id INT NOT NULL,
    PRIMARY KEY (pelicula_id, director_id),
    FOREIGN KEY (pelicula_id) REFERENCES peliculas(id) ON DELETE CASCADE,
    FOREIGN KEY (director_id) REFERENCES directores(id) ON DELETE CASCADE
);

-- La tabla Funciones une Películas, Salas y Horarios específicos
CREATE TABLE funciones (
    id INT AUTO_INCREMENT PRIMARY KEY,
    fecha_hora DATETIME NOT NULL,
    precio DECIMAL(10,2) NOT NULL,
    estado TINYINT(1) NOT NULL DEFAULT 1,
    pelicula_id INT NOT NULL,
    sala_id INT NOT NULL,
    FOREIGN KEY (pelicula_id) REFERENCES peliculas(id) ON DELETE RESTRICT ON UPDATE CASCADE,
    FOREIGN KEY (sala_id) REFERENCES salas(id) ON DELETE RESTRICT ON UPDATE CASCADE
);

CREATE TABLE reservas (
    id INT AUTO_INCREMENT PRIMARY KEY,
    fecha_creacion DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
    monto DECIMAL(10,2) NOT NULL DEFAULT 0,
    estado TINYINT(1) NOT NULL DEFAULT 1,
    usuario_id INT NOT NULL,
    funcion_id INT NOT NULL,
    FOREIGN KEY (usuario_id) REFERENCES usuarios(id) ON DELETE RESTRICT ON UPDATE CASCADE,
    FOREIGN KEY (funcion_id) REFERENCES funciones(id) ON DELETE RESTRICT ON UPDATE CASCADE
);

-- Detalle_reserva rompe la relación N:N entre Reservas y Butacas (asientos reservados por transacción)
CREATE TABLE detalle_reserva (
    id INT AUTO_INCREMENT PRIMARY KEY,
    precio DECIMAL(10,2) NOT NULL,
    reserva_id INT NOT NULL,
    butaca_id INT NOT NULL,
    FOREIGN KEY (reserva_id) REFERENCES reservas(id) ON DELETE CASCADE,
    FOREIGN KEY (butaca_id) REFERENCES butacas(id) ON DELETE RESTRICT
);

CREATE TABLE pagos (
    id INT AUTO_INCREMENT PRIMARY KEY,
    concepto VARCHAR(50) NOT NULL,
    fecha_pago DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
    fecha_anulacion DATETIME NULL,
    importe DECIMAL(10,2) NOT NULL DEFAULT 0,
    estado TINYINT(1) NOT NULL DEFAULT 1,
    reserva_id INT NOT NULL UNIQUE,
    FOREIGN KEY (reserva_id) REFERENCES reservas(id) ON DELETE RESTRICT ON UPDATE CASCADE
);