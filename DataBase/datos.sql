-- Datos de prueba para Libre Cine

-- 1. ROLES
INSERT INTO roles (nombre) VALUES 
('Administrador'),
('Empleado'),
('Usuario');

-- 2. CINES
INSERT INTO cines (nombre, direccion, abierto) VALUES
('Libre Cine Central', 'Av. Corrientes 1234, CABA', 1),
('Libre Cine Unimev', 'Juana Koslay, San Luis', 1),
('Libre Cine Shopping', 'Av. San Martín 450, Mendoza', 1),
('Libre Cine Norte', 'Ruta 9 Km 200, Córdoba', 1),
('Libre Cine Monumental', 'Av. Corrientes 950, CABA', 1),
('Libre Cine Paseo Aldrey', 'Sarmiento 2685, Mar del Plata', 1),
('Libre Cine Solar de la Abadía', 'Arce 940, CABA', 1),
('Libre Cine Patio Olmos', 'Av. Vélez Sarsfield 361, Córdoba', 1),
('Libre Cine Palmares', 'Ruta Panamericana 2650, Mendoza', 1);

-- 3. GENEROS
INSERT INTO generos (nombre) VALUES
('Acción'),
('Animación'),
('Artes marciales'),
('Aventura'),
('Bélico'),
('Ciencia Ficción'),
('Comedia'),
('Crímen'),
('Cyberpunk'),
('Documental'),
('Drama'),
('Fantasía'),
('Histórico'),
('Musical'),
('Noir'),
('Oeste'),
('Postapocalíptico'),
('Psicológico'),
('Romance'),
('Superhéroes'),
('Suspenso'),
('Terror');

-- 4. DIRECTORES
INSERT INTO directores (nombre, apellido, nacionalidad) VALUES
('Christopher', 'Nolan', 'Británica'),
('Steven', 'Spielberg', 'Estadounidense'),
('Quentin', 'Tarantino', 'Estadounidense'),
('Martin', 'Scorsese', 'Estadounidense'),
('Guillermo', 'del Toro', 'Mexicana'),
('Denis', 'Villeneuve', 'Canadiense'),
('Hayao', 'Miyazaki', 'Japonesa'),
('James', 'Cameron', 'Canadiense'),
('Lana', 'Wachowski', 'Estadounidense'),
('Lilly', 'Wachowski', 'Estadounidense'),
('Greta', 'Gerwig', 'Estadounidense'),
('Tim', 'Burton', 'Estadounidense'),
('Juan José', 'Campanella', 'Argentina'),
('Jon', 'Watts', 'Estadounidense'),
('Santiago', 'Mitre', 'Argentina'),
('Bong', 'Joon-ho', 'Surcoreana'),
('Lee', 'Unkrich', 'Estadounidense'),
('Adrian', 'Molina', 'Estadounidense'),
('Damien', 'Chazelle', 'Estadounidense');

-- 5. PELICULAS
INSERT INTO peliculas (titulo, portada, clasificacion, idioma, duracion, estado) VALUES
('Inception', '/uploads/portada/inception.jpg', 'SAM13', 'Inglés Subtitulado', 148, 1),
('The Matrix', '/uploads/portada/the_matrix.jpg', 'SAM13', 'Español Latino', 136, 1),
('Interstellar', '/uploads/portada/interstellar.jpg', 'ATP', 'Inglés Subtitulado', 169, 1),
('Pulp Fiction', '/uploads/portada/pulp_fiction.jpg', 'SAM18', 'Inglés Subtitulado', 154, 1),
('Spirited Away', '/uploads/portada/spirited_away.jpg', 'ATP', 'Español Latino', 125, 1),
('Dune: Part Two', '/uploads/portada/dune_part_two.jpg', 'SAM13', 'Inglés Subtitulado', 166, 1),
('Avatar: The Way of Water', '/uploads/portada/avatar_the_way_of_water.jpg', 'ATP', 'Español Latino 3D', 192, 1),
('Oppenheimer', '/uploads/portada/oppenheimer.jpg', 'SAM16', 'Inglés Subtitulado', 180, 1),
('Barbie', '/uploads/portada/barbie.jpg', 'ATP', 'Español Latino', 114, 1),
('Beetlejuice Beetlejuice', '/uploads/portada/beetlejuice_beetlejuice.jpg', 'SAM13', 'Español Latino', 104, 1),
('El Secreto de sus Ojos', '/uploads/portada/el_secreto_de_sus_ojos.jpg', 'SAM13', 'Español', 129, 1),
('Spiderman: No Way Home', '/uploads/portada/spiderman_no_way_home.jpg', 'ATP', 'Castellano', 148, 1),
('Argentina, 1985', '/uploads/portada/argentina_1985.jpg', 'SAM13', 'Español', 140, 1),
('Parasite', '/uploads/portada/parasite.jpg', 'SAM16', 'Coreano Subtitulado', 132, 1),
('Coco', '/uploads/portada/coco.jpg', 'ATP', 'Castellano', 105, 1),
('The Dark Knight', '/uploads/portada/the_dark_knight.jpg', 'SAM13', 'Inglés Subtitulado', 152, 1),
('Whiplash', '/uploads/portada/whiplash.jpg', 'SAM16', 'Inglés Subtitulado', 107, 1);

