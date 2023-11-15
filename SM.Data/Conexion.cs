using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;


namespace SM.Data

{
    public static class Conexion
    {

        public static string ConnectionString = ConfigurationManager.ConnectionStrings["FacturacionConnectionString"].ToString();
    }
}
