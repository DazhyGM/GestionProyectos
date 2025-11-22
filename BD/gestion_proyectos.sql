-- phpMyAdmin SQL Dump
-- version 5.2.1
-- https://www.phpmyadmin.net/
--
-- Servidor: 127.0.0.1
-- Tiempo de generación: 22-11-2025 a las 12:41:04
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
-- Base de datos: `gestion_proyectos`
--
CREATE DATABASE IF NOT EXISTS `gestion_proyectos` DEFAULT CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci;
USE `gestion_proyectos`;

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `estados_proyecto`
--

CREATE TABLE IF NOT EXISTS `estados_proyecto` (
  `id_estado` int(11) NOT NULL AUTO_INCREMENT,
  `nombre_estado` varchar(50) NOT NULL,
  PRIMARY KEY (`id_estado`),
  UNIQUE KEY `nombre_estado` (`nombre_estado`)
) ENGINE=InnoDB AUTO_INCREMENT=5 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Volcado de datos para la tabla `estados_proyecto`
--

INSERT INTO `estados_proyecto` (`id_estado`, `nombre_estado`) VALUES
(4, 'Cancelado'),
(2, 'En Progreso'),
(3, 'Finalizado'),
(1, 'Pendiente');

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `proyectos`
--

CREATE TABLE IF NOT EXISTS `proyectos` (
  `id_proyecto` int(11) NOT NULL AUTO_INCREMENT,
  `nombre` varchar(100) NOT NULL,
  `descripcion` text DEFAULT NULL,
  `fecha_inicio` date DEFAULT NULL,
  `fecha_fin` date DEFAULT NULL,
  `id_estado` int(11) DEFAULT NULL,
  `numero_documento` int(50) NOT NULL,
  PRIMARY KEY (`id_proyecto`),
  KEY `id_estado` (`id_estado`),
  KEY `numero_documento` (`numero_documento`)
) ENGINE=InnoDB AUTO_INCREMENT=16 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Volcado de datos para la tabla `proyectos`
--

INSERT INTO `proyectos` (`id_proyecto`, `nombre`, `descripcion`, `fecha_inicio`, `fecha_fin`, `id_estado`, `numero_documento`) VALUES
(9, 'Proyecto X', 'Proyecto de fiestuki bien sabrosuki para un fincho bien buenuki', '2025-11-14', '2025-11-20', 3, 1000619691),
(10, 'Proyecto Pasar Escritorios', 'Lograr pasar Desarrollo en ambientes de escritorios con este proyecto buenisimo', '2025-11-07', '2025-11-28', 2, 1000619691),
(11, 'El proyecto', 'proyecto de prueba', '2025-11-07', '2025-11-08', 2, 111),
(12, 'Proyecto Hackaton', 'Proyecto para la hackaton Uninpahu', '2025-11-19', '2025-11-20', 2, 1000619691),
(13, 'Proyecto de prueba', 'Proyecto', '2025-11-20', '2025-11-20', 4, 1000619691),
(14, 'asas', 'asas', '2025-11-21', '2025-11-22', 4, 1231231212),
(15, 'prueba', 'aaa', '2025-11-22', '2025-11-27', 2, 1000619691);

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `proyectos_usuarios`
--

CREATE TABLE IF NOT EXISTS `proyectos_usuarios` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `id_proyecto` int(11) NOT NULL,
  `numero_documento` int(11) NOT NULL,
  `rol_en_proyecto` varchar(30) DEFAULT 'colaborador',
  `fecha_asignacion` timestamp NOT NULL DEFAULT current_timestamp(),
  PRIMARY KEY (`id`),
  KEY `fk_proyecto` (`id_proyecto`),
  KEY `fk_usuario` (`numero_documento`)
) ENGINE=InnoDB AUTO_INCREMENT=6 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Volcado de datos para la tabla `proyectos_usuarios`
--

INSERT INTO `proyectos_usuarios` (`id`, `id_proyecto`, `numero_documento`, `rol_en_proyecto`, `fecha_asignacion`) VALUES
(1, 15, 111, 'colaborador', '2025-11-22 10:28:02'),
(2, 11, 121135456, 'colaborador', '2025-11-22 11:03:57'),
(3, 9, 1000619691, 'colaborador', '2025-11-22 11:26:46'),
(4, 10, 1000619691, 'colaborador', '2025-11-22 11:27:07'),
(5, 12, 1000619691, 'colaborador', '2025-11-22 11:31:20');

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `roles`
--

CREATE TABLE IF NOT EXISTS `roles` (
  `id_rol` int(11) NOT NULL AUTO_INCREMENT,
  `nombre_rol` varchar(50) NOT NULL,
  PRIMARY KEY (`id_rol`),
  UNIQUE KEY `nombre_rol` (`nombre_rol`)
) ENGINE=InnoDB AUTO_INCREMENT=5 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Volcado de datos para la tabla `roles`
--

INSERT INTO `roles` (`id_rol`, `nombre_rol`) VALUES
(1, 'Administrador'),
(4, 'Colaborador'),
(2, 'Jurado de Proyecto'),
(3, 'Líder de Proyecto');

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `tareas`
--

