using Administracion.Datos;
using Administracion.DP;
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
    /// <summary>
    /// Lógica de interacción para Pedido.xaml
    /// </summary>
    public partial class Pedido : UserControl


    {
        private PedidoDP pedidoActual;
        private bool esModificacion = false;
        public Pedido()
        {
            InitializeComponent();
            CargarPedidos();
        }

        private void CargarPedidos()
        {
            try
            {
                GridPedido.ItemsSource = new PedidoDP().ConsultarAllDP();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"{OracleDB.GetConfig("error.general")} {ex.Message}");
            }
        }

        private void BtnConsultar_Click(object sender, RoutedEventArgs e)
        {

            try
            {
                string criterio = TxtBuscar.Text.Trim();
                if (string.IsNullOrEmpty(criterio))
                {
                    CargarPedidos();
                }
                else
                {
                    var result = new ProductoDP { Codigo = criterio }.ConsultarByCodDP();
                    if (result == null || result.Count == 0)
                    {
                        MessageBox.Show(OracleDB.GetConfig("error.no_encontrado"));
                    }
                    GridPedido.ItemsSource = result;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"{OracleDB.GetConfig("error.general")} {ex.Message}");
            }
        }

        private void CliBtnIngresar(object sender, RoutedEventArgs e)
        {     
            LimpiarCampos();
            PanelNuevoPedido.Visibility = Visibility.Visible;
        }

        private void BtnEliminar_Click(object sender, RoutedEventArgs e)
        {
            if (GridPedido.SelectedItem is not PedidoDP seleccionado) return;

            if (MessageBox.Show(OracleDB.GetConfig("mensaje.confirmacion.borrar"),
                OracleDB.GetConfig("titulo.confirmacion"), MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                if (seleccionado.EliminarDP())
                {
                    MessageBox.Show(OracleDB.GetConfig("exito.eliminar"));
                    CargarPedidos();
                }
            }
        }

        private void BtnModificar_Click(object sender, RoutedEventArgs e)
        {
            // 1. Validar que el ítem seleccionado en el Grid sea de tipo PedidoDP
            // Asumo que tu DataGrid se llama 'GridPedidos'
            if (GridPedido.SelectedItem is not PedidoDP seleccionado)
            {
                MessageBox.Show(OracleDB.GetConfig("error.no_encontrado") ?? "Seleccione un pedido.");
                return;
            }

            // 2. Configurar banderas y referencias
            esModificacion = true;
            this.pedidoActual = seleccionado; // Asegúrate de tener esta variable global: private PedidoDP pedidoActual;
            lblTituloForm.Text = OracleDB.GetConfig("titulo.formulario.editar") ?? "Editar Pedido";

            // 3. Mapeo de atributos al formulario (GUI -> Objeto)

            // Identificador (Bloqueado)
            TxtPedCodigo.Text = seleccionado.PedCodigo;
            TxtPedCodigo.IsEnabled = false;

            // Datos del Cliente (FK)
            TxtPedCedula.Text = seleccionado.CliCedula;
            // Si usas un ComboBox para clientes, sería: CmbCliente.SelectedValue = seleccionado.CliCedula;

            // Datos de Texto
            TxtPedComentario.Text = seleccionado.PedComentario;
            TxtPedUbi.Text = seleccionado.PedUbicacion;

            // Datos Numéricos (Con formato de 2 decimales)
            TxtPedTotal.Text = seleccionado.PedTotal.ToString("N2");
            TxtPedMonto.Text = seleccionado.PedAbono.ToString("N2");

            // Estado (Asumiendo que tienes un ComboBox para 'P'endiente, 'E'ntregado, etc.)
            // Si es un TextBox simple, usa .Text
            if (TxtPedEstado != null)
            {
                TxtPedEstado.SelectedValue = seleccionado.PedEstado;
            }

            // 4. Mostrar el panel
            PanelNuevoPedido.Visibility = Visibility.Visible;
        }

        private void BtnGuardar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // 1. Validación de campos vacíos (debes tener este método implementado)
                if (CamposInvalidos())
                {
                    MessageBox.Show(OracleDB.GetConfig("error.validacion") ?? "Complete todos los campos.");
                    return;
                }

                // 2. Validación y conversión de valores numéricos
                if (!double.TryParse(TxtPedTotal.Text, out double total) ||
                    !double.TryParse(TxtPedMonto.Text, out double abono))
                {
                    MessageBox.Show(OracleDB.GetConfig("error.formato.numerico") ?? "Monto o Abono no son válidos.");
                    return;
                }

                // 3. Confirmación del usuario
                if (MessageBox.Show(OracleDB.GetConfig("mensaje.confirmacion.guardar") ?? "¿Desea guardar los cambios?",
                    OracleDB.GetConfig("titulo.confirmacion") ?? "Confirmar",
                    MessageBoxButton.YesNo) == MessageBoxResult.No) return;

                // 4. Creación del objeto PedidoDP con tus propiedades específicas
                PedidoDP datos = new PedidoDP
                {
                    PedCodigo = TxtPedCodigo.Text.Trim(),
                    CliCedula = TxtPedCedula.Text.Trim(),
                    PedComentario = TxtPedComentario.Text.Trim(),
                    PedUbicacion = TxtPedUbi.Text.Trim(),
                    PedTotal = total,
                    PedAbono = abono,
                    // Si usas un ComboBox para el estado:
                    PedEstado = TxtPedEstado.SelectedValue?.ToString() ?? "P"
                };

                // 5. Ejecución de la acción (Modificar o Ingresar)
                // Nota: Estos métodos deben estar definidos en tu clase PedidoDP
                bool resultado = esModificacion ? datos.ModificarDP() : datos.IngresarDP();

                if (resultado)
                {
                    MessageBox.Show(esModificacion
                        ? (OracleDB.GetConfig("exito.actualizar") ?? "Pedido actualizado.")
                        : (OracleDB.GetConfig("exito.guardar") ?? "Pedido guardado con éxito."));

                    PanelNuevoPedido.Visibility = Visibility.Collapsed;
                    CargarPedidos(); // Refresca el DataGrid
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"{(OracleDB.GetConfig("error.general") ?? "Error:")} {ex.Message}");
            }
        }

        private void BtnCancelar_Click(object sender, RoutedEventArgs e) => PanelNuevoPedido.Visibility = Visibility.Collapsed;



        private void LimpiarCampos()
        {
            TxtPedCodigo.Clear();
            TxtPedCedula.Clear();
            
            TxtPedComentario.Clear();
            TxtPedMonto.Clear();
            TxtPedTotal.Clear();
            TxtPedUbi.Clear();
            TxtBuscar.Clear();

        }
        private bool CamposInvalidos()
        {
            return string.IsNullOrWhiteSpace(TxtPedCedula.Text) ||
                   string.IsNullOrWhiteSpace(TxtPedCodigo.Text);
        }
    }
}
