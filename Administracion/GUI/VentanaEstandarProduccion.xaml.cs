using Administracion.Datos;
using Administracion.DP;
using Administracion.MD;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace Administracion.GUI
{
    public partial class VentanaEstandarProduccion : UserControl
    {
        private EstandarProduccionDP seleccionadoDP;
        private bool esModificacion = false;

        public VentanaEstandarProduccion()
        {
            InitializeComponent();
            CargarDatosIniciales();
            CargarCatalogos();
        }

        private void CargarDatosIniciales()
        {
            try
            {
                edpDatGri.ItemsSource = new EstandarProduccionDP().ConsultarAllDP();
            }
            catch (Exception ex) { MessageBox.Show($"{OracleDB.GetConfig("error.general")} {ex.Message}"); }
        }

        private void CargarCatalogos()
        {
            try
            {
                // Cargamos ambos combos usando los métodos que devuelven listas (ConsultarAll / ConsultaGeneral)
                cmbMateriaPrima.ItemsSource = new MateriaPrimaDP().ConsultarAllDP();
                cmbProducto.ItemsSource = new ProductoDP().ConsultarAllDP();
            }
            catch (Exception ex) { MessageBox.Show($"{OracleDB.GetConfig("error.general")} {ex.Message}"); }
        }

        private void edpBtnIngresar_Click(object sender, RoutedEventArgs e)
        {
            esModificacion = false;
            LimpiarCampos();
            cmbMateriaPrima.IsEnabled = true;
            cmbProducto.IsEnabled = true;
            PanelFormularioEdp.Visibility = Visibility.Visible;
        }

        private void edpBtnModificar_Click(object sender, RoutedEventArgs e)
        {
            seleccionadoDP = edpDatGri.SelectedItem as EstandarProduccionDP;
            if (seleccionadoDP == null)
            {
                MessageBox.Show(OracleDB.GetConfig("error.validacion"));
                return;
            }

            esModificacion = true;
            cmbMateriaPrima.SelectedValue = seleccionadoDP.MtpCodigo;
            cmbProducto.SelectedValue = seleccionadoDP.ProCodigo;
            txtEdpCantidad.Text = seleccionadoDP.EdpCantidad.ToString();

            // Usualmente las llaves primarias compuestas no se editan
            cmbMateriaPrima.IsEnabled = false;
            cmbProducto.IsEnabled = false;

            PanelFormularioEdp.Visibility = Visibility.Visible;
        }

        private void BtnGuardar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                EstandarProduccionDP objeto = new EstandarProduccionDP
                {
                    MtpCodigo = cmbMateriaPrima.SelectedValue?.ToString(),
                    ProCodigo = cmbProducto.SelectedValue?.ToString(),
                    EdpCantidad = double.Parse(txtEdpCantidad.Text)
                };

                int filas = esModificacion ? objeto.ActualizarDP() : objeto.InsertarDP();

                if (filas > 0)
                {
                    MessageBox.Show(OracleDB.GetConfig(esModificacion ? "exito.actualizar" : "exito.guardar"));
                    PanelFormularioEdp.Visibility = Visibility.Collapsed;
                    CargarDatosIniciales();
                }
            }
            catch (Exception ex) { MessageBox.Show("Error al guardar: " + ex.Message); }
        }

        private void edpBtnConsultar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string codigo = edpTxtblBuscarCodigo.Text.Trim();
                EstandarProduccionDP mensajeroDP = new EstandarProduccionDP();
                List<EstandarProduccionDP> resultados;

                if (string.IsNullOrEmpty(codigo))
                {
                    // Si el textbox está vacío, hacemos consulta general
                    resultados = mensajeroDP.ConsultarAllDP();
                }
                else
                {
                    // Si hay texto, filtramos por coincidencias en ambas llaves
                    // Nota: Asegúrate de tener implementado ConsultarPorCriterioDP en tu capa DP/MD
                    resultados = mensajeroDP.ConsultarByCodDP(codigo);
                }

                edpDatGri.ItemsSource = resultados;

                // Opcional: Avisar si no se encontró nada
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

        private void edpBtnEliminar_Click(object sender, RoutedEventArgs e)
        {
            var item = edpDatGri.SelectedItem as EstandarProduccionDP;
            if (item != null && MessageBox.Show(OracleDB.GetConfig("mensaje.confirmacion.borrar"),
        OracleDB.GetConfig("titulo.confirmacion"), MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                if (item.EliminarDP() > 0) CargarDatosIniciales();
            }
        }

        private void BtnCancelar_Click(object sender, RoutedEventArgs e) => PanelFormularioEdp.Visibility = Visibility.Collapsed;

        private void LimpiarCampos()
        {
            cmbMateriaPrima.SelectedIndex = -1;
            cmbProducto.SelectedIndex = -1;
            txtEdpCantidad.Text = "";
        }
    }
}