using GestionProyectos.Models;
using System;
using System.Net;
using System.Net.Mail;
using System.Windows;

namespace GestionProyectos.Services
{
    public class EmailService
    {
        public void EnviarCorreoAviso(string correoDestino, ProyectoModel proyecto)
        {
            try
            {
                SmtpClient cliente = new SmtpClient("smtp.gmail.com", 587);
                cliente.EnableSsl = true;

                cliente.Credentials = new NetworkCredential(
                    "gestionproyectos87@gmail.com",
                    "mcuz ugpi bcop zfin"
                );

                MailMessage mensaje = new MailMessage();
                mensaje.From = new MailAddress("gestionproyectos87@gmail.com", "Sistema Gestión de Proyectos");
                mensaje.To.Add(correoDestino);
                mensaje.Subject = $"Proyecto por vencer: {proyecto.Nombre}";
                mensaje.Body = $@"
                Hola,

                El proyecto: **{proyecto.Nombre}**
                con fecha de vencimiento: {proyecto.FechaFin:dd/MM/yyyy}
                Vence mañana el dia de mañana.

                Por favor revisa su estado en el que se encuentra.
                Saludos, Sistema de Gestión de Proyectos VADAKE.
                                ";

                mensaje.IsBodyHtml = false;

                cliente.Send(mensaje);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error enviando correo: " + ex.Message);
            }
        }
    }
}
