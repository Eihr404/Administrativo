using Administracion.Datos;
using Administracion.DP;
using Administracion.MD;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace Administracion.GUI
{
    public partial class VentanaDetallePedido : UserControl
    {
        private DetallePedidoDP seleccionadoDP;
        private bool esModificacion = false;

        public VentanaDetallePedido()
        {
            InitializeComponent();
            CargarDatosIniciales();
            CargarCatalogos();
        }

        private void CargarDatosIniciales()
        {
            try
            {
                dppDatGri.ItemsSource = new DetallePedidoDP().ConsultarAllDP();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"{OracleDB.GetConfig("error.general")} {ex.Message}");
            }
        }

        private void CargarCatalogos()
        {
            try
            {
                // Cargamos ambos combos usando los métodos de las capas DP
                cmbPedidoDpp.ItemsSource = new PedidoDP().ConsultarAllDP();
                cmbProductoDpp.ItemsSource = new ProductoDP().ConsultarAllDP();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"{OracleDB.GetConfig("error.general")} {ex.Message}");
            }
        }

        private void dppBtnIngresar_Click(object sender, RoutedEventArgs e)
        {
            esModificacion = false;
            LimpiarCampos();
            cmbPedidoDpp.IsEnabled = true;
            cmbProductoDpp.IsEnabled = true;
            PanelFormularioDpp.Visibility = Visibility.Visible;
        }

        private void dppBtnModificar_Click(object sender, RoutedEventArgs e)
        {
            seleccionadoDP = dppDatGri.SelectedItem as DetallePedidoDP;
            if (seleccionadoDP == null)
            {
                MessageBox.Show(OracleDB.GetConfig("error.validacion"));
                return;
            }

            esModificacion = true;
            cmbPedidoDpp.SelectedValue = seleccionadoDP.PddCodigo;
            cmbProductoDpp.SelectedValue = seleccionadoDP.ProCodigo;
            txtDppCantidad.Text = seleccionadoDP.DppCantidad.ToString();
            txtDppMonto.Text = seleccionadoDP.DppMonto.ToString();

            // Las llaves primarias compuestas (Pedido + Producto) no se editan
            cmbPedidoDpp.IsEnabled = false;
            cmbProductoDpp.IsEnabled = false;

            PanelFormularioDpp.Visibility = Visibility.Visible;
        }

        private void BtnGuardar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Validación básica de selección
                if (cmbPedidoDpp.SelectedValue == null || cmbProductoDpp.SelectedValue == null)
                {
                    MessageBox.Show(OracleDB.GetConfig("error.validacion"));
                    return;
                }

                DetallePedidoDP objeto = new DetallePedidoDP
                {
                    PddCodigo = cmbPedidoDpp.SelectedValue.ToString(),
                    ProCodigo = cmbProductoDpp.SelectedValue.ToString(),
                    DppCantidad = int.Parse(txtDppCantidad.Text),
                    DppMonto = double.Parse(txtDppMonto.Text)
                };

                int filas = esModificacion ? objeto.ActualizarDP() : objeto.InsertarDP();

                if (filas > 0)
                {
                    MessageBox.Show(OracleDB.GetConfig(esModificacion ? "exito.actualizar" : "exito.guardar"));
                    PanelFormularioDpp.Visibility = Visibility.Collapsed;
                    CargarDatosIniciales();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"{OracleDB.GetConfig("error.general")} {ex.Message}");
            }
        }

        private void dppBtnConsultar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string codigo = dppTxtblBuscarCodigo.Text.Trim();
                DetallePedidoDP mensajeroDP = new DetallePedidoDP();
                List<DetallePedidoDP> resultados;

                if (string.IsNullOrEmpty(codigo))
                {
                    resultados = mensajeroDP.ConsultarAllDP();
                }
                else
                {
                    resultados = mensajeroDP.ConsultarByCodDP(codigo);
                }

                dppDatGri.ItemsSource = resultados;

                if (resultados.Count == 0 && !string.IsNullOrEmpty(codigo))
                {
                    MessageBox.Show(OracleDB.GetConfig("error.no_encontrado"), "Información", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"{OracleDB.GetConfig("error.general")} {ex.Message}");
            }
        }

        private void dppBtnEliminar_Click(object sender, RoutedEventArgs e)
        {
            var item = dppDatGri.SelectedItem as DetallePedidoDP;
            if (item != null && MessageBox.Show(OracleDB.GetConfig("mensaje.confirmacion.borrar"),
                OracleDB.GetConfig("titulo.confirmacion"), MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                if (item.EliminarDP() > 0)
                {
                    MessageBox.Show(OracleDB.GetConfig("exito.eliminar"));
                    CargarDatosIniciales();
                }
            }
        }

        private void BtnCancelar_Click(object sender, RoutedEventArgs e) => PanelFormularioDpp.Visibility = Visibility.Collapsed;

        private void LimpiarCampos()
        {
            cmbPedidoDpp.SelectedIndex = -1;
            cmbProductoDpp.SelectedIndex = -1;
            txtDppCantidad.Clear();
            txtDppMonto.Clear();
        }
    }
}