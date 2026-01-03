using Administracion.DP;
using Administracion.MD;
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

namespace Administracion.GUI
{
    /// <summary>
    /// Lógica de interacción para ProductoForm.xaml
    /// </summary>
    public partial class ProductoForm : Window
    {
        public ProductoDP productoDP { get; set; }
        private bool esModificacion = false;
        public ProductoForm()
        {
            InitializeComponent();
            productoDP = new ProductoDP();
            CargarCombos();
            this.Title = "Nuevo Registro de Producto";
        }
        /* Carga los datos de los combobox para categoria, clasificación y unidad de medida */
        private void CargarCombos()
        {
            /* Categoría */
            CategoriaDP categoriaDP = new CategoriaDP();
            List<CategoriaDP> categorias = categoriaDP.ConsultarTodos();

            prdComBCategoria.ItemsSource = categorias;
            prdComBCategoria.DisplayMemberPath = "Descripcion";
            prdComBCategoria.SelectedValuePath = "Codigo";
            prdComBCategoria.SelectedIndex = -1;

            /* Clasificación */
            ClasificacionDP clasificacionDP = new ClasificacionDP();
            List<ClasificacionDP> clasificaciones = clasificacionDP.ConsultarTodos();

            prdComBClasificacion.ItemsSource = clasificaciones;
            prdComBClasificacion.DisplayMemberPath = "Nombre";
            prdComBClasificacion.SelectedValuePath = "Codigo";
            prdComBClasificacion.SelectedIndex = -1;

            /* Unidad de medida */
            UnidadMedidaDP unidadDP = new UnidadMedidaDP();
            List<UnidadMedidaDP> unidades = unidadDP.ConsultarTodos();

            prdComBUnidadM.ItemsSource = unidades;
            prdComBUnidadM.DisplayMemberPath = "UmeDescripcion";
            prdComBUnidadM.SelectedValuePath = "UmeCodigo";
            prdComBUnidadM.SelectedIndex = -1;
        }
        public ProductoForm(ProductoDP datosExistentes) : this()
        {
            esModificacion = true;
            this.Title = "Modificar Producto";

            prdTxtBCodigo.Text = datosExistentes.Codigo;
            prdTxtBCodigo.IsEnabled = false;

            prdComBCategoria.SelectedValue = datosExistentes.CategoriaCodigo;
            prdComBClasificacion.SelectedValue = datosExistentes.ClasificacionCodigo;
            prdComBUnidadM.SelectedValue = datosExistentes.UnidadMedidaCodigo;

            prdTxtBNombre.Text = datosExistentes.Nombre;
            prdTxtBDescripcion.Text = datosExistentes.Descripcion;
            prdTxtBPrecioVent.Text = datosExistentes.PrecioVenta.ToString();
            prdTxtBUtilidad.Text = datosExistentes.Utilidad.ToString();
            prdTxtBImagen.Text = datosExistentes.Imagen;
            prdTxtBAltImagen.Text = datosExistentes.AltTextImagen;

            productoDP = datosExistentes;
        }

        private void prdBtnGuardar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (CamposInvalidos())
                {
                    MessageBox.Show("Todos los campos son obligatorios.");
                    return;
                }

                double precioVenta = double.Parse(prdTxtBPrecioVent.Text);
                double utilidad = double.Parse(prdTxtBUtilidad.Text);

                double precioAnterior = esModificacion ? productoDP.PrecioVenta : 0;

                productoDP = new ProductoDP
                {
                    Codigo = prdTxtBCodigo.Text.Trim(),
                    CategoriaCodigo = prdComBCategoria.SelectedValue.ToString(),
                    ClasificacionCodigo = prdComBClasificacion.SelectedValue.ToString(),
                    UnidadMedidaCodigo = prdComBUnidadM.SelectedValue.ToString(),
                    Nombre = prdTxtBNombre.Text.Trim(),
                    Descripcion = prdTxtBDescripcion.Text.Trim(),
                    PrecioVentaAnt = precioAnterior,
                    PrecioVenta = precioVenta,
                    Utilidad = utilidad,
                    Imagen = prdTxtBImagen.Text.Trim(),
                    AltTextImagen = prdTxtBAltImagen.Text.Trim()
                };

                this.DialogResult = true;
            }
            catch (FormatException)
            {
                MessageBox.Show("Precio y utilidad deben ser valores numéricos válidos.");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }
        private void prdBtnCancelar_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        private bool CamposInvalidos()
        {
            return string.IsNullOrWhiteSpace(prdTxtBCodigo.Text)
                || string.IsNullOrWhiteSpace(prdComBCategoria.Text)
                || string.IsNullOrWhiteSpace(prdComBClasificacion.Text)
                || string.IsNullOrWhiteSpace(prdComBUnidadM.Text)
                || string.IsNullOrWhiteSpace(prdTxtBNombre.Text)
                || string.IsNullOrWhiteSpace(prdTxtBDescripcion.Text)
                || string.IsNullOrWhiteSpace(prdTxtBPrecioVent.Text)
                || string.IsNullOrWhiteSpace(prdTxtBUtilidad.Text)
                || string.IsNullOrWhiteSpace(prdTxtBImagen.Text)
                || string.IsNullOrWhiteSpace(prdTxtBAltImagen.Text);
        }
    }
}
