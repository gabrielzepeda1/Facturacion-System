using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SM.Entity;

namespace SM.Data
{
    public class ClienteDL
    {
        public List<Cliente> Lista()
        {   
            List<Cliente> lista = new List<Cliente>();

            using (SqlConnection connection = new SqlConnection(Conexion.ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("SELECT * FROM fn_clientes()", connection);
                cmd.CommandType = CommandType.Text; 

                try
                {
                    connection.Open(); 
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            lista.Add(new Cliente
                            {
                                CodigoCliente = Convert.ToInt32(dr["CodigoCliente"]),
                                Nombres = dr["Nombres"].ToString(),
                                Apellidos = dr["Apellidos"].ToString(),
                                NumeroIdentificacion = dr["NumeroIdentificacion"].ToString(),
                                RazonSocial = dr["RazonSocial"].ToString(),
                                Direccion = dr["Direccion"].ToString(),
                                Telefono = dr["Telefono"].ToString(),
                                CorreoElectronico = dr["CorreoElectronico"].ToString(),
                                CuentaContable = dr["CuentaContable"].ToString(),
                                LimiteCredito = Convert.ToDecimal(dr["LimiteCredito"]),
                                DiasCredito = Convert.ToInt32(dr["DiasCredito"]),
                                ExcentoImpuestos = Convert.ToBoolean(dr["ExcentoImpuestos"]),
                                Activo = Convert.ToBoolean(dr["Activo"]),
                                Distribuidor = Convert.ToBoolean(dr["Distribuidor"]),
                                PersonaJuridica = Convert.ToBoolean(dr["PersonaJuridica"]),
                                Externo = Convert.ToBoolean(dr["Externo"]),
                                CodigoPais = Convert.ToInt32(dr["CodigoPais"]),
                                CodigoEmpresa = Convert.ToInt32(dr["CodigoEmpresa"]),
                                CodigoSectorMercado = Convert.ToInt32(dr["CodigoSectorMercado"]),
                                CodigoVendedor = Convert.ToInt32(dr["CodigoVendedor"]),
                                CodigoUser = Convert.ToInt32(dr["CodigoUser"]),
                                CodigoUserUlt = Convert.ToInt32(dr["CodigoUserUlt"]) 

                            });
                        }
                    }
                    return lista;
                } 
                catch (Exception)
                {
                    throw;
                }
                
            }

        }

        public Cliente GetCliente(int CodigoCliente, int CodigoPais, int CodigoEmpresa)
        {
            Cliente cliente = new Cliente();
            {
                using (SqlConnection connection = new SqlConnection(Conexion.ConnectionString))
                {
                    SqlCommand cmd = new SqlCommand("SELECT * FROM fn_cliente(@CodigoCliente, @CodigoPais, @CodigoEmpresa)", connection);
                    cmd.Parameters.AddWithValue("@CodigoCliente", CodigoCliente);
                    cmd.Parameters.AddWithValue("@CodigoPais", CodigoPais);
                    cmd.Parameters.AddWithValue("@CodigoEmpresa", CodigoEmpresa);
                    cmd.CommandType = CommandType.Text;

                    try
                    {
                        connection.Open();
                        using (SqlDataReader dr = cmd.ExecuteReader())
                        {
                            if (dr.Read())
                            {
                                cliente.CodigoCliente = CodigoCliente;
                                cliente.Nombres = dr["Nombres"].ToString();
                                cliente.Apellidos = dr["Apellidos"].ToString();
                                cliente.NumeroIdentificacion = dr["NumeroIdentificacion"].ToString();
                                cliente.RazonSocial = dr["RazonSocial"].ToString();
                                cliente.Direccion = dr["Direccion"].ToString();
                                cliente.Telefono = dr["Telefono"].ToString();
                                cliente.CorreoElectronico = dr["CorreoElectronico"].ToString();
                                cliente.CuentaContable = dr["CuentaContable"].ToString();
                                cliente.LimiteCredito = Convert.ToDecimal(dr["LimiteCredito"]);
                                cliente.DiasCredito = Convert.ToInt32(dr["DiasCredito"]);
                                cliente.ExcentoImpuestos = Convert.ToBoolean(dr["ExcentoImpuestos"]);
                                cliente.Activo = Convert.ToBoolean(dr["Activo"]);
                                cliente.Distribuidor = Convert.ToBoolean(dr["Distribuidor"]);
                                cliente.PersonaJuridica = Convert.ToBoolean(dr["PersonaJuridica"]);
                                cliente.Externo = Convert.ToBoolean(dr["Externo"]);
                                cliente.CodigoPais = CodigoPais;
                                cliente.CodigoEmpresa = CodigoEmpresa;
                                cliente.CodigoSectorMercado = Convert.ToInt32(dr["CodigoSectorMercado"]);
                                cliente.CodigoVendedor = Convert.ToInt32(dr["CodigoVendedor"]);
                                cliente.CodigoUser = Convert.ToInt32(dr["CodigoUser"]);
                                cliente.CodigoUserUlt = Convert.ToInt32(dr["CodigoUserUlt"]);   
                            }


                        }
                        return cliente;
                    }
                    catch (SqlException)
                    {
                        throw;
                    }
                    catch (Exception)
                    {
                        throw;
                    }
                }
            }
        }

