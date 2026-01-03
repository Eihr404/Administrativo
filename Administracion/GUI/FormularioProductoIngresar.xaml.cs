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
using System.Windows.Navigation;
using System.Windows.Shapes;
using Administracion.DP;

namespace Administracion.GUI
{
    /// <summary>
    /// Lógica de interacción para FormularioProducto.xaml
    /// </summary>
    public partial class FormularioProductoIngresar : UserControl
    {
        private MainWindow _main;
        private ProductoDP controller = new ProductoDP();
        public FormularioProductoIngresar(MainWindow main)
        {
            InitializeComponent();
            _main = main;

            prdBtnGuardar.Click += PrdBtnGuardar_Click;
            prdBtnCancelar.Click += PrdBtnCancelar_Click;
        }
        private void PrdBtnGuardar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ProductoDP p = new ProductoDP
                {
                    Codigo = prdTxtBCodigo.Text.Trim(),
                    Categoria = prdComBCategoria.Text,
                    Clasificacion = prdComBClasificacion.Text,
                    UnidadMedida = prdComBUnidadM.Text,
                    Nombre = prdTxtBNombre.Text,
                    Descripcion = prdTxtBDescripcion.Text,
                    PrecioVenta = double.Parse(prdTxtBPrecioVent.Text),
                    Utilidad = double.Parse(prdTxtBUtilidad.Text),
                    Imagen = prdTxtBImagen.Text,
                    AltTextImagen = prdTxtBAltImagen.Text
                };

                p.IngresarDP();

                MessageBox.Show("Producto registrado correctamente");

                _main.CambiarVista(new VentanaProducto(_main));
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al guardar:\n" + ex.Message);
            }
        }

        private void PrdBtnCancelar_Click(object sender, RoutedEventArgs e)
        {
            _main.CambiarVista(new VentanaProducto(_main));
        }
    }
}
