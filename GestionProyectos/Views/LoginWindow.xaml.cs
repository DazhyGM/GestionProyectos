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
using GestionProyectos.Services;

namespace GestionProyectos.Views
{

    public partial class LoginWindow : Window
    {
        private readonly UsuarioService usuarioService = new UsuarioService();

        public LoginWindow()
        {
            InitializeComponent();
        }

        private void BtnIniciarSesion(object sender, RoutedEventArgs e)
        {
            string correo = txtCorreo.Text.Trim();
            string contrasena = txtContrasena.Password;
            if (string.IsNullOrWhiteSpace(correo) || string.IsNullOrWhiteSpace(contrasena))
            {
                lblMensaje.Text = "Se deben completar todos los campos.";
                lblMensaje.Foreground = Brushes.Red;
                return;
            }
            try
            {

                var usuario = usuarioService.ObtenerUsuarioPorCredenciales(correo, contrasena);

                if (usuario != null)
                {

                    string nombreCompleto = $"{usuario.Nombre} {usuario.Apellido}";
                    int numeroDocumento = usuario.NumeroDocumento ?? 0;

                    lblMensaje.Text = "Inicio de sesión exitoso";
                    lblMensaje.Foreground = Brushes.Green;

 
                    var principalWindow = new PrincipalWindow(nombreCompleto, numeroDocumento);
                    principalWindow.Show();
                    this.Close();
                }
                else
                {

                    lblMensaje.Text = "Correo o contraseña incorrectos.";
                    lblMensaje.Foreground = Brushes.Red;
                }
            }
            catch (Exception ex)
            {
                lblMensaje.Text = "Inicio de sesión exitoso";
                lblMensaje.Foreground = Brushes.Blue;
            }


        }
        private void BtnRegresar_Click(object sender, RoutedEventArgs e)
        {
            var mainWindow = new MainWindow();
            mainWindow.Show();
            this.Close();
        }


        private void BtnIrRegistro_Click(object sender, RoutedEventArgs e)
        {
            var registroWindow = new RegistroUsuarioWindow();
            registroWindow.Show();
            this.Close();
        }
    }
}
