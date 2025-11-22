using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GestionProyectos.Models;
using GestionProyectos.Models.Conex;
using System.Collections.Generic;

namespace GestionProyectos.Services
{
    public class TareaService
    {
        private readonly ConexionDB conexion = new ConexionDB();

        public List<TareaModel> ObtenerTareas(int idProyecto)
        {
            return conexion.ObtenerTareas(idProyecto);
        }

        public string AgregarTarea(TareaModel tarea)
        {
            if (string.IsNullOrWhiteSpace(tarea.NombreTarea))
                return "El nombre es obligatorio";

            if (tarea.EncargadoId == null)
                return "Debe seleccionar un encargado";

            return conexion.AgregarTarea(tarea) ? "OK" : "Error al guardar";
        }


        public bool CambiarEstadoTarea(int idTarea, bool completada)
        {
            return conexion.ActualizarEstadoTarea(idTarea, completada);
        }
    }
}
