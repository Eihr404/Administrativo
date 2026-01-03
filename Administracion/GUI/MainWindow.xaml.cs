using Administracion.Datos;
using Administracion.MD;
using Oracle.ManagedDataAccess.Client;
using System;
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

            try
            {
                bool ok = TestConexionMD.ProbarConexion();

                if (ok)
                {
                    MessageBox.Show("Conexión exitosa con Oracle");
                }
                else
                {
                    MessageBox.Show("No se pudo conectar a Oracle");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error de conexión:\n" + ex.Message);
            }
        }

        private void BtnProbarConexion_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                using (OracleConnection conn = OracleDB.CrearConexion())
                {
                    conn.Open();
                    MessageBox.Show("Conexión exitosa a Oracle 🎉");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        /* Función encargada de manejar el comportamiento por click, de los elementos 
        presentes en el menu de inicio */

        private void Navigation_Click(object sender, RoutedEventArgs e)
        {
            if (sender is not FrameworkElement element || element.Tag == null)
                return;

            switch (element.Tag.ToString())
            {
                case "Proveedores":
                    MainContent.Content = new Proveedor();
                    break;

                case "Clientes":
                    MainContent.Content = new Cliente();
                    break;
            }
        }
    }
}