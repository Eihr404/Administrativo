using Administracion.Datos;
using Administracion.DP;
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

namespace Administracion.GUI
{
    /// <summary>
    /// Lógica de interacción para Window1.xaml
    /// </summary>
    public partial class MateriaPrimaForm : Window
    {
        public MateriaPrimaDP Resultado { get; set; }
        private bool esModificacion = false;

        public MateriaPrimaForm()
        {
            InitializeComponent();
            this.Title = OracleDB.GetConfig("titulo.formulario.nuevo");
            CargarUnidades();
        }
        /* Constructor para modificación */
        public MateriaPrimaForm(MateriaPrimaDP datosExistentes) : this()
        {
            esModificacion = true;
            this.Title = OracleDB.GetConfig("titulo.formulario.editar");

            txtCodigo.Text = datosExistentes.MtpCodigo;
            txtCodigo.IsEnabled = false;
            txtNombre.Text = datosExistentes.MtpNombre;
            txtDescripcion.Text = datosExistentes.MtpDescripcion;
            txtPrecio.Text = datosExistentes.MtpPrecioCompra.ToString();
            cmbUnidadMedida.SelectedValue = datosExistentes.UmeCodigo;

            Resultado = datosExistentes;
        }
        /* Carga las unidades de medida en el ComboBox */
        private void CargarUnidades()
        {
            try
            {
                UnidadMedidaDP unidad = new UnidadMedidaDP();
                cmbUnidadMedida.ItemsSource = unidad.ConsultarTodos();
            }
            catch (Exception ex)
            {
                // Uso de error.general
                MessageBox.Show($"{OracleDB.GetConfig("error.general")} {ex.Message}");
            }
        }
        /* Evento para el botón Guardar */
        private void btnGuardar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                /* Validación usando error.validacion del .properties */
                if (string.IsNullOrWhiteSpace(txtCodigo.Text) ||
                    string.IsNullOrWhiteSpace(txtNombre.Text) ||
                    cmbUnidadMedida.SelectedValue == null)
                {
                    MessageBox.Show(OracleDB.GetConfig("error.validacion"),
                                    OracleDB.GetConfig("titulo.confirmacion"),
                                    MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                double precioAnterior = (esModificacion && Resultado != null)
                                        ? Resultado.MtpPrecioCompra
                                        : 0;

                Resultado = new MateriaPrimaDP
                {
                    MtpCodigo = txtCodigo.Text.Trim(),
                    UmeCodigo = cmbUnidadMedida.SelectedValue.ToString(),
                    MtpNombre = txtNombre.Text.Trim(),
                    MtpDescripcion = txtDescripcion.Text.Trim(),
                    MtpPrecioCompraAnt = precioAnterior,
                    MtpPrecioCompra = double.Parse(txtPrecio.Text)
                };

                this.DialogResult = true;
            }
            catch (FormatException)
            {
                MessageBox.Show(OracleDB.GetConfig("error.formato.numerico"),
                        "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"{OracleDB.GetConfig("error.general")} {ex.Message}");
            }
        }
        /* Evento para el botón Cancelar */
        private void btnCancelar_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }
    }
}
