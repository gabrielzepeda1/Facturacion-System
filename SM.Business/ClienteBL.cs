using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SM.Data;
using SM.Entity;

namespace SM.Business
{
    public class ClienteBL
    {
        ClienteDL clienteDL = new ClienteDL();
        public List<Cliente> Lista()
        {
            try
            {
                return clienteDL.Lista();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Cliente GetCliente(int CodigoCliente, int CodigoPais, int CodigoEmpresa)
        {
            try
            {
                return clienteDL.GetCliente(CodigoCliente, CodigoPais, CodigoEmpresa);

            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool Create(Cliente cliente)
        {
            try
            {
                if (cliente.Nombres == null || cliente.Nombres == "")
                {
                    throw new Exception("El nombre del cliente es requerido");
                }
                return clienteDL.Create(cliente);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool Edit(Cliente cliente)
        {
            try
            {

                if (cliente.CodigoCliente == 0)
                {
                    throw new Exception("Cliente no encontrado");

                }

                return clienteDL.Edit(cliente);
            }

            catch (Exception)
            {
                throw;
            }

        }

        public bool Delete(int CodigoCliente, int CodigoPais, int CodigoEmpresa)
        {
            try
            {

                var found = clienteDL.GetCliente(CodigoCliente, CodigoPais, CodigoEmpresa);

                if (found.CodigoCliente == 0)
                {
                    throw new Exception("Cliente no encontrado");

                }

                return clienteDL.Delete(CodigoCliente, CodigoPais, CodigoEmpresa);


            }
            catch (Exception)
            {
                throw;
            }

        }

        public bool ClienteExists(int CodigoCliente, int CodigoPais, int CodigoEmpresa)
        {
            // Implement database query to check if the clientID exists in the database.
            // Return true if it exists, false otherwise.
            // You should use parameterized queries to prevent SQL injection.
            // Here is a simplified example:

            using (SqlConnection connection = new SqlConnection(Conexion.ConnectionString))
            {
                connection.Open();

                string query = "SELECT COUNT(*) FROM Clientes WHERE CodigoCliente = @CodigoCliente AND CodigoPais = @CodigoPais AND CodigoEmpresa = @CodigoEmpresa";
                SqlCommand cmd = new SqlCommand(query, connection);
                cmd.Parameters.AddWithValue("@CodigoCliente", CodigoCliente);
                cmd.Parameters.AddWithValue("@CodigoPais", CodigoPais);
                cmd.Parameters.AddWithValue("@CodigoEmpresa", CodigoEmpresa);

                int count = Convert.ToInt32(cmd.ExecuteScalar());

                //if (count == 0)
                //    return false; // Si el codigo del cliente NO existe en la base de datos, devuelve FALSE
                //else if (count > 1)
                //    return true; // Si el codigo de cliente existe en la base de datos, devuelve TRUE

                return (count > 0) ? true : false;
            }

        }
    }

}