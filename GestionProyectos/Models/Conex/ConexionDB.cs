using GestionProyectos.Models;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;


namespace GestionProyectos.Models.Conex
{
    public class ConexionDB
    {
        string stringConex = "server= localhost; user=root; database=gestion_proyectos; password=; port=3306;";
        public UsuarioModel GetUsuario(int documento)
        {
            UsuarioModel usuario = new UsuarioModel();
            string query = "SELECT * FROM usuarios WHERE numero_documento = @documento";


            using (MySqlConnection mySqlConnection = new MySqlConnection(stringConex))
            {
                using (MySqlCommand command = new MySqlCommand(query, mySqlConnection))
                {
                    command.Parameters.AddWithValue("@documento", documento);
                    mySqlConnection.Open();
                    using (MySqlDataReader reader = command.ExecuteReader())

                    {
                        if (reader.Read())
                        {
                            usuario.NumeroDocumento = reader.GetInt32("numero_documento");
                            usuario.Nombre = reader.GetString("nombre");
                            usuario.Apellido = reader.GetString("apellido");
                            usuario.Correo = reader.GetString("correo");
                            usuario.Contrasena = reader.GetString("contrasena");
                            usuario.Telefono = reader.GetInt32("telefono");
                            usuario.NombreRol = reader.GetString("nombre_rol");
                        }
                        else {
                            usuario = null;

                        }
                    }

                }
           

        }
            return usuario;

        }
        public Boolean AgregarUsuarios(UsuarioModel usuario)
        {
            string queryInsert = "INSERT INTO usuarios (numero_documento, nombre, apellido, correo, contrasena, telefono, rol) " +
                "VALUES (@documento, @nombre, @apellido,@correo,@contrasena,@telefono, @rol)";
            using (MySqlConnection mySqlConnection = new MySqlConnection(stringConex))
            {
                using (MySqlCommand command = new MySqlCommand(queryInsert, mySqlConnection))
                {
                    command.Parameters.AddWithValue("@documento", usuario.NumeroDocumento);
                    command.Parameters.AddWithValue("@nombre", usuario.Nombre);
                    command.Parameters.AddWithValue("@apellido", usuario.Apellido);
                    command.Parameters.AddWithValue("@correo", usuario.Correo);
                    command.Parameters.AddWithValue("@contrasena", usuario.Contrasena);
                    command.Parameters.AddWithValue("@telefono", usuario.Telefono);
                    command.Parameters.AddWithValue("@rol", usuario.Rol);
                    mySqlConnection.Open();
                    int result = command.ExecuteNonQuery();
                    if (result > 0)
                    {
                        return true;
                    }

                }

            }
            return false;
        }
        

        public UsuarioModel GetUsuarioCorreo(string correo)
        {

         string query = "SELECT * FROM usuarios WHERE correo = @correo";

         using (MySqlConnection mySqlConnection = new MySqlConnection(stringConex))
            {
             using(MySqlCommand command = new MySqlCommand(query, mySqlConnection))
                {
                    command.Parameters.AddWithValue("@correo", correo);
                    mySqlConnection.Open();
                    using (MySqlDataReader reader = command.ExecuteReader())

                    {
                        if (reader.Read())
                        {
                            return new UsuarioModel
                            {
                           
                            NumeroDocumento = reader.GetInt32("numero_documento"),
                            Nombre = reader.GetString("nombre"),
                            Apellido = reader.GetString("apellido"),
                            Correo = reader.GetString("correo"),
                            Contrasena = reader.GetString("contrasena"),
                            Telefono = reader.GetInt32("telefono"),
                            Rol = reader.GetInt32("rol"),
                            };
                           
                        }
                    }
                }

            }
            return null;
        }

