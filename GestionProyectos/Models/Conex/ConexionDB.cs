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
                            usuario.Telefono = reader.GetString("telefono");
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
                            Telefono = reader.GetString("telefono"),
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
                                Telefono = reader.GetString("telefono"),
                                Rol = reader.GetInt32("id_rol"),
                                NombreRol = reader.GetString("nombre_rol")
                            });
                        }
                    }
                }
            }
            return usuarios;
        }

        public List<UsuarioModel> ObtenerColaboradores()
        {
            List<UsuarioModel> colaboradores = new List<UsuarioModel>();
            string query = @"SELECT numero_documento, nombre, apellido 
                             FROM usuarios 
                             WHERE id_rol = 4";

            using (MySqlConnection connection = new MySqlConnection(stringConex))
            {
                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    connection.Open();
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            colaboradores.Add(new UsuarioModel
                            {
                                NumeroDocumento = reader.GetInt32("numero_documento"),
                                Nombre = reader.GetString("nombre"),
                                Apellido = reader.GetString("apellido")
                            });
                        }
                    }
                }
            }
            return colaboradores;
        }

        public bool CompartirProyecto(int idProyecto, int numeroDocumento)
        {
            string query = @"INSERT INTO proyectos_usuarios (id_proyecto, numero_documento)
                             VALUES (@idProyecto, @numeroDocumento)";

            using (var connection = new MySqlConnection(stringConex))
            {
                using (var cmd = new MySqlCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@idProyecto", idProyecto);
                    cmd.Parameters.AddWithValue("@numeroDocumento", numeroDocumento);

                    connection.Open();
                    return cmd.ExecuteNonQuery() > 0;
                }
            }
        }

        public List<UsuarioModel> ObtenerUsuariosProyecto(int idProyecto)
        {
            List<UsuarioModel> lista = new List<UsuarioModel>();

            string query = @"SELECT u.numero_documento, u.nombre, u.apellido
                             FROM proyectos_usuarios pu
                             INNER JOIN usuarios u ON pu.numero_documento = u.numero_documento
                             WHERE pu.id_proyecto = @idProyecto";

            using (var con = new MySqlConnection(stringConex))
            {
                using (var cmd = new MySqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@idProyecto", idProyecto);
                    con.Open();
                    using (var r = cmd.ExecuteReader())
                    {
                        while (r.Read())
                        {
                            lista.Add(new UsuarioModel
                            {
                                NumeroDocumento = r.GetInt32("numero_documento"),
                                Nombre = r.GetString("nombre"),
                                Apellido = r.GetString("apellido")
                            });
                        }
                    }
                }
            }

            return lista;
        }

        public bool EliminarUsuarioCompartido(int idProyecto, int numeroDocumento)
        {
            string query = @"DELETE FROM proyectos_usuarios
                             WHERE id_proyecto = @idProyecto AND numero_documento = @numeroDocumento";

            using (var con = new MySqlConnection(stringConex))
            {
                using (var cmd = new MySqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@idProyecto", idProyecto);
                    cmd.Parameters.AddWithValue("@numeroDocumento", numeroDocumento);

                    con.Open();
                    return cmd.ExecuteNonQuery() > 0;
                }
            }
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

            string query = @"
        SELECT DISTINCT p.id_proyecto, p.nombre, p.descripcion, p.fecha_inicio, p.fecha_fin, 
                        p.id_estado, e.nombre_estado, p.numero_documento,
                        COALESCE(
                            ROUND(
                                (SUM(CASE WHEN t.completada = 1 THEN 1 ELSE 0 END) / COUNT(t.id_tarea)) * 100
                            ), 0
                        ) AS progreso
        FROM proyectos p
        INNER JOIN estados_proyecto e ON p.id_estado = e.id_estado
        LEFT JOIN tareas t ON p.id_proyecto = t.id_proyecto
        LEFT JOIN proyectos_usuarios pu ON p.id_proyecto = pu.id_proyecto
        WHERE (p.numero_documento = @numeroDocumento OR pu.numero_documento = @numeroDocumento)
          AND p.id_estado <> 4
        GROUP BY p.id_proyecto, p.nombre, p.descripcion, p.fecha_inicio, p.fecha_fin, 
                 p.id_estado, e.nombre_estado, p.numero_documento
        ORDER BY p.id_proyecto DESC;
    ";

            using (var mySqlConnection = new MySqlConnection(stringConex))
            {
                using (var command = new MySqlCommand(query, mySqlConnection))
                {
                    command.Parameters.AddWithValue("@numeroDocumento", numeroDocumento);
                    mySqlConnection.Open();
                    using (var reader = command.ExecuteReader())
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
                                NumeroDocumento = reader.GetInt32("numero_documento"),
                                Progreso = reader.GetInt32("progreso")
                            });
                        }
                    }
                }
            }
            return proyectos;
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

        public bool ActualizarEstadoProyecto(int idProyecto, int idNuevoEstado)
        {
            string query = "UPDATE proyectos SET id_estado = @estado WHERE id_proyecto = @id";

            using (MySqlConnection mySqlConnection = new MySqlConnection(stringConex))
            {
                using (MySqlCommand command = new MySqlCommand(query, mySqlConnection))
                {
                    command.Parameters.AddWithValue("@estado", idNuevoEstado);
                    command.Parameters.AddWithValue("@id", idProyecto);

                    mySqlConnection.Open();
                    int result = command.ExecuteNonQuery();
                    return result > 0;
                }
            }
        }

        public List<TareaModel> ObtenerTareas(int idProyecto)
        {
            List<TareaModel> tareas = new List<TareaModel>();
            string query = @"SELECT id_tarea, id_proyecto, nombre_tarea, encargado_id, completada
                     FROM tareas 
                     WHERE id_proyecto = @idProyecto";

            using (MySqlConnection mySqlConnection = new MySqlConnection(stringConex))
            {
                using (MySqlCommand command = new MySqlCommand(query, mySqlConnection))
                {
                    command.Parameters.AddWithValue("@idProyecto", idProyecto);
                    mySqlConnection.Open();

                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            tareas.Add(new TareaModel
                            {
                                IdTarea = reader.GetInt32("id_tarea"),
                                IdProyecto = reader.GetInt32("id_proyecto"),
                                NombreTarea = reader.GetString("nombre_tarea"),
                                EncargadoId = reader.GetInt32("encargado_id"),
                                Completada = reader.GetBoolean("completada")
                            });
                        }
                    }
                }
            }
            return tareas;
        }


        public bool AgregarTarea(TareaModel tarea)
        {
            string query = @"INSERT INTO tareas (id_proyecto, nombre_tarea, completada, encargado_id)
                     VALUES (@idProyecto, @nombreTarea, 0, @encargadoId)";

            using (MySqlConnection mySqlConnection = new MySqlConnection(stringConex))
            {
                using (MySqlCommand command = new MySqlCommand(query, mySqlConnection))
                {
                    command.Parameters.AddWithValue("@idProyecto", tarea.IdProyecto);
                    command.Parameters.AddWithValue("@nombreTarea", tarea.NombreTarea);
                    command.Parameters.AddWithValue("@encargadoId", tarea.EncargadoId);

                    mySqlConnection.Open();
                    int result = command.ExecuteNonQuery();
                    return result > 0;
                }
            }
        }


        public bool ActualizarEstadoTarea(int idTarea, bool completada)
        {
            string query = "UPDATE tareas SET completada = @completada WHERE id_tarea = @id";

            using (MySqlConnection mySqlConnection = new MySqlConnection(stringConex))
            {
                using (MySqlCommand command = new MySqlCommand(query, mySqlConnection))
                {
                    command.Parameters.AddWithValue("@completada", completada);
                    command.Parameters.AddWithValue("@id", idTarea);

                    mySqlConnection.Open();
                    int result = command.ExecuteNonQuery();
                    return result > 0;
                }
            }
        }

        public List<ProyectoModel> ObtenerPInactivos(int numeroDocumento)
        {
            List<ProyectoModel> proyectos = new List<ProyectoModel>();
            string query = @"SELECT id_proyecto, nombre, descripcion, fecha_inicio, fecha_fin, 
                            id_estado, numero_documento
                     FROM proyectos
                        WHERE id_estado = 4 AND numero_documento = @numeroDocumento";

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
                                    NumeroDocumento = reader.GetInt32("numero_documento")
                                });
                            }
                        }
                    }
                }

                return proyectos;
            }
        }

        public string ActivarProyecto(ProyectoModel proyecto)
        {
            try
            {
                using (var connection = new MySqlConnection(stringConex))
                {
                    connection.Open();
                    string query = @"UPDATE proyectos SET Id_Estado = 1 WHERE Id_Proyecto = @Id_Proyecto";

                    using (var cmd = new MySqlCommand(query, connection))
                    {
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

        public List<UsuarioModel> BuscarColaboradores(string filtro)
        {
            List<UsuarioModel> lista = new List<UsuarioModel>();

            try
            {
                using (var connection = new MySqlConnection(stringConex))
                {
                    connection.Open();

                    string sql = @"
                SELECT numero_documento, nombre, apellido, correo, telefono, rol
                FROM usuarios
                WHERE rol = 4
                AND (
                        numero_documento LIKE @filtro
                     OR nombre LIKE @filtro
                     OR apellido LIKE @filtro
                    )
                LIMIT 10;
            ";

                    using (var cmd = new MySqlCommand(sql, connection))
                    {
                        cmd.Parameters.AddWithValue("@filtro", "%" + filtro + "%");

                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                lista.Add(new UsuarioModel
                                {
                                    NumeroDocumento = reader["numero_documento"] as int?,
                                    Nombre = reader["nombre"].ToString(),
                                    Apellido = reader["apellido"].ToString(),
                                    Correo = reader["correo"].ToString(),
                                    Telefono = reader["telefono"].ToString(),
                                    Rol = Convert.ToInt32(reader["rol"])
                                });
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Error en BuscarColaboradores: " + ex.Message);
            }

            return lista;
        }

        public List<UsuarioModel> ObtenerUsuariosPorRol(int rol)
        {
            List<UsuarioModel> lista = new List<UsuarioModel>();

            string query = @"SELECT numero_documento, nombre, apellido, correo, telefono, id_rol
                     FROM usuarios
                     WHERE id_rol = @rol";

            using (var conn = new MySqlConnection(stringConex))
            using (var cmd = new MySqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@rol", rol);
                conn.Open();

                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        lista.Add(new UsuarioModel
                        {
                            NumeroDocumento = reader.GetInt32("numero_documento"),
                            Nombre = reader.GetString("nombre"),
                            Apellido = reader.GetString("apellido"),
                            Correo = reader.GetString("correo"),
                            Telefono = reader.GetString("telefono"),
                            Rol = reader.GetInt32("id_rol")
                        });
                    }
                }
            }

            return lista;
        }


    }

}