-- 6. PELICULA_DIRECTOR (Muchos a Muchos)
INSERT INTO pelicula_director (pelicula_id, director_id) VALUES
(1, 1),
(2, 9),
(2, 10),
(3, 1),
(4, 3),
(5, 7),
(6, 6),
(7, 8),
(8, 1),
(9, 11),
(10, 12),
(11, 13),
(12, 14),
(13, 15),
(14, 16),
(15, 17),
(15, 18),
(16, 1),
(17, 19);

-- 7. PELICULA_GENERO
-- IDs según el orden de inserción de películas y géneros.

INSERT INTO pelicula_genero (pelicula_id, genero_id) VALUES
-- Inception: Acción, Ciencia ficción, Suspenso
(1, 1), (1, 6), (1, 21),

-- The Matrix: Acción, Ciencia ficción
(2, 1), (2, 6),

-- Interstellar: Aventura, Ciencia ficción, Drama
(3, 4), (3, 6), (3, 11),

-- Pulp Fiction: Crimen, Drama
(4, 8), (4, 11),

-- Spirited Away: Animación, Fantasía
(5, 2), (5, 12),

-- Dune: Part Two: Aventura, Ciencia ficción
(6, 4), (6, 6),

-- Avatar: The Way of Water: Aventura, Ciencia ficción
(7, 4), (7, 6),

-- Oppenheimer: Drama, Histórico
(8, 11), (8, 13),

-- Barbie: Comedia, Fantasía
(9, 7), (9, 12),

-- Beetlejuice Beetlejuice: Comedia, Fantasía
(10, 7), (10, 12),

-- El Secreto de sus Ojos: Crimen, Drama, Suspenso
(11, 8), (11, 11), (11, 21),

-- Spiderman: No Way Home: Acción, Ciencia ficción, Superhéroes
(12, 1), (12, 6), (12, 20),

-- Argentina, 1985: Drama, Histórico
(13, 11), (13, 13),

-- Parasite: Comedia, Drama, Suspenso
(14, 7), (14, 11), (14, 21),

-- Coco: Animación, Aventura, Fantasía
(15, 2), (15, 4), (15, 12),

-- The Dark Knight: Acción, Crimen, Superhéroes
(16, 1), (16, 8), (16, 20),

-- Whiplash: Drama, Musical
(17, 11), (17, 14);

