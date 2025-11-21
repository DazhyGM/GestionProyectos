using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Data.SqlClient;
using GestionProyectos.Models;
using GestionProyectos.Services;
using System.Collections.Generic;

namespace GestionProyectos.Controllers
{
    public class TareaController
    {
        private readonly TareaService tareaService = new TareaService();

        public List<TareaModel> ObtenerTareas(int idProyecto) => tareaService.ObtenerTareas(idProyecto);
        public string AgregarTarea(TareaModel tarea) => tareaService.AgregarTarea(tarea);
        public bool CambiarEstadoTarea(int idTarea, bool completada) => tareaService.CambiarEstadoTarea(idTarea, completada);
    }
}

