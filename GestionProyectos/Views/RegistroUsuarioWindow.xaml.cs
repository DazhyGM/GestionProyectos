using GestionProyectos.Models;
using GestionProyectos.Services;
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

namespace GestionProyectos.Views
{
    /// <summary>
    /// Lógica de interacción para RegistroUsuarioWindow.xaml
    /// </summary>
    public partial class RegistroUsuarioWindow : Window
    {
        private readonly UsuarioService usuarioService = new UsuarioService();
        public RegistroUsuarioWindow()
        {
            InitializeComponent();
        }
        private void BtnRegistrar_Click(object sender, RoutedEventArgs e)
        {
            lblResultado.Content = "";
            lblResultado.Foreground = Brushes.Black;

            // Validaciones
            if (string.IsNullOrWhiteSpace(txtDocumento.Text) ||
                string.IsNullOrWhiteSpace(txtNombre.Text) ||
                string.IsNullOrWhiteSpace(txtApellido.Text) ||
                string.IsNullOrWhiteSpace(txtCorreo.Text) ||
                string.IsNullOrWhiteSpace(txtContrasena.Password) ||
                string.IsNullOrWhiteSpace(txtTelefono.Text))
            {
                lblResultado.Content = "Todos los campos son obligatorios.";
                lblResultado.Foreground = Brushes.Red;
                return;
            }

            if (!int.TryParse(txtDocumento.Text.Trim(), out int documento) || documento <= 0)
            {
                lblResultado.Content = "El número de documento debe ser un valor numérico válido.";
                lblResultado.Foreground = Brushes.Red;
                return;
            }

            if (!int.TryParse(txtTelefono.Text.Trim(), out int telefono) || telefono <= 0)
            {
                lblResultado.Content = "El teléfono debe ser un valor numérico válido.";
                lblResultado.Foreground = Brushes.Red;
                return;
            }

            var usuario = new UsuarioModel
            {
                NumeroDocumento = documento,
                Nombre = txtNombre.Text.Trim(),
                Apellido = txtApellido.Text.Trim(),
                Correo = txtCorreo.Text.Trim(),
                Contrasena = txtContrasena.Password,
                Telefono = telefono
            };

            try
            {
                string resultado = usuarioService.RegistrarUsuarios(usuario);

                if (resultado == "OK")
                {
                    MessageBox.Show("Usuario registrado correctamente.",
                                   "Registro Exitoso",
                                   MessageBoxButton.OK,
                                   MessageBoxImage.Information);

                    var loginWindow = new LoginWindow();
                    loginWindow.Show();
                    this.Close();
                }
                else
                {
                    lblResultado.Content = "Error: " + resultado;
                    lblResultado.Foreground = Brushes.Red;
                }
            }
            catch (Exception ex)
            {
                lblResultado.Content = $"Error inesperado: {ex.Message}";
                lblResultado.Foreground = Brushes.Red;
            }
        }

        private void BtnVolver_Click(object sender, RoutedEventArgs e)
        {
            var MainWindow = new MainWindow();
            MainWindow.Show();
            this.Close();
        }
    }
}