CREATE TABLE IF NOT EXISTS `tareas` (
  `id_tarea` int(11) NOT NULL AUTO_INCREMENT,
  `id_proyecto` int(11) NOT NULL,
  `nombre_tarea` varchar(100) NOT NULL,
  `completada` tinyint(1) DEFAULT 0,
  `encargado_id` int(11) DEFAULT NULL,
  PRIMARY KEY (`id_tarea`),
  KEY `id_proyecto` (`id_proyecto`)
) ENGINE=InnoDB AUTO_INCREMENT=27 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Volcado de datos para la tabla `tareas`
--

INSERT INTO `tareas` (`id_tarea`, `id_proyecto`, `nombre_tarea`, `completada`, `encargado_id`) VALUES
(1, 9, 'hacer un coctel', 1, 1000619691),
(2, 9, 'tomarse el coctel', 1, 1000619691),
(3, 10, 'hacer el proyecto', 1, 1000619691),
(4, 10, 'hacer tareas', 1, 1000619691),
(5, 9, 'sasas', 1, 1000619691),
(6, 12, 'Elegir Reto de la Hackaton', 1, 1000619691),
(7, 12, 'Verificar documentacion', 0, 1000619691),
(8, 12, 'Elegir estructura del proyecto', 0, 1000619691),
(9, 12, 'Elegir lenguajes front y back', 0, 1000619691),
(10, 12, 'Realizar mockups o wireframes', 0, 1000619691),
(11, 12, 'Realizar casos de uso', 0, 1000619691),
(12, 12, 'Requisitos funcionales y no funcionales', 0, 1000619691),
(13, 12, 'Pruebas de back en postman', 0, 1000619691),
(14, 10, 'Hacer el parcial', 1, 1000619691),
(20, 15, 'aaa', 1, 111),
(21, 15, 'aa', 0, 1),
(22, 11, 'nueva tarea', 1, 121135456),
(23, 11, 'asa', 0, 111),
(24, 10, 'asa', 0, 1000619691),
(25, 10, 'aaa', 0, 1000619691),
(26, 12, 'aaa', 1, 1000619691);

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `usuarios`
--

CREATE TABLE IF NOT EXISTS `usuarios` (
  `numero_documento` int(20) NOT NULL,
  `nombre` varchar(255) DEFAULT NULL,
  `apellido` varchar(255) DEFAULT NULL,
  `correo` varchar(255) DEFAULT NULL,
  `contrasena` varchar(255) DEFAULT NULL,
  `telefono` varchar(11) DEFAULT NULL,
  `rol` int(11) NOT NULL,
  PRIMARY KEY (`numero_documento`),
  KEY `FK_Usuarios_Roles` (`rol`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Volcado de datos para la tabla `usuarios`
--

INSERT INTO `usuarios` (`numero_documento`, `nombre`, `apellido`, `correo`, `contrasena`, `telefono`, `rol`) VALUES
(111, 'Dav', 'Sab', 'dav@gmail.com', '123', '123', 4),
(121135456, 'Kevin', 'MocaCO', 'moc@gmail.com', '123', '3202455682', 4),
(1000619691, 'Kevin', 'Sabogal', 'kevinsabogal24@gmail.com', '123', '123', 4),
(1111262797, 'vane', 'medina', 'medina@gmail.com', '123', '2345', 4),
(1112111121, 'Manuel', 'Medrano', 'manu@gmail.com', '123', '123', 4),
(1231231212, 'Prueba', 'De la prueba', 'prueba@gmail.com', '123', '3203202020', 4);

--
-- Restricciones para tablas volcadas
--

--
-- Filtros para la tabla `proyectos`
--
ALTER TABLE `proyectos`
  ADD CONSTRAINT `proyectos_ibfk_1` FOREIGN KEY (`id_estado`) REFERENCES `estados_proyecto` (`id_estado`),
  ADD CONSTRAINT `proyectos_ibfk_2` FOREIGN KEY (`numero_documento`) REFERENCES `usuarios` (`numero_documento`);

--
-- Filtros para la tabla `proyectos_usuarios`
--
ALTER TABLE `proyectos_usuarios`
  ADD CONSTRAINT `fk_proyecto` FOREIGN KEY (`id_proyecto`) REFERENCES `proyectos` (`id_proyecto`) ON DELETE CASCADE,
  ADD CONSTRAINT `fk_usuario` FOREIGN KEY (`numero_documento`) REFERENCES `usuarios` (`numero_documento`) ON DELETE CASCADE;

--
-- Filtros para la tabla `tareas`
--
ALTER TABLE `tareas`
  ADD CONSTRAINT `tareas_ibfk_1` FOREIGN KEY (`id_proyecto`) REFERENCES `proyectos` (`id_proyecto`) ON DELETE CASCADE;

--
-- Filtros para la tabla `usuarios`
--
ALTER TABLE `usuarios`
  ADD CONSTRAINT `FK_Usuarios_Roles` FOREIGN KEY (`rol`) REFERENCES `roles` (`id_rol`) ON UPDATE CASCADE;
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
