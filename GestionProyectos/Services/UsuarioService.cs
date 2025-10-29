using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using GestionProyectos.Models;

namespace GestionProyectos.Services
{
    public class UsuarioService
    {
        // 🧠 Lista temporal para simular base de datos
        private static List<UsuarioModel> usuariosSimulados = new List<UsuarioModel>();

        public string AgregarUsuario(UsuarioModel usuario)
        {
            if (!CorreoValido(usuario.Correo))
                return "Correo electrónico inválido. No se puede registrar el usuario.";

            if (!DocumentoValido(usuario.NumeroDocumento))
                return "Número de documento inválido. Debe contener solo números.";

            // Verificar duplicados (simulado)
            if (usuariosSimulados.Exists(u => u.NumeroDocumento == usuario.NumeroDocumento))
                return "Ya existe un usuario registrado con ese número de documento.";

            if (usuariosSimulados.Exists(u => u.Correo == usuario.Correo))
                return "Ya existe un usuario registrado con ese correo electrónico.";

            usuariosSimulados.Add(usuario);
            return "OK";
        }

        public bool VerificarUsuario(string correo, string contrasena)
        {
            var usuario = usuariosSimulados.Find(u =>
                u.Correo == correo && u.Contrasena == contrasena);

            if (usuario != null)
            {
                SesionUsuario.UsuarioActual = usuario;
                Console.WriteLine($"Usuario autenticado: {usuario.Nombre}");
                return true;
            }

            Console.WriteLine("Usuario no encontrado.");
            return false;
        }

        public List<UsuarioModel> ListarUsuarios()
        {
            Console.WriteLine($"Usuarios listados correctamente. Total: {usuariosSimulados.Count}");
            return new List<UsuarioModel>(usuariosSimulados);
        }

        public UsuarioModel ObtenerUsuarioPorCorreo(string correo)
        {
            return usuariosSimulados.Find(u => u.Correo == correo);
        }

        private bool CorreoValido(string correo)
        {
            if (string.IsNullOrEmpty(correo)) return false;
            string regex = @"^[A-Za-z0-9+_.-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,6}$";
            return Regex.IsMatch(correo, regex);
        }

        private bool DocumentoValido(string numeroDocumento)
        {
            if (string.IsNullOrEmpty(numeroDocumento)) return false;
            return Regex.IsMatch(numeroDocumento, @"^\d+$");
        }
    }
}