        public List<RolModel> ObtenerRoles()
        {
            List<RolModel> roles = new List<RolModel>();
            string query = "SELECT id_rol, nombre_rol FROM roles ORDER BY id_rol";

            using (MySqlConnection mySqlConnection = new MySqlConnection(stringConex))
            {
                using (MySqlCommand command = new MySqlCommand(query, mySqlConnection))
                {
                    mySqlConnection.Open();
                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            roles.Add(new RolModel
                            {
                                IdRol = reader.GetInt32("id_rol"),
                                NombreRol = reader.GetString("nombre_rol")
                            });
                        }
                    }
                }
            }
            return roles;
        }

        // NUEVO: Método para obtener todos los usuarios
        public List<UsuarioModel> ObtenerUsuarios()
        {
            List<UsuarioModel> usuarios = new List<UsuarioModel>();
            string query = @"SELECT u.numero_documento, u.nombre, u.apellido, u.correo, 
                                   u.contrasena, u.telefono, u.id_rol, r.nombre_rol
                            FROM usuarios u
                            INNER JOIN roles r ON u.id_rol = r.id_rol
                            ORDER BY u.numero_documento";

            using (MySqlConnection mySqlConnection = new MySqlConnection(stringConex))
            {
                using (MySqlCommand command = new MySqlCommand(query, mySqlConnection))
                {
                    mySqlConnection.Open();
                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            usuarios.Add(new UsuarioModel
                            {
                                NumeroDocumento = reader.GetInt32("numero_documento"),
                                Nombre = reader.GetString("nombre"),
                                Apellido = reader.GetString("apellido"),
                                Correo = reader.GetString("correo"),
                                Contrasena = reader.GetString("contrasena"),
                                Telefono = reader.GetInt32("telefono"),
                                Rol = reader.GetInt32("id_rol"),
                                NombreRol = reader.GetString("nombre_rol")
                            });
                        }
                    }
                }
            }
            return usuarios;
        }

        public bool AgregarProyecto(ProyectoModel proyecto)
        {
            string query = @"INSERT INTO proyectos (nombre, descripcion, fecha_inicio, fecha_fin, id_estado, numero_documento)
                     VALUES (@nombre, @descripcion, @fechaInicio, @fechaFin, @idEstado, @numeroDocumento)";

            using (MySqlConnection mySqlConnection = new MySqlConnection(stringConex))
            {
                using (MySqlCommand command = new MySqlCommand(query, mySqlConnection))
                {
                    command.Parameters.AddWithValue("@nombre", proyecto.Nombre);
                    command.Parameters.AddWithValue("@descripcion", proyecto.Descripcion);
                    command.Parameters.AddWithValue("@fechaInicio", proyecto.FechaInicio);
                    command.Parameters.AddWithValue("@fechaFin", proyecto.FechaFin);
                    command.Parameters.AddWithValue("@idEstado", proyecto.IdEstado);
                    command.Parameters.AddWithValue("@numeroDocumento", proyecto.NumeroDocumento);

                    mySqlConnection.Open();
                    int result = command.ExecuteNonQuery();
                    return result > 0;
                }
            }
        }

        public List<ProyectoModel> ObtenerProyectos(int numeroDocumento)
        {
            List<ProyectoModel> proyectos = new List<ProyectoModel>();
            string query = @"SELECT p.id_proyecto, p.nombre, p.descripcion, p.fecha_inicio, p.fecha_fin, 
                            p.id_estado, e.nombre_estado, p.numero_documento
                     FROM proyectos p
                     INNER JOIN estados_proyecto e ON p.id_estado = e.id_estado
                        WHERE p.numero_documento = @numeroDocumento
                        AND p.id_estado <> 4
                     ORDER BY p.id_proyecto DESC";

            using (MySqlConnection mySqlConnection = new MySqlConnection(stringConex))
            {
                using (MySqlCommand command = new MySqlCommand(query, mySqlConnection))
                {
                    command.Parameters.AddWithValue("@numeroDocumento", numeroDocumento);
                    {
                        mySqlConnection.Open();
                        using (MySqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                proyectos.Add(new ProyectoModel
                                {
                                    IdProyecto = reader.GetInt32("id_proyecto"),
                                    Nombre = reader.GetString("nombre"),
                                    Descripcion = reader.GetString("descripcion"),
                                    FechaInicio = reader.GetDateTime("fecha_inicio"),
                                    FechaFin = reader.GetDateTime("fecha_fin"),
                                    IdEstado = reader.GetInt32("id_estado"),
                                    NombreEstado = reader.GetString("nombre_estado"),
                                    NumeroDocumento = reader.GetInt32("numero_documento")
                                });
                            }
                        }
                    }
                }

                return proyectos;
            }
        }

        public List<EstadoProyectoModel> ObtenerEstadosProyecto()
        {
            List<EstadoProyectoModel> estados = new List<EstadoProyectoModel>();
            string query = "SELECT id_estado, nombre_estado FROM estados_proyecto ORDER BY id_estado";

            using (MySqlConnection mySqlConnection = new MySqlConnection(stringConex))
            {
                using (MySqlCommand command = new MySqlCommand(query, mySqlConnection))
                {
                    mySqlConnection.Open();
                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            estados.Add(new EstadoProyectoModel
                            {
                                IdEstado = reader.GetInt32("id_estado"),
                                NombreEstado = reader.GetString("nombre_estado")
                            });
                        }
                    }
                }
            }

            return estados;
        }

        public string ActualizarProyecto(ProyectoModel proyecto)
        {
            try
            {
                using (var connection = new MySqlConnection(stringConex))
                {
                    connection.Open();
                    string query = @"UPDATE proyectos 
                             SET nombre=@nombre, descripcion=@descripcion,
                                 fecha_inicio=@fecha_inicio, fecha_fin=@fecha_fin,
                                 id_estado=@id_estado
                             WHERE id_proyecto=@id_proyecto";

                    using (var cmd = new MySqlCommand(query, connection))
                    {
                        cmd.Parameters.AddWithValue("@nombre", proyecto.Nombre);
                        cmd.Parameters.AddWithValue("@descripcion", proyecto.Descripcion);
                        cmd.Parameters.AddWithValue("@fecha_inicio", proyecto.FechaInicio);
                        cmd.Parameters.AddWithValue("@fecha_fin", proyecto.FechaFin);
                        cmd.Parameters.AddWithValue("@id_estado", proyecto.IdEstado);
                        cmd.Parameters.AddWithValue("@id_proyecto", proyecto.IdProyecto);

                        cmd.ExecuteNonQuery();
                    }
                }
                return "OK";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }


    }

}