        public bool Create(Cliente cliente)
        {
            bool response = false;

            using (SqlConnection connection = new SqlConnection(Conexion.ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("sp_CrearCliente", connection);
                cmd.Parameters.AddWithValue("@CodigoCliente", cliente.CodigoCliente);
                //ELIMINAR CODIGO CLIENTE UNA VEZ QUE EL CODIGO SE GENERE AUTOMATICAMENTE EN SQL SERVER
                cmd.Parameters.AddWithValue("@Nombres", cliente.Nombres);
                cmd.Parameters.AddWithValue("@Apellidos", cliente.Apellidos);
                cmd.Parameters.AddWithValue("@NumeroIdentificacion", cliente.NumeroIdentificacion);
                cmd.Parameters.AddWithValue("@RazonSocial", cliente.RazonSocial);
                cmd.Parameters.AddWithValue("@Direccion", cliente.Direccion);
                cmd.Parameters.AddWithValue("@Telefono", cliente.Telefono);
                cmd.Parameters.AddWithValue("@CorreoElectronico", cliente.CorreoElectronico);
                cmd.Parameters.AddWithValue("@CuentaContable", cliente.CuentaContable);
                cmd.Parameters.AddWithValue("@LimiteCredito", cliente.LimiteCredito);
                cmd.Parameters.AddWithValue("@DiasCredito", cliente.DiasCredito);
                cmd.Parameters.AddWithValue("@ExcentoImpuestos", cliente.ExcentoImpuestos);
                cmd.Parameters.AddWithValue("@Activo", cliente.Activo);
                cmd.Parameters.AddWithValue("@Distribuidor", cliente.Distribuidor);
                cmd.Parameters.AddWithValue("@PersonaJuridica", cliente.PersonaJuridica);
                cmd.Parameters.AddWithValue("@Externo", cliente.Externo);
                cmd.Parameters.AddWithValue("@CodigoPais", cliente.CodigoPais);
                cmd.Parameters.AddWithValue("@CodigoEmpresa", cliente.CodigoEmpresa);
                cmd.Parameters.AddWithValue("@CodigoSectorMercado", cliente.CodigoSectorMercado);
                cmd.Parameters.AddWithValue("@CodigoVendedor", cliente.CodigoVendedor);
                cmd.Parameters.AddWithValue("@CodigoUser", cliente.CodigoUser);
                cmd.Parameters.AddWithValue("@CodigoUserUlt", cliente.CodigoUserUlt);
                cmd.CommandType = CommandType.StoredProcedure;

                try
                {
                    connection.Open();

                    int affectedRows = cmd.ExecuteNonQuery();
                    if (affectedRows > 0)
                    {
                        response = true;
                    }

                    return response;
                }
                catch (Exception)
                {
                    throw;
                }
            }
        }

        public bool Edit(Cliente cliente)
        {
            bool successfulEdit = false;

            using (SqlConnection connection = new SqlConnection(Conexion.ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("sp_EditarCliente", connection);
                cmd.Parameters.AddWithValue("@CodigoCliente", cliente.CodigoCliente);
                cmd.Parameters.AddWithValue("@Nombres", cliente.Nombres);
                cmd.Parameters.AddWithValue("@Apellidos", cliente.Apellidos);
                cmd.Parameters.AddWithValue("@NumeroIdentificacion", cliente.NumeroIdentificacion);
                cmd.Parameters.AddWithValue("@RazonSocial", cliente.RazonSocial);
                cmd.Parameters.AddWithValue("@Direccion", cliente.Direccion);
                cmd.Parameters.AddWithValue("@Telefono", cliente.Telefono);
                cmd.Parameters.AddWithValue("@CorreoElectronico", cliente.CorreoElectronico);
                cmd.Parameters.AddWithValue("@CuentaContable", cliente.CuentaContable);
                cmd.Parameters.AddWithValue("@LimiteCredito", cliente.LimiteCredito);
                cmd.Parameters.AddWithValue("@DiasCredito", cliente.DiasCredito);
                cmd.Parameters.AddWithValue("@ExcentoImpuestos", cliente.ExcentoImpuestos);
                cmd.Parameters.AddWithValue("@Activo", cliente.Activo);
                cmd.Parameters.AddWithValue("@Distribuidor", cliente.Distribuidor);
                cmd.Parameters.AddWithValue("@PersonaJuridica", cliente.PersonaJuridica);
                cmd.Parameters.AddWithValue("@Externo", cliente.Externo);
                cmd.Parameters.AddWithValue("@CodigoPais", cliente.CodigoPais);
                cmd.Parameters.AddWithValue("@CodigoEmpresa", cliente.CodigoEmpresa);
                cmd.Parameters.AddWithValue("@CodigoSectorMercado", cliente.CodigoSectorMercado);
                cmd.Parameters.AddWithValue("@CodigoVendedor", cliente.CodigoVendedor);
                cmd.Parameters.AddWithValue("@CodigoUser", cliente.CodigoUser);
                cmd.Parameters.AddWithValue("@CodigoUserUlt", cliente.CodigoUserUlt);
                cmd.CommandType = CommandType.StoredProcedure;

                try
                {
                    connection.Open();

                    int affectedRows = cmd.ExecuteNonQuery();
                    if (affectedRows > 0)
                    {
                        successfulEdit = true;
                    }

                    return successfulEdit;
                }
                catch (Exception)
                {
                    throw;
                }
            }

        }

        public bool Delete(int CodigoCliente, int CodigoPais, int CodigoEmpresa)
        {
            bool response = false;

            using (SqlConnection connection = new SqlConnection(Conexion.ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("sp_EliminarCliente", connection);
                cmd.Parameters.AddWithValue("@CodigoCliente", CodigoCliente );
                cmd.Parameters.AddWithValue("@CodigoPais", CodigoPais);
                cmd.Parameters.AddWithValue("@CodigoEmpresa", CodigoEmpresa);

                cmd.CommandType = CommandType.StoredProcedure;
                try
                {
                    connection.Open();
                    int affectedRows = cmd.ExecuteNonQuery();
                    if (affectedRows > 0) response = true;



                    return response;

                }
                catch (Exception)
                {
                    throw;
                }
            }
        }
    }
}