-- 8. SALAS
INSERT INTO salas (numero, tipo, capacidad, estado, cine_id) VALUES
-- ==========================================
-- 1. Libre Cine Central (9 salas)
-- ==========================================
(1, '2D Atmos', 60, 1, 1),
(2, '3D Clásica', 45, 1, 1),
(3, 'IMAX', 120, 1, 1),
(4, '2D Digital', 80, 1, 1),
(5, '4XD Premium', 48, 1, 1),
(6, '2D Comfort', 90, 1, 1),
(7, '3D Turbo', 75, 1, 1),
(8, 'VIP Lounge', 30, 1, 1),
(9, '2D Standard', 110, 1, 1),
-- ==========================================
-- 2. Libre Cine Unimev (8 salas)
-- ==========================================
(1, '2D Digital', 50, 1, 2),
(2, '3D Premium', 40, 1, 2),
(3, '2D Comfort', 85, 1, 2),
(4, '3D Clásica', 60, 1, 2),
(5, '4XD Dynamic', 50, 1, 2),
(6, '2D Atmos', 100, 1, 2),
(7, 'Laser 4K', 120, 1, 2),
(8, '2D Standard', 150, 1, 2),
-- ==========================================
-- 3. Libre Cine Shopping (10 salas)
-- ==========================================
(1, 'Laser 4K', 80, 1, 3),
(2, 'IMAX', 250, 1, 3),
(3, '3D', 150, 1, 3),
(4, '2D', 120, 1, 3),
(5, '2D Comfort', 100, 1, 3),
(6, '3D Premium', 90, 1, 3),
(7, '4XD Premium', 64, 1, 3),
(8, 'VIP Lounge', 40, 1, 3),
(9, '2D Atmos', 130, 1, 3),
(10, '3D Turbo', 110, 1, 3),
-- ==========================================
-- 4. Libre Cine Norte (8 salas)
-- ==========================================
(1, '2D Comfort', 100, 1, 4),
(2, '3D Turbo', 130, 1, 4),
(3, 'IMAX', 180, 1, 4),
(4, 'Laser 4K', 110, 1, 4),
(5, '2D Atmos', 95, 1, 4),
(6, '3D Clásica', 70, 1, 4),
(7, 'VIP Luxury', 32, 1, 4),
(8, '2D Standard', 140, 1, 4),
-- ==========================================
-- 5. Libre Cine Monumental (11 salas)
-- ==========================================
(1, '2D Premium', 80, 1, 5),
(2, '3D Clásica', 65, 1, 5),
(3, 'IMAX Laser', 220, 1, 5),
(4, '2D Comfort', 105, 1, 5),
(5, '3D Premium', 95, 1, 5),
(6, '4XD Premium', 56, 1, 5),
(7, 'VIP Lounge', 38, 1, 5),
(8, '2D Atmos', 115, 1, 5),
(9, 'Laser 4K', 135, 1, 5),
(10, '2D Standard', 160, 1, 5),
-- ==========================================
-- 6. Libre Cine Paseo Aldrey (9 salas)
-- ==========================================
(1, 'Laser 4K', 95, 1, 6),
(2, 'IMAX', 210, 1, 6),
(3, '3D Premium', 110, 1, 6),
(4, '2D Comfort', 100, 1, 6),
(5, '2D Atmos', 125, 1, 6),
(6, '4XD Dynamic', 60, 1, 6),
(7, '3D Clásica', 80, 1, 6),
(8, 'VIP Lounge', 35, 1, 6),
(9, '2D Standard', 145, 1, 6),
-- ==========================================
-- 7. Libre Cine Solar de la Abadía (8 salas)
-- ==========================================
(1, '2D Comfort', 75, 1, 7),
(2, '3D Premium', 70, 1, 7),
(3, 'Laser 4K', 105, 1, 7),
(4, '2D Atmos', 90, 1, 7),
(5, '4XD Premium', 48, 1, 7),
(6, 'VIP Luxury', 28, 1, 7),
(7, '3D Clásica', 60, 1, 7),
(8, '2D Standard', 115, 1, 7),
-- ==========================================
-- 8. Libre Cine Patio Olmos (11 salas)
-- ==========================================
(1, 'IMAX Laser', 240, 1, 8),
(2, 'Laser 4K', 130, 1, 8),
(3, '2D Atmos', 140, 1, 8),
(4, '3D Premium', 100, 1, 8),
(5, '4XD Dynamic', 64, 1, 8),
(6, '2D Comfort', 110, 1, 8),
(7, '3D Clásica', 85, 1, 8),
(8, 'VIP Lounge', 42, 1, 8),
(9, '2D Standard', 170, 1, 8),
(10, '3D Turbo', 115, 1, 8),
-- ==========================================
-- 9. Libre Cine Palmares (11 salas)
-- ==========================================
(1, 'IMAX', 260, 1, 9),
(2, 'Laser 4K', 125, 1, 9),
(3, '2D Atmos', 150, 1, 9),
(4, '3D Premium', 105, 1, 9),
(5, '4XD Premium', 56, 1, 9),
(6, '2D Comfort', 115, 1, 9),
(7, '3D Clásica', 75, 1, 9),
(8, 'VIP Luxury', 36, 1, 9),
(9, '2D Standard', 180, 1, 9),
(10, '3D Turbo', 120, 1, 9);

