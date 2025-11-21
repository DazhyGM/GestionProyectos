using GestionProyectos.Models;
using GestionProyectos.Services;
using System.Collections.Generic;

namespace GestionProyectos.Controllers
{
    public class ProyectoController
    {
        private readonly ProyectoService proyectoService = new ProyectoService();

        public string RegistrarProyecto(ProyectoModel proyecto)
        {
            return proyectoService.RegistrarProyecto(proyecto);
        }

        public List<ProyectoModel> ObtenerProyectos(int numeroDocumento)
        {
            return proyectoService.ObtenerProyectos(numeroDocumento);
        }

        public List<EstadoProyectoModel> ObtenerEstados()
        {
            return proyectoService.ObtenerEstados();
        }
        public string ActualizarProyecto(ProyectoModel proyecto)
        {
            return proyectoService.ActualizarProyecto(proyecto);
        }
        public bool ActualizarEstado(int idProyecto, int idNuevoEstado)
        {
            return proyectoService.CambiarEstado(idProyecto, idNuevoEstado);
        }
        public List<ProyectoModel> ObtenerPInactivos(int numeroDocumento)
        {
            return proyectoService.ObtenerPInactivos(numeroDocumento);

        }

        public string ActivarProyecto(ProyectoModel proyecto)
        {
            return proyectoService.ActivarProyecto(proyecto);
        }

    }
}
