using GestionProyectos.Controllers;
using GestionProyectos.Models;
using iTextSharp.text;
using iTextSharp.text.pdf;
using Microsoft.Win32;
using OfficeOpenXml;
using OxyPlot;
using OxyPlot.Annotations;
using OxyPlot.Axes;
using OxyPlot.Series;
using OxyPlot.Wpf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Element = iTextSharp.text.Element;
using PdfImage = iTextSharp.text.Image;
using PdfPageSize = iTextSharp.text.PageSize;

namespace GestionProyectos.Views
{
    public partial class Graficas : Window
    {
        private GraficasController controller = new GraficasController();
        private string usuarioActual;
        private int numeroDocumentoUsuario;

        public Graficas(string nombreUsuario, int numeroDocumento)
        {
            InitializeComponent();
            txtUsuarioNombre.Text = nombreUsuario;
            usuarioActual = nombreUsuario;
            numeroDocumentoUsuario = numeroDocumento;
            CargarBarras();
            CargarComboUsuarios();
            CargarProyectosPorUsuario();
        }
        private void CargarComboUsuarios()
        {
            try
            {
                var encargados = controller.ObtenerEncargados();
                cmbUsuarios.ItemsSource = encargados;
                cmbUsuarios.DisplayMemberPath = "Nombre";
                cmbUsuarios.SelectedValuePath = "NumeroDocumento";
                if (encargados.Count > 0)
                {
                    cmbUsuarios.SelectedIndex = 0;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar usuarios: {ex.Message}", "Error",
                              MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void cmbUsuarios_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmbUsuarios.SelectedValue != null && IsLoaded)
            {
                try
                {
                    int numeroDocumento = (int)cmbUsuarios.SelectedValue;
                    var tareas = controller.ObtenerCantidadTareasPorEncargado(numeroDocumento);
                    CargarPie(tareas);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error al cargar tareas: {ex.Message}", "Error",
                                  MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        public void CargarBarras()
        {
            List<GraficasModel> datos = controller.ObtenerProyectosEstado();

            var model = new PlotModel
            {
                Title = "Proyectos por Estado",
                TitleFontSize = 14,
                PlotAreaBorderColor = OxyColors.LightGray,
                PlotAreaBorderThickness = new OxyThickness(1)
            };

            var barSeries = new BarSeries
            {
                LabelPlacement = LabelPlacement.Inside,
                LabelFormatString = "{0}",
                FillColor = OxyColor.FromRgb(33, 150, 243)
            };

            foreach (var item in datos)
            {
                barSeries.Items.Add(new BarItem { Value = item.Cantidad });
            }

            var estados = datos.Select(d => d.NombreEstado).ToList();

            model.Axes.Add(new CategoryAxis
            {
                Position = AxisPosition.Left,
                ItemsSource = estados
            });

            model.Axes.Add(new LinearAxis
            {
                Position = AxisPosition.Bottom,
                Title = "Cantidad",
                Minimum = 0
            });

            model.Series.Add(barSeries);
            chartBarras.Model = model;
        }

        public void CargarPie(List<GraficasModel> datos)
        {
            try
            {
                var model = new PlotModel { Title = "Tareas del Usuario" };
                var pieSeries = new PieSeries
                {
                    InsideLabelPosition = 0.8,
                    StrokeThickness = 2,
                    InsideLabelFormat = "{1}",
                    TickDistance = 1.0
                };

                if (datos != null && datos.Count > 0)
                {
                    var usuario = datos[0];
                    if (usuario.Cantidad > 0)
                    {
                        pieSeries.Slices.Add(new PieSlice(
                            $"{usuario.Nombre}\n({usuario.Cantidad} tareas)",
                            usuario.Cantidad)
                        {
                            Fill = OxyColor.FromRgb(52, 152, 219)
                        });
                    }
                    else
                    {
                        pieSeries.Slices.Add(new PieSlice(
                            $"{usuario.Nombre}\n(0 tareas)",
                            1)
                        {
                            Fill = OxyColor.FromRgb(231, 76, 60)
                        });
                    }
                }
                else
                {
                    pieSeries.Slices.Add(new PieSlice("0 tareas", 1)
                    {
                        Fill = OxyColor.FromRgb(231, 76, 60)
                    });
                }

                model.Series.Add(pieSeries);
                chartPastel.Model = model;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar gráfica circular: {ex.Message}", "Error",
                              MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


        private void CargarProyectosPorUsuario()
        {
            try
            {
                var proyectosPorUsuario = controller.ObtenerProyectosPorUsuario();
                CargarLineas(proyectosPorUsuario);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar proyectos por usuario: {ex.Message}", "Error",
                              MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


        public void CargarLineas(List<GraficasModel> datos)
        {
            try
            {
                var model = new PlotModel { Title = "Proyectos Asigados" };
                var series = new LineSeries
                {
                    MarkerType = MarkerType.Circle,
                    MarkerSize = 5,
                    MarkerStroke = OxyColors.White,
                    MarkerFill = OxyColor.FromRgb(46, 204, 113),
                    Color = OxyColor.FromRgb(46, 204, 113)
                };
                int x = 1;
                foreach (var item in datos)
                {
                    series.Points.Add(new DataPoint(x++, item.Cantidad));
                }
                if (datos.Count > 0)
                {
                    var categoryAxis = new CategoryAxis
                    {
                        Position = AxisPosition.Bottom,
                        Title = "Usuarios",
                        ItemsSource = datos.Select(d => d.Nombre ?? "Usuario").ToList(),
                        Angle = 90,
                        MajorStep = 1
                    };
                    model.Axes.Add(categoryAxis);
                }
                model.Axes.Add(new LinearAxis
                {
                    Position = AxisPosition.Left,
                    Title = "Cantidad de Proyectos",
                    Minimum = 0,
                    MinorStep = 1,
                    MajorStep = 1
                });

                model.Series.Add(series);
                chartLineas.Model = model;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar gráfica de líneas: {ex.Message}", "Error",
                              MessageBoxButton.OK, MessageBoxImage.Error);
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




        private void BtnExportar_Click(object sender, RoutedEventArgs e)
        {
            var result = MessageBox.Show("¿Desea exportar el reporte en PDF?",
                                   "Exportar Reporte",
                                   MessageBoxButton.YesNoCancel);

            if (result == MessageBoxResult.Yes)
            {
                ExportarPDF();
            }
            else if (result == MessageBoxResult.No)
            {
                ExportarExcel();
            }
        }


        private void ExportarPDF()
        {
            try
            {
                var saveDialog = new SaveFileDialog
                {
                    Filter = "PDF files (*.pdf)|*.pdf",
                    FileName = $"Reporte_Graficas_{DateTime.Now:yyyyMMdd_HHmmss}.pdf"
                };

                if (saveDialog.ShowDialog() != true) return;

                var pdf = new Document(PdfPageSize.A4, 40, 40, 40, 40);
                using var stream = new FileStream(saveDialog.FileName, FileMode.Create);
                var writer = PdfWriter.GetInstance(pdf, stream);
                pdf.Open();

                var titulo = new Paragraph("REPORTE DE GRÁFICAS\nGESTIÓN DE PROYECTOS",
                    FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 20))
                { Alignment = iTextSharp.text.Element.ALIGN_CENTER };
                pdf.Add(titulo);
                pdf.Add(new Paragraph(" "));
                pdf.Add(new Paragraph(" "));
                var fecha = new Paragraph($"Generado el: {DateTime.Now:dd/MM/yyyy HH:mm}",
                    FontFactory.GetFont(FontFactory.HELVETICA, 12))
                { Alignment = iTextSharp.text.Element.ALIGN_CENTER };
                pdf.Add(fecha);
                pdf.Add(new Paragraph("\n\n"));

                var graficas = new List<(PlotView chart, int width, int height)>
        {
            (chartBarras, 550, 370),
            (chartLineas, 550, 370),
            (chartPastel, 550, 370)
        };
                for (int i = 0; i < graficas.Count; i++)
                {
                    if (i % 2 == 0 && i != 0) pdf.NewPage();

                    var (chart, width, height) = graficas[i];

                    var imgBytes = CapImage(chart);
                    if (imgBytes != null)
                    {
                        var img = PdfImage.GetInstance(imgBytes);
                        img.ScaleToFit(width, height);
                        img.Alignment = iTextSharp.text.Element.ALIGN_CENTER;
                        pdf.Add(img);
                    }

                    pdf.Add(new Paragraph("\n"));
                }

                pdf.Close();
                MessageBox.Show($"PDF generado exitosamente", "Éxito");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}", "Error");
            }
        }



        private byte[] CapImage(PlotView plotView)
        {
            try
            {
                double scale = 2.0;
                var width = (int)(plotView.ActualWidth * scale);
                var height = (int)(plotView.ActualHeight * scale);
                if (width <= 0) width = 600;
                if (height <= 0) height = 400;

                var bitmap = new RenderTargetBitmap(
                    width, height, 96 * scale, 96 * scale, PixelFormats.Pbgra32);

                bitmap.Render(plotView);

                var encoder = new PngBitmapEncoder();
                encoder.Frames.Add(BitmapFrame.Create(bitmap));

                using (var stream = new MemoryStream())
                {
                    encoder.Save(stream);
                    return stream.ToArray();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al capturar gráfica: {ex.Message}", "Error");
                return null;
            }
        }

        private void ExportarExcel()
        {
        }





    }
}