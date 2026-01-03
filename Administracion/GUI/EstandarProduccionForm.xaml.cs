using System;
using System.Windows;
using Administracion.Datos;
using Administracion.DP;
using Administracion.MD;

namespace Administracion.GUI 
{
    public partial class EstandarProduccionForm : Window
    {
        public EstandarProduccionDP Resultado { get; set; }
        private bool esModificacion = false;

        public EstandarProduccionForm()
        {
            InitializeComponent();
            this.Title = OracleDB.GetConfig("titulo.formulario.nuevo");
            CargarCatalogos();
        }

        public EstandarProduccionForm(EstandarProduccionDP datos) : this()
        {
            esModificacion = true;
            this.Title = OracleDB.GetConfig("titulo.formulario.editar");

            // Ahora sí reconocerá cmbMateriaPrima y cmbProducto
            cmbMateriaPrima.SelectedValue = datos.MtpCodigo;
            cmbProducto.SelectedValue = datos.ProCodigo;

            cmbMateriaPrima.IsEnabled = false;
            cmbProducto.IsEnabled = false;

            txtDescripcion.Text = datos.EdpDescripcion;
            txtCantidad.Text = datos.EdpCantidad.ToString();
            Resultado = datos;
        }

        private void CargarCatalogos()
        {
            try
            {
                // Asegúrate de que ProductoDP tenga el método ConsultaGeneralDP()
                cmbMateriaPrima.ItemsSource = new MateriaPrimaDP().ConsultarAllDP();
                cmbProducto.ItemsSource = new ProductoDP().ConsultarAllDP();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar catálogos: " + ex.Message);
            }
        }

        private void btnGuardar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (cmbMateriaPrima.SelectedValue == null || cmbProducto.SelectedValue == null)
                {
                    MessageBox.Show(OracleDB.GetConfig("error.validacion"));
                    return;
                }

                Resultado = new EstandarProduccionDP
                {
                    MtpCodigo = cmbMateriaPrima.SelectedValue.ToString(),
                    ProCodigo = cmbProducto.SelectedValue.ToString(),
                    EdpDescripcion = txtDescripcion.Text,
                    EdpCantidad = double.Parse(txtCantidad.Text)
                };
                this.DialogResult = true;
            }
            catch (Exception)
            {
                MessageBox.Show(OracleDB.GetConfig("error.formato.numerico"));
            }
        }

        private void btnCancelar_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }
    }
}