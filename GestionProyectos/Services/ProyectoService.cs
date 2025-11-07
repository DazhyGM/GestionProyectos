using GestionProyectos.Models;
using GestionProyectos.Models.Conex;
using System;
using System.Collections.Generic;

namespace GestionProyectos.Services
{
    public class ProyectoService
    {
        private readonly ConexionDB conexion = new ConexionDB();

        // Registrar un nuevo proyecto
        public string RegistrarProyecto(ProyectoModel proyecto)
        {
            if (proyecto == null)
                return "Datos inválidos.";

            if (string.IsNullOrWhiteSpace(proyecto.Nombre))
                return "El nombre del proyecto es obligatorio.";

            if (proyecto.FechaInicio > proyecto.FechaFin)
                return "La fecha de inicio no puede ser posterior a la fecha de fin.";

            bool insertado = conexion.AgregarProyecto(proyecto);
            return insertado ? "OK" : "No se pudo registrar el proyecto.";
        }

        // Obtener todos los proyectos
        public List<ProyectoModel> ObtenerProyectos(int numeroDocumento)
        {
            return conexion.ObtenerProyectos(numeroDocumento);
        }

        // Obtener los estados disponibles para los proyectos
        public List<EstadoProyectoModel> ObtenerEstados()
        {
            return conexion.ObtenerEstadosProyecto();
        }
        public string ActualizarProyecto(ProyectoModel proyecto)
        {
            return conexion.ActualizarProyecto(proyecto);
        }

    }
}
