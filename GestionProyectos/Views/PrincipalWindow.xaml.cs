using GestionProyectos.Models;
using GestionProyectos.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace GestionProyectos.Views
{
    public partial class PrincipalWindow : Window
    {
        private string usuarioActual;
        private int numeroDocumentoUsuario;
        private ProyectoModel proyectoEnEdicion = null;
        private List<ProyectoModel> proyectos = new List<ProyectoModel>();
        private readonly ProyectoController proyectoController = new ProyectoController();

        public PrincipalWindow(string nombreUsuario, int numeroDocumento)
        {
            InitializeComponent();
            usuarioActual = nombreUsuario;

            numeroDocumentoUsuario = numeroDocumento;
            txtUsuarioNombre.Text = nombreUsuario;

            CargarEstados();
            CargarProyectos();
        }

        private void CargarEstados()
        {
            try
            {
                var estados = proyectoController.ObtenerEstados();
                cmbEstado.Items.Clear();

                if (estados != null && estados.Count > 0)
                {
                    foreach (var e in estados)
                    {
                        if (e.IdEstado == 4) continue;

                        var item = new ComboBoxItem { Content = e.NombreEstado, Tag = e.IdEstado };
                        cmbEstado.Items.Add(item);
                    }
                    cmbEstado.SelectedIndex = 0;
                }
                else
                {

                    cmbEstado.Items.Add(new ComboBoxItem { Content = "Pendiente", Tag = 1 });
                    cmbEstado.SelectedIndex = 0;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error cargando estados: " + ex.Message);
            }
        }


        private void CargarProyectos()
        {
            try
            {
                proyectos = (proyectoController.ObtenerProyectos(numeroDocumentoUsuario) ?? new List<ProyectoModel>())
                    .Where(p => p.IdEstado != 4)
                    .ToList();
                
                var listaParaMostrar = proyectos.Select(p => new
                {
                    Id = p.IdProyecto,                         
                    Nombre = p.Nombre ?? "",
                    Descripcion = p.Descripcion ?? "",
                    FechaInicio = p.FechaInicio,
                    FechaFin = p.FechaFin,
                    Estado = p.NombreEstado ?? (p.IdEstado.ToString())
                }).OrderByDescending(x => x.Id).ToList();

                listaProyectos.ItemsSource = listaParaMostrar;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error cargando proyectos: " + ex.Message);
            }
        }


        private void BtnCrearProyecto_Click(object sender, RoutedEventArgs e)
        {
            lblMensaje.Content = "";
            lblMensaje.Foreground = Brushes.Black;


            if (string.IsNullOrWhiteSpace(txtNombreProyecto.Text))
            {
                lblMensaje.Content = "El nombre del proyecto es obligatorio";
                lblMensaje.Foreground = Brushes.Red;
                return;
            }

            if (string.IsNullOrWhiteSpace(txtDescripcionProyecto.Text))
            {
                lblMensaje.Content = "La descripción es obligatoria";
                lblMensaje.Foreground = Brushes.Red;
                return;
            }

            if (dpFechaInicio.SelectedDate == null || dpFechaFin.SelectedDate == null)
            {
                lblMensaje.Content = "Seleccione las fechas";
                lblMensaje.Foreground = Brushes.Red;
                return;
            }

            if (dpFechaFin.SelectedDate < dpFechaInicio.SelectedDate)
            {
                lblMensaje.Content = "La fecha de fin debe ser posterior a la de inicio";
                lblMensaje.Foreground = Brushes.Red;
                return;
            }

            if (!(cmbEstado.SelectedItem is ComboBoxItem estadoSeleccionado))
            {
                lblMensaje.Content = "Seleccione un estado";
                lblMensaje.Foreground = Brushes.Red;
                return;
            }

            var proyecto = new ProyectoModel
            {
                Nombre = txtNombreProyecto.Text.Trim(),
                Descripcion = txtDescripcionProyecto.Text.Trim(),
                FechaInicio = dpFechaInicio.SelectedDate.Value,
                FechaFin = dpFechaFin.SelectedDate.Value,
                IdEstado = Convert.ToInt32(estadoSeleccionado.Tag),
                NumeroDocumento = numeroDocumentoUsuario
            };

            try
            {
                string resultado = proyectoController.RegistrarProyecto(proyecto);

                if (resultado == "OK")
                {
                    lblMensaje.Content = "Proyecto creado exitosamente";
                    lblMensaje.Foreground = Brushes.Green;
                    LimpiarFormulario();
                    CargarProyectos();
                }
                else
                {
                    lblMensaje.Content = resultado;
                    lblMensaje.Foreground = Brushes.Red;
                }
            }
            catch (Exception ex)
            {
                lblMensaje.Content = "Error al crear el proyecto: " + ex.Message;
                lblMensaje.Foreground = Brushes.Red;
            }
        }

        private void LimpiarFormulario()
        {
            txtNombreProyecto.Clear();
            txtDescripcionProyecto.Clear();
            dpFechaInicio.SelectedDate = null;
            dpFechaFin.SelectedDate = null;
            if (cmbEstado.Items.Count > 0)
                cmbEstado.SelectedIndex = 0;
        }


        private void BtnActualizar_Click(object sender, RoutedEventArgs e)
        {
            CargarProyectos();
            MessageBox.Show("Lista actualizada", "Información", MessageBoxButton.OK, MessageBoxImage.Information);
        }


        private void TxtBuscar_TextChanged(object sender, TextChangedEventArgs e)
        {
            string textoBusqueda = txtBuscar.Text.ToLower();

            if (string.IsNullOrWhiteSpace(textoBusqueda))
            {
                CargarProyectos();
                return;
            }

            var filtrados = proyectos.Where(p =>
                (p.Nombre ?? "").ToLower().Contains(textoBusqueda) ||
                (p.Descripcion ?? "").ToLower().Contains(textoBusqueda) ||
                (p.NombreEstado ?? "").ToLower().Contains(textoBusqueda)
            ).Select(p => new
            {
                Id = p.IdProyecto,
                Nombre = p.Nombre,
                Descripcion = p.Descripcion,
                FechaInicio = p.FechaInicio,
                FechaFin = p.FechaFin,
                Estado = p.NombreEstado
            }).OrderByDescending(x => x.Id).ToList();

            listaProyectos.ItemsSource = filtrados;
        }


        private void BtnEditar_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.Tag is int id)
            {
                var proyecto = proyectos.FirstOrDefault(p => p.IdProyecto == id);
                if (proyecto != null)
                {
                    proyectoEnEdicion = proyecto;

                    txtNombreProyecto.Text = proyecto.Nombre;
                    txtDescripcionProyecto.Text = proyecto.Descripcion;
                    dpFechaInicio.SelectedDate = proyecto.FechaInicio;
                    dpFechaFin.SelectedDate = proyecto.FechaFin;

                    for (int i = 0; i < cmbEstado.Items.Count; i++)
                    {
                        var item = (ComboBoxItem)cmbEstado.Items[i];
                        if (Convert.ToInt32(item.Tag) == proyecto.IdEstado)
                        {
                            cmbEstado.SelectedIndex = i;
                            break;
                        }
                    }

                    btnGuardarProyecto.Visibility = Visibility.Visible;
                    lblMensaje.Content = "Modo edición activo - realiza cambios y presiona Guardar";
                    lblMensaje.Foreground = Brushes.Orange;
                }
            }
        }

        private void BtnGuardarProyecto_Click(object sender, RoutedEventArgs e)
        {
            if (proyectoEnEdicion == null)
            {
                MessageBox.Show("No hay ningún proyecto en edición.", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }


            if (string.IsNullOrWhiteSpace(txtNombreProyecto.Text) ||
                string.IsNullOrWhiteSpace(txtDescripcionProyecto.Text) ||
                dpFechaInicio.SelectedDate == null || dpFechaFin.SelectedDate == null)
            {
                lblMensaje.Content = "Complete todos los campos antes de guardar.";
                lblMensaje.Foreground = Brushes.Red;
                return;
            }


            proyectoEnEdicion.Nombre = txtNombreProyecto.Text.Trim();
            proyectoEnEdicion.Descripcion = txtDescripcionProyecto.Text.Trim();
            proyectoEnEdicion.FechaInicio = dpFechaInicio.SelectedDate.Value;
            proyectoEnEdicion.FechaFin = dpFechaFin.SelectedDate.Value;

            if (cmbEstado.SelectedItem is ComboBoxItem estadoSeleccionado)
                proyectoEnEdicion.IdEstado = Convert.ToInt32(estadoSeleccionado.Tag);

            try
            {
                string resultado = proyectoController.ActualizarProyecto(proyectoEnEdicion);

                if (resultado == "OK")
                {
                    lblMensaje.Content = "Proyecto actualizado correctamente";
                    lblMensaje.Foreground = Brushes.Green;
                    LimpiarFormulario();
                    proyectoEnEdicion = null;
                    btnGuardarProyecto.Visibility = Visibility.Collapsed;
                    CargarProyectos();
                }
                else
                {
                    lblMensaje.Content = resultado;
                    lblMensaje.Foreground = Brushes.Red;
                }
            }
            catch (Exception ex)
            {
                lblMensaje.Content = "Error al actualizar el proyecto: " + ex.Message;
                lblMensaje.Foreground = Brushes.Red;
            }
        }


        private void BtnEliminar_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.Tag is int id)
            {
                var proyecto = proyectos.FirstOrDefault(p => p.IdProyecto == id);
                if (proyecto == null)
                {
                    MessageBox.Show("Proyecto no encontrado.");
                    return;
                }

                var confirm = MessageBox.Show(
                    $"¿Está seguro que desea marcar como 'Cancelado' el proyecto '{proyecto.Nombre}'?",
                    "Confirmar cancelación",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Warning);

                if (confirm == MessageBoxResult.Yes)
                {
                    try
                    {
                        proyecto.IdEstado = 4;
                        string resultado = proyectoController.ActualizarProyecto(proyecto);

                        if (resultado == "OK")
                        {
                            MessageBox.Show("Proyecto cancelado correctamente.");
                            CargarProyectos();
                        }
                        else
                        {
                            MessageBox.Show("Error al cancelar el proyecto: " + resultado);
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error al cancelar el proyecto: " + ex.Message);
                    }
                }
            }
        }



        private void BtnCerrarSesion_Click(object sender, RoutedEventArgs e)
        {
            var result = MessageBox.Show("¿Está seguro que desea cerrar sesión?",
                                        "Confirmar",
                                        MessageBoxButton.YesNo,
                                        MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                var loginWindow = new LoginWindow();
                loginWindow.Show();
                this.Close();
            }
        }
    }
}
