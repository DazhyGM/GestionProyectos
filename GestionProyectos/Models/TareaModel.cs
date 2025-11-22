using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionProyectos.Models
{
    public class TareaModel
    {
        public int IdTarea { get; set; }
        public int IdProyecto { get; set; }

        public string NombreTarea { get; set; }

        public int? EncargadoId { get; set; }

        public bool Completada { get; set; }
    }
}