-- 9. BUTACAS (Muestra reducida por Sala)
CREATE TEMPORARY TABLE digitos_butacas (
    n INT PRIMARY KEY
);

INSERT INTO digitos_butacas (n) VALUES
(0), (1), (2), (3), (4), (5), (6), (7), (8), (9);

INSERT INTO butacas (numero, ocupado, sala_id)
SELECT
    CONCAT(
        CHAR(65 + FLOOR(numeros.n / 14)),
        MOD(numeros.n, 14) + 1
    ),
    0,
    s.id
FROM salas AS s
CROSS JOIN (
    SELECT
        unidades.n + decenas.n * 10 + centenas.n * 100 AS n
    FROM digitos_butacas AS unidades
    CROSS JOIN digitos_butacas AS decenas
    CROSS JOIN digitos_butacas AS centenas
) AS numeros
WHERE numeros.n < s.capacidad
  AND numeros.n < 364
ORDER BY s.id, numeros.n;

DROP TEMPORARY TABLE digitos_butacas;

-- 10. USUARIOS
-- Cuentas de prueba con contraseñas almacenadas como hash.
INSERT INTO usuarios
(nombre, apellido, correo, dni, foto_perfil, contraseña,
 password_hasheada, estado, rol_id)
VALUES
('Admin', 'Prueba', 'admin@gmail.com', 123, NULL, 'AQAAAAIAAYagAAAAEA0Rdpf77ra9W4pRZqnqeBfysluUZvPbCaSRrH5DZGIf2kLSYOb4d+SuLQGXUwJ3Ng==', 1, 1, 1),
('Juan', 'Pérez', 'juan.perez@gmail.com', 40123456, NULL, 'AQAAAAIAAYagAAAAEHG5FBF59S3kD9kRL7GWS6mF21VBvXdp14pXkG5Wn3JE8GNUMsc47VDjKnZWXrPZaw==', 1, 1, 3),
('María', 'Gómez', 'maria.gomez@gmail.com', 38456123, NULL, 'AQAAAAIAAYagAAAAEKF2Bvi+JMKFINQOvhoLtmkQx0BpxjQcjSYUsOcIfWjXIvC/kCuZHPMcGTh24YPp0Q==', 1, 1, 3),
('Carlos', 'López', 'carlos.lopez@gmail.com', 42987654, NULL, 'AQAAAAIAAYagAAAAEC+3ejO//Cdms0STajRK13rQGA/ab5f6lSYsZ/6Anrc5irouQbMVgJThhgXp/G1UBA==', 1, 1, 2),
('Ana', 'Martínez', 'ana.martinez@gmail.com', 35111222, NULL, 'AQAAAAIAAYagAAAAEAIJH6M4glqHsq6rRtdmUpC+W+05OBpuaC2MKQnRMSgb+r3eNwzUngW9S475rH4AWA==', 1, 1, 3),
('Luis', 'Rodríguez', 'luis.rod@gmail.com', 41555666, NULL, 'AQAAAAIAAYagAAAAEKqQwM0nOHL5v8F76Tny4B/PIXhbFFfnY/hEjjkqcj1Qn9iCefGgdrYA2I4gHHGECg==', 1, 0, 3),
('Carlos', 'Gómez', 'carlos.gomez@gmail.com', 30123456, NULL, 'AQAAAAIAAYagAAAAEDRgOyWP/tHCTx/9mxxi/KrFLuNDNBvu5/Jd8R/foGuHa5TTTeQpfPbBaw/q4jessg==', 1, 1, 3),
('María', 'Rodríguez', 'maria.rod@yahoo.com', 32456123, NULL, 'AQAAAAIAAYagAAAAEJXRwW0V6XJFiMLxVqoPjCqAbh7JAh2iRhynR5+2+eH8w2EiQmMy5Pep5IaLueriXQ==', 1, 1, 3),
('Javier', 'López', 'javi.lopez@outlook.com', 28999888, NULL, 'AQAAAAIAAYagAAAAEPBWVoNkx0axPRdOrO40sKhTfw5UXhTRtgp46P+CxsnF+jXA+STz5Td+e2s5XYWNzg==', 1, 1, 3),
('Lucía', 'Fernández', 'lucia.f@gmail.com', 35111223, NULL, 'AQAAAAIAAYagAAAAEL5A+pq/rAgNoVfO6ttu7DHW6Br0IawhQvv8iKyCkDrk5sqPTpjtFKYsfCt/9RCA9g==', 1, 1, 3),
('Diego', 'Martinez', 'diego.mtz@hotmail.com', 40222333, NULL, 'AQAAAAIAAYagAAAAEE8UlEKZ822JFPoMIaJvHcHMgQABYHan7bmOg3OcqhwZcx7PXiGabOyyIF8ttRtOag==', 1, 1, 3),
('Sofía', 'Benítez', 'sofia.b@gmail.com', 38444555, NULL, 'AQAAAAIAAYagAAAAEOHA2OP4Ur7b00W49a4N2/30tcAQSmYoZkx8YJ5wys/d0zC31jWB8pXGYgNU3ty4Sw==', 1, 1, 3),
('Lucas', 'Castro', 'lucas.castro@live.com.ar', 41555667, NULL, 'AQAAAAIAAYagAAAAEFI++GEMmlwn+UCDaz0iSM5jHverVgH9hbPbbrHvh6C4ZBdAd8oHwTxyO+Iv6L1oyQ==', 1, 1, 3),
('Valentina', 'Silva', 'valen.silva@gmail.com', 39666777, NULL, 'AQAAAAIAAYagAAAAELMZn7DIvajDLlZ/NjD60++Ft+IZhwkBHgmoWWYCRsU0twidwdbULYnyMX5lrWBmUQ==', 1, 1, 3),
('Martín', 'Sosa', 'martin.sosa@yahoo.com.ar', 33777888, NULL, 'AQAAAAIAAYagAAAAEPTFoYerVxj2HYoLDtiPZxalKHtwRTqtizuk9uSXFaaCdXBH0gbYxd+cXFvEb2KfVQ==', 1, 1, 3),
('Camila', 'Ruiz', 'camila.ruiz@gmail.com', 42888999, NULL, 'AQAAAAIAAYagAAAAENaP0C1ilnVxkyDdAobzhUR5d2Jnb2YWYCgKreHpnYpIpx0pu6LNr73LjXoaxBMR+A==', 1, 1, 3);

