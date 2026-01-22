using Administracion.Datos;
using Administracion.DP;
using Administracion.MD;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace Administracion.GUI
{
    public partial class VentanaPedido : UserControl
    {
        // Variables de estado para el formulario integrado
        private PedidoDP Resultado { get; set; }
        private bool esModificacion = false;

        public VentanaPedido()
        {
            InitializeComponent();
            CargarDatosIniciales();
            // Si tuvieras un ComboBox de Clientes, se cargaría aquí similar a CargarUnidades()
        }

        #region Lógica de Carga y Consulta
        private void CargarDatosIniciales()
        {
            try
            {
                PedidoDP mensajeroDP = new PedidoDP();
                pddDatGri.ItemsSource = mensajeroDP.ConsultarAllDP();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"{OracleDB.GetConfig("error.general")} {ex.Message}");
            }
        }

        private void pddBtnConsultar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                PedidoDP mensajeroDP = new PedidoDP();
                string codigoABuscar = pddTxtblBuscarCodigo.Text.Trim();
                List<PedidoDP> resultado = string.IsNullOrEmpty(codigoABuscar)
                    ? mensajeroDP.ConsultarAllDP()
                    : mensajeroDP.ConsultarByCodDP(codigoABuscar);

                pddDatGri.ItemsSource = resultado;
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }
        #endregion

        #region Control del Formulario Integrado (Visibility)
        private void pddBtnIngresar_Click(object sender, RoutedEventArgs e)
        {
            esModificacion = false;
            LimpiarFormulario();
            TxtPddCodigo.IsEnabled = true;
            PanelFormularioPdd.Visibility = Visibility.Visible;
        }

        private void pddBtnModificar_Click(object sender, RoutedEventArgs e)
        {
            PedidoDP seleccionado = pddDatGri.SelectedItem as PedidoDP;

            if (seleccionado == null)
            {
                MessageBox.Show(OracleDB.GetConfig("error.validacion"));
                return;
            }

            esModificacion = true;
            Resultado = seleccionado;

            // Llenar campos con datos existentes
            TxtPddCodigo.Text = seleccionado.PddCodigo;
            TxtPddCodigo.IsEnabled = false;
            TxtCliCedula.Text = seleccionado.CliCedula;
            TxtPddComentario.Text = seleccionado.PddComentario;
            TxtPddEstado.Text = seleccionado.PddEstado;
            TxtPddUbicacion.Text = seleccionado.PddUbicacion;
            TxtPddMontoTotal.Text = seleccionado.PddMontoTotal.ToString();
            TxtPddAbono.Text = seleccionado.PddAbono.ToString();

            PanelFormularioPdd.Visibility = Visibility.Visible;
        }

        private void BtnCancelar_Click(object sender, RoutedEventArgs e)
        {
            PanelFormularioPdd.Visibility = Visibility.Collapsed;
            LimpiarFormulario();
        }

        private void LimpiarFormulario()
        {
            TxtPddCodigo.Text = string.Empty;
            TxtCliCedula.Text = string.Empty;
            TxtPddComentario.Text = string.Empty;
            TxtPddEstado.Text = string.Empty;
            TxtPddUbicacion.Text = string.Empty;
            TxtPddMontoTotal.Text = string.Empty;
            TxtPddAbono.Text = string.Empty;
            Resultado = null;
        }
        #endregion

        #region Lógica de Guardado (Insertar/Actualizar)
        private void BtnGuardar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // 1. Validación de campos (Código y Cédula obligatorios)
                if (string.IsNullOrWhiteSpace(TxtPddCodigo.Text) ||
                    string.IsNullOrWhiteSpace(TxtCliCedula.Text))
                {
                    MessageBox.Show(OracleDB.GetConfig("error.validacion"), "Aviso", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                // 2. Preparar el objeto DP
                PedidoDP pedido = new PedidoDP
                {
                    PddCodigo = TxtPddCodigo.Text.Trim(),
                    CliCedula = TxtCliCedula.Text.Trim(),
                    PddComentario = TxtPddComentario.Text.Trim(),
                    PddEstado = TxtPddEstado.Text.Trim(),
                    PddUbicacion = TxtPddUbicacion.Text.Trim(),
                    PddMontoTotal = double.TryParse(TxtPddMontoTotal.Text, out double total) ? total : 0,
                    PddAbono = double.TryParse(TxtPddAbono.Text, out double abono) ? abono : 0
                };

                // 3. Ejecutar acción en DB
                int filasAfectadas = esModificacion ? pedido.ActualizarDP() : pedido.InsertarDP();

                if (filasAfectadas > 0)
                {
                    MessageBox.Show(esModificacion ? OracleDB.GetConfig("exito.actualizar") : OracleDB.GetConfig("exito.guardar"));
                    PanelFormularioPdd.Visibility = Visibility.Collapsed;
                    CargarDatosIniciales(); // Refrescar la tabla
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"{OracleDB.GetConfig("error.general")} {ex.Message}");
            }
        }
        #endregion

        private void pddBtnEliminar_Click(object sender, RoutedEventArgs e)
        {
            // 1. Obtener el elemento seleccionado
            PedidoDP sel = pddDatGri.SelectedItem as PedidoDP;

            // 2. Validar que no sea nulo antes de proceder
            if (sel == null)
            {
                MessageBox.Show("Por favor, seleccione una nota de venta de la lista.", "Aviso", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            // 3. Confirmación del usuario
            if (MessageBox.Show(OracleDB.GetConfig("mensaje.confirmacion.borrar"), "Confirmar", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                try
                {
                    // 4. Intento de eliminación en la base de datos
                    if (sel.EliminarDP() > 0)
                    {
                        // Mensaje de éxito desde el archivo de configuración
                        MessageBox.Show(OracleDB.GetConfig("exito.eliminar"), "Éxito", MessageBoxButton.OK, MessageBoxImage.Information);

                        // Actualizar la tabla
                        CargarDatosIniciales();
                    }
                }
                catch (Exception ex)
                {
                    // 5. Captura el error (evita que el programa se cierre) y muestra el motivo real
                    // Por ejemplo: "No se puede eliminar porque tiene detalles asociados"
                    MessageBox.Show($"No se pudo eliminar el registro.\n\nDetalle: {ex.Message}",
                                    "Error de Base de Datos",
                                    MessageBoxButton.OK,
                                    MessageBoxImage.Error);
                }
            }
        }
    }
}