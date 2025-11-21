-- phpMyAdmin SQL Dump
-- version 5.2.1
-- https://www.phpmyadmin.net/
--
-- Servidor: 127.0.0.1
-- Tiempo de generación: 22-11-2025 a las 00:42:00
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
) ENGINE=InnoDB AUTO_INCREMENT=15 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Volcado de datos para la tabla `proyectos`
--

INSERT INTO `proyectos` (`id_proyecto`, `nombre`, `descripcion`, `fecha_inicio`, `fecha_fin`, `id_estado`, `numero_documento`) VALUES
(9, 'Proyecto X', 'Proyecto de fiestuki bien sabrosuki para un fincho bien buenuki', '2025-11-14', '2025-11-20', 3, 1000619691),
(10, 'Proyecto Pasar Escritorios', 'Lograr pasar Desarrollo en ambientes de escritorios con este proyecto buenisimo', '2025-11-07', '2025-11-28', 3, 1000619691),
(11, 'El proyecto', 'proyecto de prueba', '2025-11-07', '2025-11-08', 2, 111),
(12, 'Proyecto Hackaton', 'Proyecto para la hackaton Uninpahu', '2025-11-19', '2025-11-20', 2, 1000619691),
(13, 'Proyecto de prueba', 'Proyecto', '2025-11-20', '2025-11-20', 4, 1000619691),
(14, 'asas', 'asas', '2025-11-21', '2025-11-22', 4, 1231231212);

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
  `encargado` varchar(100) DEFAULT NULL,
  `completada` tinyint(1) DEFAULT 0,
  PRIMARY KEY (`id_tarea`),
  KEY `id_proyecto` (`id_proyecto`)
) ENGINE=InnoDB AUTO_INCREMENT=20 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Volcado de datos para la tabla `tareas`
--

INSERT INTO `tareas` (`id_tarea`, `id_proyecto`, `nombre_tarea`, `encargado`, `completada`) VALUES
(1, 9, 'hacer un coctel', 'Kevin', 1),
(2, 9, 'tomarse el coctel', 'Kevin', 1),
(3, 10, 'hacer el proyecto', 'kevin', 1),
(4, 10, 'hacer tareas', 'kevin', 1),
(5, 9, 'sasas', 'kevin', 1),
(6, 12, 'Elegir Reto de la Hackaton', 'Harold', 1),
(7, 12, 'Verificar documentacion', 'Harold', 0),
(8, 12, 'Elegir estructura del proyecto', 'Kevin', 0),
(9, 12, 'Elegir lenguajes front y back', 'Kevin', 0),
(10, 12, 'Realizar mockups o wireframes', 'Harold', 0),
(11, 12, 'Realizar casos de uso', 'Harold', 0),
(12, 12, 'Requisitos funcionales y no funcionales', 'Harold', 0),
(13, 12, 'Pruebas de back en postman', 'Kevin', 0),
(14, 10, 'Hacer el parcial', 'Kevin', 1),
(15, 13, 'adsasa', 'kevin', 1),
(16, 13, 'dsaasa', 'kevin', 0),
(17, 14, 'asas', 'sasa', 1),
(18, 14, 'ssss', 'sss', 0),
(19, 14, 'asa', 'aaa', 0);

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
(1000619691, 'Kevin', 'Sabogal', 'kevinsabogal24@gmail.com', '123', '123', 1),
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