-- 11. FUNCIONES
INSERT INTO funciones (fecha_hora, precio, estado, pelicula_id, sala_id) VALUES
-- Fechas futuras calculadas desde el día de la importación.
(CURDATE() + INTERVAL 1 DAY + INTERVAL 18 HOUR, 4500.00, 1, 1, 1),
(CURDATE() + INTERVAL 1 DAY + INTERVAL 21 HOUR + INTERVAL 30 MINUTE, 4500.00, 1, 1, 1),
(CURDATE() + INTERVAL 2 DAY + INTERVAL 15 HOUR, 5500.00, 1, 5, 2),
(CURDATE() + INTERVAL 2 DAY + INTERVAL 20 HOUR, 7000.00, 1, 6, 3),
(CURDATE() + INTERVAL 3 DAY + INTERVAL 22 HOUR + INTERVAL 30 MINUTE, 7000.00, 1, 3, 3),
(CURDATE() + INTERVAL 3 DAY + INTERVAL 17 HOUR, 4000.00, 1, 9, 4),
(CURDATE() + INTERVAL 4 DAY + INTERVAL 18 HOUR + INTERVAL 30 MINUTE, 4500.00, 1, 11, 4),
(CURDATE() + INTERVAL 4 DAY + INTERVAL 21 HOUR + INTERVAL 45 MINUTE, 5500.00, 1, 12, 4),
(CURDATE() + INTERVAL 5 DAY + INTERVAL 15 HOUR, 4000.00, 1, 13, 5),
(CURDATE() + INTERVAL 5 DAY + INTERVAL 19 HOUR, 6000.00, 1, 14, 4),
(CURDATE() + INTERVAL 6 DAY + INTERVAL 20 HOUR, 4500.00, 1, 15, 6),
(CURDATE() + INTERVAL 6 DAY + INTERVAL 23 HOUR, 5000.00, 1, 16, 5),
(CURDATE() + INTERVAL 7 DAY + INTERVAL 17 HOUR + INTERVAL 30 MINUTE, 4500.00, 1, 17, 6),
(CURDATE() + INTERVAL 7 DAY + INTERVAL 14 HOUR, 3500.00, 1, 17, 4),
(CURDATE() + INTERVAL 8 DAY + INTERVAL 22 HOUR, 5500.00, 1, 9, 4),
(CURDATE() + INTERVAL 8 DAY + INTERVAL 19 HOUR + INTERVAL 30 MINUTE, 4500.00, 1, 8, 5);

