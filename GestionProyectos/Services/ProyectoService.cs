using GestionProyectos.Models;
using GestionProyectos.Models.Conex;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;

namespace GestionProyectos.Services
{
    public class ProyectoService
    {
        private readonly ConexionDB conexion = new ConexionDB();


        public string RegistrarProyecto(ProyectoModel proyecto)
        {
            if (proyecto == null)
                return "Datos inválidos.";

            if (string.IsNullOrWhiteSpace(proyecto.Nombre))
                return "El nombre del proyecto es obligatorio.";

            if (proyecto.FechaInicio > proyecto.FechaFin)
                return "La fecha de inicio no puede ser posterior a la fecha de fin.";

            bool insertado = conexion.AgregarProyecto(proyecto);
            return insertado ? "OK" : "No se pudo registrar el proyecto.";
        }


        public List<ProyectoModel> ObtenerProyectos(int numeroDocumento)
        {
            return conexion.ObtenerProyectos(numeroDocumento);
        }

        public List<EstadoProyectoModel> ObtenerEstados()
        {
            return conexion.ObtenerEstadosProyecto();
        }
        public string ActualizarProyecto(ProyectoModel proyecto)
        {
            return conexion.ActualizarProyecto(proyecto);
        }
        public bool CambiarEstado(int idProyecto, int idNuevoEstado)
        {
            return conexion.ActualizarEstadoProyecto(idProyecto, idNuevoEstado);
        }

        public List<ProyectoModel> ObtenerPInactivos(int numeroDocumento)
        {
            return conexion.ObtenerPInactivos(numeroDocumento);
        }

        public string ActivarProyecto(ProyectoModel proyecto)
        {
            return conexion.ActivarProyecto(proyecto);
        }
        public List<ProyectoModel> ObtenerProyectosPorVencer()
        {
            List<ProyectoModel> lista = new List<ProyectoModel>();

            string query = @"
        SELECT id_proyecto, nombre, descripcion, fecha_inicio, fecha_fin, id_estado, numero_documento
        FROM proyectos
        WHERE DATE(fecha_fin) = DATE(@fechaVencimiento)
          AND id_estado <> 4;
    ";

            using (var conn = conexion.GetConnection())
            using (var cmd = new MySqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@fechaVencimiento", DateTime.Now.AddDays(1).Date);

                conn.Open();
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        lista.Add(new ProyectoModel
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

            return lista;
        }



    }
}
