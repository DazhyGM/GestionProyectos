using GestionProyectos.Controllers;
using GestionProyectos.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Xml.Linq;

namespace GestionProyectos.Views
{
    /// <summary>
    /// Lógica de interacción para ProyectosInactivosWindow.xaml
    /// </summary>
    public partial class ProyectosInactivosWindow : Window
    {

        private ProyectoController proyectoController;
        private int numeroDocumentoUsuario;
        private List<ProyectoModel> proyectos;
        /// cambio 
        private string usuarioActual;
        public ProyectosInactivosWindow(string nombreUsuario, int numeroDocumento)
        {
            InitializeComponent();
            usuarioActual = nombreUsuario;
            numeroDocumentoUsuario = numeroDocumento;
            proyectoController = new ProyectoController();
            CargarPInactivos();
        }



        private void CargarPInactivos()
        {
            try
            {
                proyectos = (proyectoController.ObtenerPInactivos(numeroDocumentoUsuario) ?? new List<ProyectoModel>());

                var listaParaMostrar = proyectos
                    .Where(p => p.IdEstado == 4)
                    .Select(p => new
                    {
                        Id = p.IdProyecto,
                        Nombre = p.Nombre ?? "",
                        Descripcion = p.Descripcion ?? "",
                        FechaInicio = p.FechaInicio,
                        FechaFin = p.FechaFin,
                        Estado = p.NombreEstado ?? p.IdEstado.ToString()
                    })
                    .OrderByDescending(x => x.Id).ToList();

                listaProyectosI.ItemsSource = listaParaMostrar;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error cargando proyectos: " + ex.Message);
            }
        }



        private void BtnVolver_Click(object sender, RoutedEventArgs e)
        {
            PrincipalWindow volver = new PrincipalWindow(usuarioActual, numeroDocumentoUsuario);
            volver.Show();
            this.Close();
          
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

        private void BtnActivar_Click(object sender, RoutedEventArgs e)
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
                    $"¿Está seguro que desea Actrivar el proyecto '{proyecto.Nombre}'?",
                    "Confirmar Activación",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Warning);

                if (confirm == MessageBoxResult.Yes)
                {
                    try
                    {
                        proyecto.IdEstado = 1;
                        string resultado = proyectoController.ActivarProyecto(proyecto);

                        if (resultado == "OK")
                        {
                            MessageBox.Show("Proyecto activado correctamente.");
                            CargarPInactivos();
                        }
                        else
                        {
                            MessageBox.Show("Error al activar el proyecto: " + resultado);
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error al activar  el proyecto: " + ex.Message);
                    }
                }
            }

        }
    }
}
