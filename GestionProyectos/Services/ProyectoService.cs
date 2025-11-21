using GestionProyectos.Models;
using GestionProyectos.Models.Conex;
using System;
using System.Collections.Generic;

namespace GestionProyectos.Services
{
    public class ProyectoService
    {
        private readonly ConexionDB conexion = new ConexionDB();


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


        public List<ProyectoModel> ObtenerProyectos(int numeroDocumento)
        {
            return conexion.ObtenerProyectos(numeroDocumento);
        }

        public List<EstadoProyectoModel> ObtenerEstados()
        {
            return conexion.ObtenerEstadosProyecto();
        }
        public string ActualizarProyecto(ProyectoModel proyecto)
        {
            return conexion.ActualizarProyecto(proyecto);
        }
        public bool CambiarEstado(int idProyecto, int idNuevoEstado)
        {
            return conexion.ActualizarEstadoProyecto(idProyecto, idNuevoEstado);
        }

        public List<ProyectoModel> ObtenerPInactivos(int numeroDocumento)
        {
            return conexion.ObtenerPInactivos(numeroDocumento);
        }

        public string ActivarProyecto(ProyectoModel proyecto)
        {
            return conexion.ActivarProyecto(proyecto);
        }

    }
}
