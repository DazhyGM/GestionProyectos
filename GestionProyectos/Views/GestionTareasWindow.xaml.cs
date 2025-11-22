using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Diagnostics;
using GestionProyectos.Controllers;
using GestionProyectos.Models;
using GestionProyectos.Services;
using GestionProyectos.Models.Conex;

namespace GestionProyectos.Views
{
    public partial class GestionTareasWindow : Window
    {
        private int idProyectoActual;

        private readonly TareaController tareaController = new TareaController();
        private readonly UsuarioService usuarioService = new UsuarioService();
        private readonly ConexionDB conexion = new ConexionDB();

        private List<UsuarioModel> colaboradores = new List<UsuarioModel>();

        public GestionTareasWindow(int idProyecto, string nombreProyecto)
        {
            InitializeComponent();
            idProyectoActual = idProyecto;

            this.Title = $"Tareas - {nombreProyecto}";

            CargarColaboradores();
            CargarTareas();
            CargarColaboradoresProyecto();
        }

        // ============================================================
        // 1. Cargar colaboradores (rol 4)
        // ============================================================
        private void CargarColaboradores()
        {
            try
            {
                colaboradores = usuarioService.ObtenerColaboradores() ?? new List<UsuarioModel>();
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error cargando colaboradores: " + ex.Message);
                colaboradores = new List<UsuarioModel>();
            }
        }

