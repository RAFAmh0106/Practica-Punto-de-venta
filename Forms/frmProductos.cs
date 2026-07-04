using System.Windows.Forms;
using MySql.Data.MySqlClient;
using Punto.Datos;
using System.Data;
using System;

namespace Punto.Forms
{
    public partial class frmProductos : Form
    {
        Conexion conexion = new Conexion();
        public frmProductos()
        {
            InitializeComponent();
            CargarProductos();
        }

        private void CargarProductos()
        {
            try
            {
                using (MySqlConnection cn = conexion.ObtenerConexion())
                {
                    string sql = @"SELECT producto_id AS ID,codigo AS Codigo,descripcion AS Nombre,precio AS Precio,stock AS Stock FROM productos";
                    MySqlDataAdapter da = new MySqlDataAdapter(sql, cn);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    dgvProductos.DataSource = dt;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Limpiar()
        {
            lblId.Text = "0";
            txtCodigo.Clear();
            txtNombre.Clear();
            txtPrecio.Clear();
            txtStock.Clear();
            cmbCategorias.SelectedIndex = -1;
            txtCodigo.Focus();
        }

        private void frmProductos_Load(object sender, System.EventArgs e)
        {
            CargarProductos();
        }

        private void btnNuevo_Click(object sender, EventArgs e)
        {
            if(txtCodigo.Text.Trim() == "" || txtNombre.Text.Trim() == "" || txtPrecio.Text.Trim() == "" || txtStock.Text.Trim() == "" || cmbCategorias.Text.Trim() == "")
            {
                MessageBox.Show("Complete todos los campos", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            decimal precio;
            int stock;

            if(!decimal.TryParse(txtPrecio.Text,out precio))
            {
                MessageBox.Show("El precio no es valido.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtPrecio.Focus();
                return;
            }

            if(!int.TryParse(txtStock.Text,out stock))
            {
                MessageBox.Show("El stock no es valido.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtStock.Focus();
                return;
            }

            try
            {
                using(MySqlConnection cn= conexion.ObtenerConexion())
                {
                    string sql= @"INSERT INTO productos(codigo,descripcion,precio,stock,categoria)VALUES(@codigo,@descripcion,@precio,@stock,@categoria)";

                    MySqlCommand cmd = new MySqlCommand(sql, cn);

                    cmd.Parameters.AddWithValue("@codigo", txtCodigo.Text.Trim());
                    cmd.Parameters.AddWithValue("@descripcion", txtNombre.Text.Trim());
                    cmd.Parameters.AddWithValue("@precio", precio);
                    cmd.Parameters.AddWithValue("@stock", stock);
                    cmd.Parameters.AddWithValue("@categoria", cmbCategorias.Text);

                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Producto registrado correctamente.","Exito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    CargarProductos();
                    Limpiar();
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show("Error al registrar el producto:\n" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void dgvProductos_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if(e.RowIndex >= 0)
            {
                DataGridViewRow fila = dgvProductos.Rows[e.RowIndex];

                lblId.Text = fila.Cells["ID"].Value.ToString();
                txtCodigo.Text = fila.Cells["Codigo"].Value.ToString();
                txtNombre.Text = fila.Cells["Nombre"].Value.ToString();
                txtPrecio.Text = fila.Cells["Precio"].Value.ToString();
                txtStock.Text = fila.Cells["Stock"].Value.ToString();
            }

        }

        private void btnEditar_Click(object sender, EventArgs e)
        {
            if (lblId.Text == "0")
            {
                MessageBox.Show("Seleccione un producto");
                return;
            }

            decimal precio;
            int stock;

            if (!decimal.TryParse(txtPrecio.Text, out precio))
            {
                MessageBox.Show("Precio no valido");
                return;
            }

            if (!int.TryParse(txtStock.Text, out stock))
            {
                MessageBox.Show("Stock no valido");
                return;
            }

            try
            {
                using (MySqlConnection cn = conexion.ObtenerConexion())
                {
                    string sql = @"UPDATE productos SET codigo=@codigo,descripcion=@descripcion,precio=@precio,stock=@stock,categoria=@categoria WHERE producto_id=@id";
                    MySqlCommand cmd = new MySqlCommand(sql, cn);
                    cmd.Parameters.AddWithValue("@codigo", txtCodigo.Text.Trim());
                    cmd.Parameters.AddWithValue("@descripcion", txtNombre.Text.Trim());
                    cmd.Parameters.AddWithValue("@precio", precio);
                    cmd.Parameters.AddWithValue("@stock", stock);
                    cmd.Parameters.AddWithValue("@id", lblId.Text);
                    cmd.Parameters.AddWithValue("@categoria", cmbCategorias.Text);
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Producto actualizado correctamente.", "Exito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    CargarProductos();
                    Limpiar();
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al actualizar el producto:\n" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            if (lblId.Text == "0")
            {
                MessageBox.Show("Seleccione un producto");
                return;
            }

            DialogResult respuesta= MessageBox.Show("¿Está seguro de eliminar el producto?", "Confirmar", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if(respuesta== DialogResult.Yes)
            {
                try
                {
                    using(MySqlConnection cn= conexion.ObtenerConexion())
                    {
                        string sql = "DELETE FROM productos WHERE producto_id=@id";
                        MySqlCommand cmd = new MySqlCommand(sql, cn);
                        cmd.Parameters.AddWithValue("@id", Convert.ToInt32(lblId.Text));
                        cmd.ExecuteNonQuery();
                        MessageBox.Show("Producto eliminado correctamente.");
                        CargarProductos();
                        Limpiar();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void txtBusqueda_TextChanged(object sender, EventArgs e)
        {
            try
            {
                using (MySqlConnection cn = conexion.ObtenerConexion())
                {
                    string sql = @"SELECT producto_id AS ID,codigo AS Codigo,descripcion AS Nombre,precio AS Precio,stock AS Stock FROM productos WHERE descripcion LIKE @buscar";
                    MySqlDataAdapter da = new MySqlDataAdapter(sql, cn);
                    da.SelectCommand.Parameters.AddWithValue("@buscar", "%" + txtBusqueda.Text.Trim() + "%");
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    dgvProductos.DataSource = dt;
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void gbBusqueda_Enter(object sender, EventArgs e)
        {

        }
    }
}
