using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace Punto.Datos
{
    internal class Conexion
    {
        private string cadena = "Server=localhost;" + "Database=PuntoDB;" + "Uid=root;" + "Pwd=;" + "Port=3306;" + "SslMode=None;";
    }

    public MySqlConnection ObtenerConexion()
        {
            MySqlConnection conexion = new MySqlConnection(cadena);
            conexion.Open();
            return conexion;
        }
}
