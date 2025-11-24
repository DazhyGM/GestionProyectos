using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionProyectos.Models
{
    public class GraficasModel
    {

        public int Cantidad { get; set; }
        public string? NombreEstado { get; set; }
        public int EncargadoId { get; set; }
        public int? NumeroDocumento { get; set; }
        public string? Nombre { get; set; }

        public string DisplayText
        {
            get { return $"{Nombre} ({NumeroDocumento})"; }
        }
    }


}
