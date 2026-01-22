using Administracion.Datos;
using Administracion.DP;
using Administracion.MD;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace Administracion.GUI
{
    public partial class VentanaMateriaPrima : UserControl
    {
        // Variables de estado para el formulario integrado
        private MateriaPrimaDP Resultado { get; set; }
        private bool esModificacion = false;

        public VentanaMateriaPrima()
        {
            InitializeComponent();
            CargarDatosIniciales();
            CargarUnidades();
            CargarBodegas();
        }

        #region Lógica de Carga y Consulta
        private void CargarDatosIniciales()
        {
            try
            {
                MateriaPrimaDP mensajeroDP = new MateriaPrimaDP();
                mtpDatGri.ItemsSource = mensajeroDP.ConsultarAllDP();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"{OracleDB.GetConfig("error.general")} {ex.Message}");
            }
        }

        private void CargarUnidades()
        {
            try
            {
                UnidadMedidaDP unidad = new UnidadMedidaDP();
                CmbUmeCodigo.ItemsSource = unidad.ConsultarTodos();
                CmbUmeCodigo.SelectedValuePath = "UmeCodigo"; 
                CmbUmeCodigo.DisplayMemberPath = "UmeDescripcion";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"{OracleDB.GetConfig("error.general")} {ex.Message}");
            }
        }
        private void CargarBodegas()
        {
            try
            {
                BodegaDP bodega = new BodegaDP();
                CmbBodega.ItemsSource = bodega.ConsultarTodos();
                CmbBodega.SelectedValuePath = "Codigo";
                CmbBodega.DisplayMemberPath = "Nombre";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"{OracleDB.GetConfig("error.general")} {ex.Message}");
            }
        }

        private void mtpBtnConsultar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                MateriaPrimaDP mensajeroDP = new MateriaPrimaDP();
                string codigoABuscar = mtpTxtblBuscarCodigo.Text.Trim();
                List<MateriaPrimaDP> resultado = string.IsNullOrEmpty(codigoABuscar)
                    ? mensajeroDP.ConsultarAllDP()
                    : mensajeroDP.ConsultarByCodDP(codigoABuscar);

                mtpDatGri.ItemsSource = resultado;
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }
        #endregion

        #region Control del Formulario Integrado (Visibility)
        private void mtpBtnIngresar_Click(object sender, RoutedEventArgs e)
        {
            esModificacion = false;
            LimpiarFormulario();
            TxtMtpCodigo.IsEnabled = true;
            PanelFormularioMtp.Visibility = Visibility.Visible;
        }

        private void mtpBtnModificar_Click(object sender, RoutedEventArgs e)
        {
            MateriaPrimaDP seleccionado = mtpDatGri.SelectedItem as MateriaPrimaDP;

            if (seleccionado == null)
            {
                MessageBox.Show(OracleDB.GetConfig("error.validacion"));
                return;
            }

            esModificacion = true;
            Resultado = seleccionado;

            // Llenar campos con datos existentes
            TxtMtpCodigo.Text = seleccionado.MtpCodigo;
            TxtMtpCodigo.IsEnabled = false;
            TxtMtpNombre.Text = seleccionado.MtpNombre;
            TxtMtpDesc.Text = seleccionado.MtpDescripcion;
            TxtMtpPrecio.Text = seleccionado.MtpPrecioCompra.ToString();
            CmbUmeCodigo.SelectedValue = seleccionado.UmeCodigo;
            CmbBodega.SelectedValue = seleccionado.BodCodigo;

            PanelFormularioMtp.Visibility = Visibility.Visible;
        }

        private void BtnCancelar_Click(object sender, RoutedEventArgs e)
        {
            PanelFormularioMtp.Visibility = Visibility.Collapsed;
            LimpiarFormulario();
        }

        private void LimpiarFormulario()
        {
            TxtMtpCodigo.Text = string.Empty;
            TxtMtpNombre.Text = string.Empty;
            TxtMtpDesc.Text = string.Empty;
            TxtMtpPrecio.Text = string.Empty;
            CmbUmeCodigo.SelectedIndex = -1;
            CmbBodega.SelectedIndex = -1;
            Resultado = null;
        }
        #endregion

        #region Lógica de Guardado (Insertar/Actualizar)
        private void BtnGuardar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // 1. Validación de campos
                if (string.IsNullOrWhiteSpace(TxtMtpCodigo.Text) ||
                    string.IsNullOrWhiteSpace(TxtMtpNombre.Text) ||
                    CmbUmeCodigo.SelectedValue == null)
                {
                    MessageBox.Show(OracleDB.GetConfig("error.validacion"), "Aviso", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                // 2. Preparar el objeto DP
                double precioAnterior = esModificacion ? Resultado.MtpPrecioCompra : 0;

                MateriaPrimaDP materia = new MateriaPrimaDP
                {
                    MtpCodigo = TxtMtpCodigo.Text.Trim(),
                    UmeCodigo = CmbUmeCodigo.SelectedValue.ToString(),
                    BodCodigo = CmbBodega.SelectedValue.ToString(),
                    MtpNombre = TxtMtpNombre.Text.Trim(),
                    MtpDescripcion = TxtMtpDesc.Text.Trim(),
                    MtpPrecioCompraAnt = precioAnterior,
                    MtpPrecioCompra = double.Parse(TxtMtpPrecio.Text)
                };

                // 3. Ejecutar acción en DB
                int filasAfectadas = esModificacion ? materia.ActualizarDP() : materia.InsertarDP();

                if (filasAfectadas > 0)
                {
                    MessageBox.Show(esModificacion ? OracleDB.GetConfig("exito.actualizar") : OracleDB.GetConfig("exito.guardar"));
                    PanelFormularioMtp.Visibility = Visibility.Collapsed;
                    CargarDatosIniciales(); // Refrescar la tabla
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"{OracleDB.GetConfig("error.general")} {ex.Message}");
            }
        }
        #endregion

        private void mtpBtnEliminar_Click(object sender, RoutedEventArgs e)
        {
            // 1. Obtener el elemento seleccionado
            MateriaPrimaDP sel = mtpDatGri.SelectedItem as MateriaPrimaDP;

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