using Administracion.Datos;
using Administracion.DP; // Donde están tus Getters y Setters
using Administracion.MD; // Donde está tu conexión a Oracle
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

namespace Administracion.GUI
{
    public partial class MateriaPrima : UserControl
    {
        MateriaPrimaMD modelo = new MateriaPrimaMD();

        public MateriaPrima()
        {
            InitializeComponent();
            CargarDatosIniciales();
        }

        private void CargarDatosIniciales()
        {
            try
            {
                MateriaPrimaDP mensajeroDP = new MateriaPrimaDP();
                mtpDatGri.ItemsSource = mensajeroDP.ConsultaGeneralDP();
            }
            catch (Exception ex)
            {
                // Uso de error.general desde el properties
                MessageBox.Show($"{OracleDB.GetConfig("error.general")} {ex.Message}");
            }
        }

        private void mtpBtnConsultar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                MateriaPrimaDP mensajeroDP = new MateriaPrimaDP();
                string codigoABuscar = mtpTxtblBuscarCodigo.Text.Trim();
                List<MateriaPrimaDP> resultado;

                resultado = string.IsNullOrEmpty(codigoABuscar)
                    ? mensajeroDP.ConsultaGeneralDP()
                    : mensajeroDP.ConsultaPorParametroDP(codigoABuscar);

                mtpDatGri.ItemsSource = resultado;

                if (resultado.Count == 0 && !string.IsNullOrEmpty(codigoABuscar))
                {
                    MessageBox.Show(OracleDB.GetConfig("error.no_encontrado"),
                    OracleDB.GetConfig("titulo.confirmacion"),
                    MessageBoxButton.OK,
                    MessageBoxImage.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"{OracleDB.GetConfig("error.general")} {ex.Message}");
            }
        }

        private void mtpBtnIngresar_Click(object sender, RoutedEventArgs e)
        {
            MateriaPrimaForm formulario = new MateriaPrimaForm();
            formulario.Owner = Window.GetWindow(this);

            if (formulario.ShowDialog() == true)
            {
                try
                {
                    if (formulario.Resultado.InsertarDP() > 0)
                    {
                        // Mensaje de éxito centralizado
                        MessageBox.Show(OracleDB.GetConfig("exito.guardar"),
                                        OracleDB.GetConfig("titulo.confirmacion"),
                                        MessageBoxButton.OK, MessageBoxImage.Information);
                        mtpBtnConsultar_Click(null, null);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"{OracleDB.GetConfig("error.general")} {ex.Message}");
                }
            }
        }

        private void mtpBtnModificar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                MateriaPrimaDP seleccionado = mtpDatGri.SelectedItem as MateriaPrimaDP;

                if (seleccionado == null)
                {
                    MessageBox.Show(OracleDB.GetConfig("error.validacion"));
                    return;
                }

                MateriaPrimaForm formulario = new MateriaPrimaForm(seleccionado);
                formulario.Owner = Window.GetWindow(this);

                if (formulario.ShowDialog() == true)
                {
                    if (formulario.Resultado.ActualizarDP() > 0)
                    {
                        MessageBox.Show(OracleDB.GetConfig("exito.actualizar"));
                        mtpBtnConsultar_Click(null, null);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"{OracleDB.GetConfig("error.general")} {ex.Message}");
            }
        }

        private void mtpBtnEliminar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                MateriaPrimaDP seleccionado = mtpDatGri.SelectedItem as MateriaPrimaDP;

                if (seleccionado == null)
                {
                    MessageBox.Show(OracleDB.GetConfig("error.validacion"));
                    return;
                }

                // Confirmación usando los mensajes del archivo .properties
                var respuesta = MessageBox.Show(OracleDB.GetConfig("mensaje.confirmacion.borrar"),
                                               OracleDB.GetConfig("titulo.confirmacion"),
                                               MessageBoxButton.YesNo, MessageBoxImage.Warning);

                if (respuesta == MessageBoxResult.Yes)
                {
                    if (seleccionado.EliminarDP() > 0)
                    {
                        MessageBox.Show(OracleDB.GetConfig("exito.eliminar"));
                        mtpBtnConsultar_Click(null, null);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"{OracleDB.GetConfig("error.general")} {ex.Message}");
            }
        }
    }
}