        // ============================================================
        // 2. Cargar tareas del proyecto
        // ============================================================
        private void CargarTareas()
        {
            try
            {
                var tareas = tareaController.ObtenerTareas(idProyectoActual) ?? new List<TareaModel>();
                listaTareas.ItemsSource = tareas;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error cargando tareas: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // ============================================================
        // 3. Cargar colaboradores ya agregados al proyecto
        // ============================================================
        private void CargarColaboradoresProyecto()
        {
            try
            {
                var lista = conexion.ObtenerUsuariosProyecto(idProyectoActual) ?? new List<UsuarioModel>();
                lstColaboradoresProyecto.ItemsSource = lista;
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error cargando colaboradores del proyecto: " + ex.Message);
                lstColaboradoresProyecto.ItemsSource = new List<UsuarioModel>();
            }
        }

        // ============================================================
        // 4. Agregar tarea
        // ============================================================
        private void BtnAgregarTarea_Click(object sender, RoutedEventArgs e)
        {
            string nombreTarea = txtNombreTarea.Text.Trim();

            if (string.IsNullOrWhiteSpace(nombreTarea))
            {
                MessageBox.Show("Debe ingresar una descripción para la tarea.");
                return;
            }

            if (string.IsNullOrWhiteSpace(txtDocumentoEncargado.Text))
            {
                MessageBox.Show("Debe seleccionar un encargado para la tarea.");
                return;
            }

            if (!int.TryParse(txtDocumentoEncargado.Text, out int encargadoId))
            {
                MessageBox.Show("El documento del encargado no es válido.");
                return;
            }

            TareaModel nuevaTarea = new TareaModel
            {
                IdProyecto = idProyectoActual,
                NombreTarea = nombreTarea,
                EncargadoId = encargadoId,
                Completada = false
            };

            string mensaje = tareaController.AgregarTarea(nuevaTarea);

            if (mensaje == "OK")
            {
                MessageBox.Show("Tarea registrada correctamente.");
                CargarTareas();
                LimpiarCampos();
            }
            else
            {
                MessageBox.Show("Error: " + mensaje);
            }
        }

        // ============================================================
        // 5. Cambiar estado de la tarea
        // ============================================================
        private void CheckBox_CheckChanged(object sender, RoutedEventArgs e)
        {
            try
            {
                if (sender is CheckBox chk && chk.Tag != null)
                {
                    if (int.TryParse(chk.Tag.ToString(), out int idTarea))
                    {
                        bool estado = chk.IsChecked ?? false;
                        tareaController.CambiarEstadoTarea(idTarea, estado);
                        CargarTareas();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cambiar estado: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // ============================================================
        // 6. Autocompletar encargado (buscador)
        // ============================================================
        private void txtDocumentoEncargado_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                string texto = txtDocumentoEncargado.Text.Trim().ToLower();

                if (string.IsNullOrWhiteSpace(texto))
                {
                    lstColaboradores.Visibility = Visibility.Collapsed;
                    lstColaboradores.ItemsSource = null;
                    return;
                }

                var filtrados = colaboradores
                    .Where(x =>
                        (x.NumeroDocumento.HasValue &&
                         x.NumeroDocumento.Value.ToString().Contains(texto)) ||

                        (!string.IsNullOrEmpty(x.Nombre) &&
                         x.Nombre.ToLower().Contains(texto)) ||

                        (!string.IsNullOrEmpty(x.Apellido) &&
                         x.Apellido.ToLower().Contains(texto))
                    )
                    .ToList();

                lstColaboradores.ItemsSource = filtrados;
                lstColaboradores.Visibility = filtrados.Any() ? Visibility.Visible : Visibility.Collapsed;
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error en búsqueda encargado: " + ex.Message);
            }
        }

        private void lstColaboradores_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (lstColaboradores.SelectedItem is UsuarioModel seleccionado)
                {
                    txtDocumentoEncargado.Text = seleccionado.NumeroDocumento?.ToString() ?? "";
                    txtNombreEncargado.Text = seleccionado.NombreCompleto;
                    lstColaboradores.Visibility = Visibility.Collapsed;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error seleccionando encargado: " + ex.Message);
            }
        }

        // ============================================================
        // 7. Buscar colaborador (sección proyecto)
        // ============================================================
        private void BtnBuscarColaborador_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string filtro = txtBuscarColaborador.Text.Trim();
                var lista = conexion.BuscarColaboradores(filtro) ?? new List<UsuarioModel>();
                lstColaboradoresDisponibles.ItemsSource = lista;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error buscando colaboradores: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // ============================================================
        // 8. Agregar colaborador al proyecto
        // ============================================================
        private void BtnAgregarColaborador_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (lstColaboradoresDisponibles.SelectedItem is UsuarioModel seleccionado)
                {
                    if (!seleccionado.NumeroDocumento.HasValue)
                    {
                        MessageBox.Show("El colaborador seleccionado no tiene documento válido.", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                        return;
                    }

                    bool ok = conexion.CompartirProyecto(idProyectoActual, seleccionado.NumeroDocumento.Value);

                    if (ok)
                    {
                        MessageBox.Show("Colaborador agregado correctamente.", "Información", MessageBoxButton.OK, MessageBoxImage.Information);
                        CargarColaboradoresProyecto();
                        CargarColaboradores();
                    }
                    else
                    {
                        MessageBox.Show("Error al agregar colaborador.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                else
                {
                    MessageBox.Show("Seleccione un colaborador disponible para agregar.", "Validación", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error agregando colaborador: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // ============================================================
        // 9. Eliminar colaborador del proyecto
        // ============================================================
        private void BtnEliminarColaborador_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (lstColaboradoresProyecto.SelectedItem is UsuarioModel seleccionado)
                {
                    if (!seleccionado.NumeroDocumento.HasValue)
                    {
                        MessageBox.Show("El colaborador seleccionado no tiene documento válido.", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                        return;
                    }

                    var confirm = MessageBox.Show(
                        $"¿Eliminar al colaborador {seleccionado.NombreCompleto} del proyecto?",
                        "Confirmar",
                        MessageBoxButton.YesNo,
                        MessageBoxImage.Question
                    );

                    if (confirm != MessageBoxResult.Yes) return;

                    bool ok = conexion.EliminarUsuarioCompartido(idProyectoActual, seleccionado.NumeroDocumento.Value);

                    if (ok)
                    {
                        MessageBox.Show("Colaborador eliminado correctamente.", "Información", MessageBoxButton.OK, MessageBoxImage.Information);
                        CargarColaboradoresProyecto();
                        CargarColaboradores();
                    }
                    else
                    {
                        MessageBox.Show("No se pudo eliminar el colaborador.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                else
                {
                    MessageBox.Show("Seleccione un colaborador del proyecto para eliminar.", "Validación", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error eliminando colaborador: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // ============================================================
        // 10. Placeholder del buscador
        // ============================================================
        private void TxtBuscarColaborador_TextChanged(object sender, TextChangedEventArgs e)
        {
            PlaceholderBuscar.Visibility =
                string.IsNullOrWhiteSpace(txtBuscarColaborador.Text)
                ? Visibility.Visible
                : Visibility.Collapsed;
        }

        // ============================================================
        // 11. Limpiar campos
        // ============================================================
        private void LimpiarCampos()
        {
            txtNombreTarea.Text = "";
            txtDocumentoEncargado.Text = "";
            txtNombreEncargado.Text = "";
        }
    }
}
