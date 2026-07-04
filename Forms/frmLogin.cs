using System;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using Punto.Datos;

namespace Punto.Forms
{
    public partial class frmLogin : Form
    {
        public frmLogin()
        {
            InitializeComponent();
        }

        private void btnLogin_Click(object sender, System.EventArgs e)
        {
            // Validar que los campos no estén vacíos
            if (txtUser.Text.Trim()== "" || txtPassword.Text.Trim() == "")
            {
                MessageBox.Show("Complete todos los campos");
                return;
            }
            try
            {
                Conexion conexion = new Conexion();
                MySqlConnection cn = conexion.ObtenerConexion();
                string sql = "SELECT nombre_completo" + "FROM usuarios"+ "WHERE username=@usuario" + " AND password=@password";

                MySqlCommand cmd = new MySqlCommand(sql, cn);
                cmd.Parameters.AddWithValue("@usuario", txtUser.Text);
                cmd.Parameters.AddWithValue("@password", txtPassword.Text);

                MySqlDataReader dr = cmd.ExecuteReader();
                if (dr.Read())
                {
                    MessageBox.Show("Bienvenido" + dr["nombre_completo"]);
                    frmPrincipal principal = new frmPrincipal();
                    this.Hide();
                    principal.Show();
                }
                else
                {
                    MessageBox.Show("Usuario o contraseña incorrectos");
                }
                cn.Close();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void frmLogin_Load(object sender, System.EventArgs e)
        {

        }
    }
}
