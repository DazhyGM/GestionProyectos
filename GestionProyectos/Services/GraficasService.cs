using GestionProyectos.Models;
using GestionProyectos.Models.Conex;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionProyectos.Services
{
    public  class GraficasService
    {
        private readonly ConexionDB conexion = new ConexionDB();

        public List<GraficasModel> ObtenerProyectosEstado()
        {
            return conexion.ObtenerProyectosEstado();
        }

        public List<GraficasModel> ObtenerCantidadTareasPorEncargado(int numeroDocumento)
        {
            return conexion.ObtenerCantidadTareasPorEncargado(numeroDocumento);
        }

        public List<GraficasModel> ObtenerEncargados()
        {
            return conexion.ObtenerEncargados();
        }

        public List<GraficasModel> ObtenerProyectosPorUsuario()
        {
            return conexion.ObtenerProyectosporUsuario();
        }
    }

}
