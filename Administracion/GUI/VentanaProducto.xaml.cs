using Administracion.DP;
using Administracion.MD;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

namespace Administracion.GUI
{
    /// <summary>
    /// Lógica de interacción para VentanaProducto.xaml
    /// </summary>
    public partial class VentanaProducto : UserControl
    {
        private MainWindow _main;

        private ProductoDP productoDP;
        public VentanaProducto(MainWindow main)
        {
            InitializeComponent();
            _main = main;
            CargarProducto();
            productoDP = new ProductoDP();
        }

        public void CargarProducto()
        {
            try
                {
                ProductoDP dp = new ProductoDP();
                List<ProductoDP> productos = new(
                    dp.ConsultarAllDP()
                );

                prdDatGri.ItemsSource = productos;
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "ERROR COMPLETO:\n\n" + ex.ToString(),
                    "Error",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error
                );
            }
        }

        private void prdBtnIngresar_Click(object sender, RoutedEventArgs e)
        {
            _main.CambiarVista(new FormularioProductoIngresar(_main));
        }
    }
}
