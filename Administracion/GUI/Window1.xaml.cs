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
        // Propiedad donde guardaremos el objeto para que la otra ventana lo lea
        public MateriaPrimaDP Resultado { get; private set; }

        public MateriaPrimaForm()
        {
            InitializeComponent();
        }

        private void btnGuardar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Validaciones básicas
                if (string.IsNullOrWhiteSpace(txtCodigo.Text)) throw new Exception("El código es obligatorio.");

                Resultado = new MateriaPrimaDP
                {
                    MtpCodigo = txtCodigo.Text.Trim(),
                    UmeCodigo = txtUnidad.Text.Trim(),
                    MtpNombre = txtNombre.Text.Trim(),
                    MtpDescripcion = txtDescripcion.Text.Trim(),
                    MtpPrecioCompra = double.Parse(txtPrecio.Text),
                    MtpPrecioCompraAnt = 0 // Al ser nuevo, el anterior es 0
                };

                this.DialogResult = true; // Esto cierra la ventana y avisa que se guardó
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error en los datos: " + ex.Message);
            }
        }

        private void btnCancelar_Click(object sender, RoutedEventArgs e) => this.DialogResult = false;
    }
}