-- 12. RESERVAS
-- IDs preparados para una instalación sobre tablas vacías.
-- La reserva 3 está cancelada para probar ese estado.

INSERT INTO reservas
(id, fecha_creacion, monto, estado, usuario_id, funcion_id)
VALUES
(1, NOW(), 9000.00, 1, 2, 1),
(2, NOW(), 5500.00, 1, 3, 3),
(3, NOW(), 7000.00, 0, 5, 4),
(4, NOW(), 9000.00, 1, 7, 2),
(5, NOW(), 5500.00, 1, 8, 8),
(6, NOW(), 12000.00, 1, 9, 10),
(7, NOW(), 4000.00, 1, 10, 6),
(8, NOW(), 4500.00, 1, 11, 7),
(9, NOW(), 10000.00, 1, 12, 12),
(10, NOW(), 4500.00, 1, 13, 13),
(11, NOW(), 10500.00, 1, 14, 14);

-- 13. DETALLE_RESERVA
-- Busca cada butaca por su número y la sala de la función.
-- El precio se obtiene de esa función.

INSERT INTO detalle_reserva (precio, reserva_id, butaca_id)
SELECT
    f.precio,
    r.id,
    b.id
FROM (
    SELECT 1 AS reserva_id, 'A1' AS numero
    UNION ALL SELECT 1, 'A2'
    UNION ALL SELECT 2, 'A1'
    UNION ALL SELECT 3, 'A1'
    UNION ALL SELECT 4, 'A1'
    UNION ALL SELECT 4, 'A2'
    UNION ALL SELECT 5, 'A1'
    UNION ALL SELECT 6, 'A1'
    UNION ALL SELECT 6, 'A2'
    UNION ALL SELECT 7, 'A1'
    UNION ALL SELECT 8, 'A1'
    UNION ALL SELECT 9, 'A1'
    UNION ALL SELECT 9, 'A2'
    UNION ALL SELECT 10, 'A1'
    UNION ALL SELECT 11, 'A1'
    UNION ALL SELECT 11, 'A2'
    UNION ALL SELECT 11, 'A3'
) AS seleccion
INNER JOIN reservas AS r
    ON r.id = seleccion.reserva_id
INNER JOIN funciones AS f
    ON f.id = r.funcion_id
INNER JOIN butacas AS b
    ON b.sala_id = f.sala_id
    AND b.numero = seleccion.numero;

-- 14. PAGOS
-- Un pago por cada reserva activa de esta muestra.
-- La reserva cancelada queda sin pago.

INSERT INTO pagos
(concepto, fecha_pago, fecha_anulacion, importe, estado, reserva_id)
SELECT
    CONCAT('Pago de prueba - Reserva ', r.id),
    NOW(),
    NULL,
    r.monto,
    1,
    r.id
FROM reservas AS r
WHERE r.estado = 1;