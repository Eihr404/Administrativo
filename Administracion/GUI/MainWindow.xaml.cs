using System.Text;
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
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        /* Función encargada de manejar el comportamiento por click, de los elementos 
        presentes en el menu de inicio */

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            if (sender is not MenuItem item || item.Tag == null)
                return;

            switch (item.Tag.ToString())
            {
                // En caso de dar click a opción proveedores, muestra la ventana proveedores
                case "Proveedores":
                    new Proveedor().Show();
                    break;
            }
        }
    }
}