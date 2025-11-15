using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionProyectos.Models
{
    public class UsuarioModel
    {
        public int? NumeroDocumento { get; set; }
        public string? Nombre { get; set; }
        public string? Apellido { get; set; }
        public string? Correo { get; set; }
        public string? Contrasena { get; set; }
        public string? Telefono { get; set; }
        public int Rol { get; set; } = 4;
        public string NombreRol { get; set; }

        public UsuarioModel() { }
        public UsuarioModel(int numeroDocumento, string nombre, string apellido, string correo, string contrasena, string telefono) 
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
