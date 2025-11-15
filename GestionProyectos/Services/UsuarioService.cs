using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using GestionProyectos.Models;
using GestionProyectos.Models.Conex;

namespace GestionProyectos.Services
{
    public class UsuarioService
     
    {
       private readonly ConexionDB conexion = new ConexionDB();
         
        public string RegistrarUsuarios(UsuarioModel usuario)
        {
            if (usuario == null)
                return "datos no validos";

            if (string.IsNullOrWhiteSpace(usuario.Nombre))
                return "El nombre es obligatorio.";

            if (string.IsNullOrWhiteSpace(usuario.Apellido))
                return "El apellido es obligatorio.";

            if (string.IsNullOrWhiteSpace(usuario.Correo))
                return "El correo electrónico es obligatorio.";

            if (!CorreoValido(usuario.Correo))
                return "Correo electrónico inválido. formato ejemplo@gmail.com";

            if (usuario.NumeroDocumento== null || usuario.NumeroDocumento <=0)
                return "Número de documento invalido, solo deben ser numeros";

            if (string.IsNullOrWhiteSpace(usuario.Contrasena))
                return "La contraseña no puede estar vacía.";


            if (string.IsNullOrWhiteSpace(usuario.Telefono))
                return "El número de teléfono es inválido.";
            var usuarioExistente = conexion.GetUsuario(usuario.NumeroDocumento.Value);
            
            if (usuarioExistente != null && usuarioExistente.NumeroDocumento.HasValue)
            {
                return "El número de documento ya está registrado.";
            }
        
            try
            {
                bool insertado = conexion.AgregarUsuarios(usuario);
                return insertado ? "OK" : "No se pudo guardar el usuario en la base de datos.";
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Error en UsuarioService: " + ex.Message);
                return "Error al registrar el usuario. Intente nuevamente.";
            }
        }



        private bool CorreoValido(string correo)
        {
            if (string.IsNullOrEmpty(correo)) return false;
            string regex = @"^[A-Za-z0-9+_.-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,6}$";
            return Regex.IsMatch(correo, regex);
        }



        public bool IniciarSesion(string correo, string contrasena)
        {
            if (string.IsNullOrWhiteSpace(correo) || string.IsNullOrWhiteSpace(contrasena))
                return false;
            var usuario = conexion.GetUsuarioCorreo(correo);
            return usuario != null && usuario.Contrasena == contrasena;
        }

        public UsuarioModel ObtenerUsuarioPorCredenciales(string correo, string contrasena)
        {
            if (string.IsNullOrWhiteSpace(correo) || string.IsNullOrWhiteSpace(contrasena))
                return null;

            try
            {
                var usuario = conexion.GetUsuarioCorreo(correo);

                
                if (usuario != null && usuario.Contrasena == contrasena)
                {
                    return usuario;
                }

                return null;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Error en ObtenerUsuarioPorCredenciales: " + ex.Message);
                return null;
            }
        }
    }
}
