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
using GestionProyectos.Controllers;
using GestionProyectos.Models;

namespace GestionProyectos.Views
{
    public partial class GestionTareasWindow : Window
    {
        private int idProyectoActual;
        private readonly TareaController tareaController = new TareaController();

        public GestionTareasWindow(int idProyecto, string nombreProyecto)
        {
            InitializeComponent();
            idProyectoActual = idProyecto;
            this.Title = $"Tareas - {nombreProyecto}";
            CargarTareas();
        }

        private void CargarTareas()
        {
            var tareas = tareaController.ObtenerTareas(idProyectoActual);
            listaTareas.ItemsSource = tareas;
        }

        private void BtnAgregarTarea_Click(object sender, RoutedEventArgs e)
        {
            var tarea = new TareaModel
            {
                IdProyecto = idProyectoActual,
                NombreTarea = txtNombreTarea.Text,
                Encargado = txtEncargado.Text,
                Completada = false
            };

            string resultado = tareaController.AgregarTarea(tarea);
            if (resultado == "OK")
            {
                txtNombreTarea.Clear();
                txtEncargado.Clear();
                CargarTareas();
            }
            else
            {
                MessageBox.Show(resultado);
            }
        }

        private void CheckBox_CheckChanged(object sender, RoutedEventArgs e)
        {
            if (sender is CheckBox chk && chk.Tag is int idTarea)
            {
                bool estado = chk.IsChecked ?? false;
                tareaController.CambiarEstadoTarea(idTarea, estado);
                
            }
        }
    }
}
