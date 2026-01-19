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
using Administracion.DP;
using Administracion.Datos;
using System.ComponentModel;

namespace Administracion.GUI
{
    /// <summary>
    /// Lógica de interacción para Proveedor.xaml
    /// </summary>
    public partial class Proveedor : UserControl
    {
        public Proveedor()
        {
            InitializeComponent();
            PrvGuiCargarDatosIniciales();
        }

        private void PrvGuiCargarDatosIniciales()
        {
            try
            {
                ProveedorDP proveedoresDP = new ProveedorDP();
                GridProveedor.ItemsSource = proveedoresDP.ObtenerProveedoresDP();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"{OracleDB.GetConfig("error.general")} {ex.Message}");
            }
        }


        private ProveedorDP? ProveedorSeleccionado()
        {
            return GridProveedor.SelectedItem as ProveedorDP;

        }

        private void BtnConsultar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string codigoPrv = TxtBuscar.Text?.Trim() ?? "";
                GridProveedor.ItemsSource = new ProveedorDP().BuscarProveedorDP(codigoPrv);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"{OracleDB.GetConfig("error.general")} {ex.Message}");
            }
        }

        private void CliBtnIngresar(object sender, RoutedEventArgs e)
        {
            PanelNuevoProveedor.Visibility = Visibility.Visible;
        }

        private void BtnEliminar_Click(object sender, RoutedEventArgs e)
        {
            ProveedorDP? proveedorSeleccionado = ProveedorSeleccionado();
            if (proveedorSeleccionado == null)
            {
                MessageBox.Show("Seleccione un proveedor a eliminar.");
                return;
            }

            if (MessageBox.Show(Properties.Resources.mensaje_confirmacion_borrar, "Confirmar", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                if (proveedorSeleccionado.EliminarProveedorDP() > 0)
                {
                    MessageBox.Show(OracleDB.GetConfig("exito.eliminar"));
                    PrvGuiCargarDatosIniciales(); // Refrescar la tabla
                }
            }            

        }

        private void BtnModificar_Click(object sender, RoutedEventArgs e)
        {
            ProveedorDP? proveedorSeleccionado = ProveedorSeleccionado();
            if (proveedorSeleccionado == null)
            {
                MessageBox.Show("Seleccione un proveedor a modificar.");
                return;
            }
            else
            {
                TxtPrvCedula.Text = proveedorSeleccionado.PrvCodigo;
                TxtPrvNombre.Text = proveedorSeleccionado.PrvNombre;
                TxtPrvDireccion.Text = proveedorSeleccionado.PrvDireccion;
                TxtPrvTelefono.Text = proveedorSeleccionado.PrvTelefono;
                PanelNuevoProveedor.Visibility = Visibility.Visible;
            }
            ;
        }

        private void BtnGuardar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if(string.IsNullOrWhiteSpace(TxtPrvCedula.Text) ||
                   string.IsNullOrWhiteSpace(TxtPrvNombre.Text) ||
                   string.IsNullOrWhiteSpace(TxtPrvDireccion.Text) ||
                   string.IsNullOrWhiteSpace(TxtPrvTelefono.Text))
                {
                    MessageBox.Show("Por favor, complete todos los campos.");
                    return;
                }
                
                ProveedorDP proveedorDP = new ProveedorDP();

                proveedorDP.InsertarProveedorDP(
                    TxtPrvCedula.Text.Trim(),
                    TxtPrvNombre.Text.Trim(),
                    TxtPrvDireccion.Text.Trim(),
                    TxtPrvTelefono.Text.Trim()
                );
            }
            catch (Exception ex)
            {
                MessageBox.Show($"{OracleDB.GetConfig("error.general")} {ex.Message}");
            }
        }

        private void BtnCancelar_Click(object sender, RoutedEventArgs e)
        {
            PanelNuevoProveedor.Visibility = Visibility.Collapsed;
            LimpiarFormulario();
        }

        private void LimpiarFormulario()
        {
            TxtPrvCedula.Text = string.Empty;
            TxtPrvNombre.Text = string.Empty;
            TxtPrvDireccion.Text = string.Empty;
            TxtPrvTelefono.Text = string.Empty;
        }
    }
}
