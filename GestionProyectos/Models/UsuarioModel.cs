using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionProyectos.Models
{
    public class UsuarioModel
    {
        private int? NumeroDocumento { get; set; }
        private String? Nombre { get; set; }
        private String? Apellido { get; set; }
        private String? Correo { get; set; }
        private String? Contrasena { get; set; }
        private String? Telefono { get; set; }

        public UsuarioModel(int numeroDocumento, String nombre, String apellido, String correo, String contrasena, String telefono) 
        { 
            NumeroDocumento = numeroDocumento;
            Nombre = nombre;
            Apellido = apellido;
            Correo = correo;
            Contrasena = contrasena;
            Telefono = telefono;

        }

    }
}
