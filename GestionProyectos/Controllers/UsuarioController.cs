using GestionProyectos.Models;
using GestionProyectos.Services;

namespace GestionProyectos.Controllers
{
    class UsuarioController
    {
        private readonly UsuarioService usuarioService = new UsuarioService();



        public string RegistrarUsuarios(int documento, string nombre, string apellido, string correo, string contrasena, int telefono)
        {
            UsuarioModel usuario = new UsuarioModel
            {
                NumeroDocumento = documento,
                Nombre = nombre,
                Apellido = apellido,
                Correo = correo,
                Contrasena = contrasena,
                Telefono = telefono

            };
            return usuarioService.RegistrarUsuarios(usuario);


        }


    }
}
