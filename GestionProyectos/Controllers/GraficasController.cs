using GestionProyectos.Models;
using GestionProyectos.Models.Conex;
using GestionProyectos.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionProyectos.Controllers
{
    public class GraficasController
    {

        private readonly GraficasService graficasService = new GraficasService();

        public List<GraficasModel> ObtenerProyectosEstado()
        {
            return graficasService.ObtenerProyectosEstado();
        }

        public List<GraficasModel> ObtenerCantidadTareasPorEncargado(int numeroDocumento)
        {
            return graficasService.ObtenerCantidadTareasPorEncargado(numeroDocumento);
        }

        public List<GraficasModel> ObtenerEncargados()
        {
            return graficasService.ObtenerEncargados();
        }

        public List<GraficasModel> ObtenerProyectosPorUsuario()
        {
            return graficasService.ObtenerProyectosPorUsuario();
        }

    }
}
