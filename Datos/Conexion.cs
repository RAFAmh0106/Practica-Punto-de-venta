using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace Punto.Datos
{
    internal class Conexion
    {
        private string cadenaConexion= "Server=localhost;"+"Database=PuntoDB;"+"Uid=root;"+ "Pwd=;"+ "Port=3306;";

        public MySqlConnection ObtenerConexion()
        {
            MySqlConnection conexion = new MySqlConnection(cadenaConexion);

            conexion.Open();
            return conexion;
        }
    }
   
}
