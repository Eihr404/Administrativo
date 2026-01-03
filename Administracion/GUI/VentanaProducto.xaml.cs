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
        private ProductoDP productoDP;
        public VentanaProducto()
        {
            InitializeComponent();
            productoDP = new ProductoDP();
            CargarProductos();
        }
        /* Carga todos los productos que estan dentro de la base de datos */
        public void CargarProductos()
        {
            try
            {
                prdDatGri.ItemsSource = null;
                prdDatGri.ItemsSource = productoDP.ConsultarAllDP();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar productos: " + ex.Message);
            }
        }
        /* Evento click del boton ingresar producto */
        private void prdBtnIngresar_Click(object sender, RoutedEventArgs e)
        {
            ProductoForm formulario = new ProductoForm();
            formulario.Owner = Window.GetWindow(this);

            if (formulario.ShowDialog() == true)
            {
                try
                {
                    if (formulario.productoDP.IngresarDP())
                    {
                        MessageBox.Show("Producto registrado correctamente.");
                        CargarProductos();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al guardar: " + ex.Message);
                }
            }
        }
        /* Evento click del boton consultar producto */
        private void prdBtnConsultar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string codigo = prdTxtblBuscarCodigo.Text.Trim();

                if (string.IsNullOrEmpty(codigo))
                {
                    CargarProductos();
                }
                else
                {
                    ProductoDP p = new ProductoDP { Codigo = codigo };
                    ProductoDP resultado = p.ConsultarByCodDP();

                    if (resultado != null)
                        prdDatGri.ItemsSource = new List<ProductoDP> { resultado };
                    else
                        MessageBox.Show("No se encontró el producto.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al consultar: " + ex.Message);
            }
        }
        /* Evento click del boton modificar producto */
        private void prdBtnModificar_Click(object sender, RoutedEventArgs e)
        {
            if (prdDatGri.SelectedItem is not ProductoDP seleccionado)
            {
                MessageBox.Show("Seleccione un producto.");
                return;
            }

            ProductoForm formulario = new ProductoForm(seleccionado);
            formulario.Owner = Window.GetWindow(this);

            if (formulario.ShowDialog() == true)
            {
                try
                {
                    if (formulario.productoDP.ModificarDP())
                    {
                        MessageBox.Show("Producto modificado correctamente.");
                        CargarProductos();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al modificar: " + ex.Message);
                }
            }
        }
        /* Evento click del boton eliminar producto */
        private void prdBtnEliminar_Click(object sender, RoutedEventArgs e)
        {
            if (prdDatGri.SelectedItem is not ProductoDP seleccionado)
            {
                MessageBox.Show("Seleccione un producto para eliminar.");
                return;
            }

            var resp = MessageBox.Show(
                $"¿Desea eliminar el producto {seleccionado.Nombre}?",
                "Confirmar eliminación",
                MessageBoxButton.YesNo,
                MessageBoxImage.Warning);

            if (resp == MessageBoxResult.Yes)
            {
                if (seleccionado.EliminarDP())
                {
                    MessageBox.Show("Producto eliminado.");
                    CargarProductos();
                }
            }
        }
    }
}
