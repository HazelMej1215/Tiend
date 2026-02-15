-- phpMyAdmin SQL Dump
-- version 5.2.1
-- https://www.phpmyadmin.net/
--
-- Servidor: 127.0.0.1
-- Tiempo de generación: 15-02-2026 a las 02:34:14
-- Versión del servidor: 10.4.32-MariaDB
-- Versión de PHP: 8.2.12

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
START TRANSACTION;
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;

--
-- Base de datos: `tienda_admin`
--

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `categorias`
--

CREATE TABLE `categorias` (
  `id_categoria` int(11) NOT NULL,
  `nombre` varchar(80) NOT NULL,
  `estado` enum('ACTIVO','INACTIVO') NOT NULL DEFAULT 'ACTIVO',
  `creado_en` datetime NOT NULL DEFAULT current_timestamp()
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

--
-- Volcado de datos para la tabla `categorias`
--

INSERT INTO `categorias` (`id_categoria`, `nombre`, `estado`, `creado_en`) VALUES
(1, 'Computadoras', 'ACTIVO', '2026-02-13 18:34:24'),
(2, 'Teclados', 'ACTIVO', '2026-02-13 18:34:24'),
(3, 'Componentes', 'ACTIVO', '2026-02-13 18:34:24'),
(4, 'Monitores', 'ACTIVO', '2026-02-13 18:34:24'),
(5, 'Mouse', 'ACTIVO', '2026-02-13 18:34:24');

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `estados_producto`
--

CREATE TABLE `estados_producto` (
  `id_estado` tinyint(4) NOT NULL,
  `nombre` varchar(20) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

--
-- Volcado de datos para la tabla `estados_producto`
--

INSERT INTO `estados_producto` (`id_estado`, `nombre`) VALUES
(1, 'ACTIVO'),
(2, 'INACTIVO');

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `productos`
--

CREATE TABLE `productos` (
  `id_producto` int(11) NOT NULL,
  `nombre` varchar(120) NOT NULL,
  `descripcion` text DEFAULT NULL,
  `precio` decimal(10,2) NOT NULL,
  `stock` int(11) NOT NULL DEFAULT 0,
  `sku` varchar(60) DEFAULT NULL,
  `id_categoria` int(11) NOT NULL,
  `id_estado` tinyint(4) NOT NULL DEFAULT 1,
  `creado_en` datetime NOT NULL DEFAULT current_timestamp(),
  `actualizado_en` datetime DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

--
-- Volcado de datos para la tabla `productos`
--

INSERT INTO `productos` (`id_producto`, `nombre`, `descripcion`, `precio`, `stock`, `sku`, `id_categoria`, `id_estado`, `creado_en`, `actualizado_en`) VALUES
(1, 'Laptop Lenovo', 'Ryzen 5, 16GB RAM, 512GB SSD', 14500.00, 8, 'LAP-LEN-001', 1, 1, '2026-02-14 19:00:42', NULL),
(2, 'Teclado Mecánico', 'RGB, switches azules', 899.00, 15, 'TEC-MEC-002', 2, 1, '2026-02-14 19:00:42', NULL),
(3, 'SSD 1TB', 'NVMe Gen3', 1199.00, 10, 'SSD-1TB-003', 3, 1, '2026-02-14 19:00:42', NULL),
(4, 'Monitor 24\"', 'FullHD 75Hz', 2499.00, 6, 'MON-24-004', 4, 1, '2026-02-14 19:00:42', NULL),
(5, 'Mouse Gamer', 'DPI ajustable', 399.00, 20, 'MOU-GAM-005', 5, 1, '2026-02-14 19:00:42', NULL);

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `usuarios`
--

CREATE TABLE `usuarios` (
  `id_usuario` int(11) NOT NULL,
  `nombre` varchar(100) NOT NULL,
  `email` varchar(120) NOT NULL,
  `password_hash` varchar(255) NOT NULL,
  `rol` enum('ADMIN','CLIENTE') NOT NULL DEFAULT 'CLIENTE',
  `estado` enum('ACTIVO','INACTIVO') NOT NULL DEFAULT 'ACTIVO',
  `creado_en` datetime NOT NULL DEFAULT current_timestamp()
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

--
-- Volcado de datos para la tabla `usuarios`
--

INSERT INTO `usuarios` (`id_usuario`, `nombre`, `email`, `password_hash`, `rol`, `estado`, `creado_en`) VALUES
(1, 'Admin', 'admin@local', '$2a$10$CAMBIA_ESTO_POR_HASH_REAL', 'ADMIN', 'ACTIVO', '2026-02-13 18:34:24');

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `ventas`
--

CREATE TABLE `ventas` (
  `id_venta` int(11) NOT NULL,
  `fecha` datetime NOT NULL DEFAULT current_timestamp(),
  `total` decimal(12,2) NOT NULL DEFAULT 0.00,
  `estatus` enum('PAGADA','CANCELADA') NOT NULL DEFAULT 'PAGADA',
  `id_usuario` int(11) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `venta_detalle`
--

CREATE TABLE `venta_detalle` (
  `id_detalle` int(11) NOT NULL,
  `id_venta` int(11) NOT NULL,
  `id_producto` int(11) NOT NULL,
  `cantidad` int(11) NOT NULL,
  `precio_unitario` decimal(10,2) NOT NULL,
  `subtotal` decimal(12,2) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

--
-- Disparadores `venta_detalle`
--
DELIMITER $$
CREATE TRIGGER `trg_detalle_ad_total` AFTER DELETE ON `venta_detalle` FOR EACH ROW BEGIN
  UPDATE ventas
  SET total = (
    SELECT IFNULL(SUM(subtotal),0)
    FROM venta_detalle
    WHERE id_venta = OLD.id_venta
  )
  WHERE id_venta = OLD.id_venta;
END
$$
DELIMITER ;
DELIMITER $$
CREATE TRIGGER `trg_detalle_ai_total` AFTER INSERT ON `venta_detalle` FOR EACH ROW BEGIN
  UPDATE ventas
  SET total = (
    SELECT IFNULL(SUM(subtotal),0)
    FROM venta_detalle
    WHERE id_venta = NEW.id_venta
  )
  WHERE id_venta = NEW.id_venta;
END
$$
DELIMITER ;
DELIMITER $$
CREATE TRIGGER `trg_detalle_bd_stock` BEFORE DELETE ON `venta_detalle` FOR EACH ROW BEGIN
  UPDATE productos
  SET stock = stock + OLD.cantidad,
      actualizado_en = CURRENT_TIMESTAMP
  WHERE id_producto = OLD.id_producto;
END
$$
DELIMITER ;
DELIMITER $$
CREATE TRIGGER `trg_detalle_bi_subtotal_stock` BEFORE INSERT ON `venta_detalle` FOR EACH ROW BEGIN
  DECLARE stock_actual INT;

  SELECT stock INTO stock_actual
  FROM productos
  WHERE id_producto = NEW.id_producto
  FOR UPDATE;

  IF stock_actual < NEW.cantidad THEN
    SIGNAL SQLSTATE '45000'
      SET MESSAGE_TEXT = 'Stock insuficiente.';
  END IF;

  SET NEW.subtotal = NEW.cantidad * NEW.precio_unitario;

  UPDATE productos
  SET stock = stock - NEW.cantidad,
      actualizado_en = CURRENT_TIMESTAMP
  WHERE id_producto = NEW.id_producto;
END
$$
DELIMITER ;

--
-- Índices para tablas volcadas
--

--
-- Indices de la tabla `categorias`
--
ALTER TABLE `categorias`
  ADD PRIMARY KEY (`id_categoria`),
  ADD UNIQUE KEY `nombre` (`nombre`);

--
-- Indices de la tabla `estados_producto`
--
ALTER TABLE `estados_producto`
  ADD PRIMARY KEY (`id_estado`),
  ADD UNIQUE KEY `nombre` (`nombre`);

--
-- Indices de la tabla `productos`
--
ALTER TABLE `productos`
  ADD PRIMARY KEY (`id_producto`),
  ADD UNIQUE KEY `sku` (`sku`),
  ADD KEY `idx_productos_categoria` (`id_categoria`),
  ADD KEY `idx_productos_estado` (`id_estado`),
  ADD KEY `idx_productos_nombre` (`nombre`);

--
-- Indices de la tabla `usuarios`
--
ALTER TABLE `usuarios`
  ADD PRIMARY KEY (`id_usuario`),
  ADD UNIQUE KEY `email` (`email`);

--
-- Indices de la tabla `ventas`
--
ALTER TABLE `ventas`
  ADD PRIMARY KEY (`id_venta`),
  ADD KEY `fk_ventas_usuario` (`id_usuario`),
  ADD KEY `idx_ventas_fecha` (`fecha`),
  ADD KEY `idx_ventas_estatus` (`estatus`);

--
-- Indices de la tabla `venta_detalle`
--
ALTER TABLE `venta_detalle`
  ADD PRIMARY KEY (`id_detalle`),
  ADD KEY `idx_detalle_venta` (`id_venta`),
  ADD KEY `idx_detalle_producto` (`id_producto`);

--
-- AUTO_INCREMENT de las tablas volcadas
--

--
-- AUTO_INCREMENT de la tabla `categorias`
--
ALTER TABLE `categorias`
  MODIFY `id_categoria` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=11;

--
-- AUTO_INCREMENT de la tabla `productos`
--
ALTER TABLE `productos`
  MODIFY `id_producto` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=6;

--
-- AUTO_INCREMENT de la tabla `usuarios`
--
ALTER TABLE `usuarios`
  MODIFY `id_usuario` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=2;

--
-- AUTO_INCREMENT de la tabla `ventas`
--
ALTER TABLE `ventas`
  MODIFY `id_venta` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT de la tabla `venta_detalle`
--
ALTER TABLE `venta_detalle`
  MODIFY `id_detalle` int(11) NOT NULL AUTO_INCREMENT;

--
-- Restricciones para tablas volcadas
--

--
-- Filtros para la tabla `productos`
--
ALTER TABLE `productos`
  ADD CONSTRAINT `fk_productos_categoria` FOREIGN KEY (`id_categoria`) REFERENCES `categorias` (`id_categoria`) ON UPDATE CASCADE,
  ADD CONSTRAINT `fk_productos_estado` FOREIGN KEY (`id_estado`) REFERENCES `estados_producto` (`id_estado`) ON UPDATE CASCADE;

--
-- Filtros para la tabla `ventas`
--
ALTER TABLE `ventas`
  ADD CONSTRAINT `fk_ventas_usuario` FOREIGN KEY (`id_usuario`) REFERENCES `usuarios` (`id_usuario`) ON DELETE SET NULL ON UPDATE CASCADE;

--
-- Filtros para la tabla `venta_detalle`
--
ALTER TABLE `venta_detalle`
  ADD CONSTRAINT `fk_detalle_producto` FOREIGN KEY (`id_producto`) REFERENCES `productos` (`id_producto`) ON UPDATE CASCADE,
  ADD CONSTRAINT `fk_detalle_venta` FOREIGN KEY (`id_venta`) REFERENCES `ventas` (`id_venta`) ON DELETE CASCADE ON UPDATE CASCADE;
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
