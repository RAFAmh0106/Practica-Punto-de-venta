using System.Windows.Forms;
using Punto.Datos;
using MySql.Data.MySqlClient;

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
            frmPrincipal principal= new frmPrincipal();
            this.Hide();
            principal.Show();
        }

        private void frmLogin_Load(object sender, System.EventArgs e)
        {

        }
    }
}
