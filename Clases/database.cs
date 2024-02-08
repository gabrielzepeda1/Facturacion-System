using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Diagnostics;
using System.IO;
using Microsoft.VisualBasic.CompilerServices;

namespace FACTURACION_CLASS
{

    public class database
    {
        private readonly seguridad conn = new seguridad();

        /// <summary>
        /// CREA UNA COPIA DEL ARCHIVO QUE SERA ADJUNTO EN MEMORIA, Y LUEGO BORRA DICHA COPIA
        /// ESTO EVITA EL ERROR DE USO DEL SERVIDOR
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        public Stream GetStreamFile(string filePath)
        {

            using (var fileStream = File.OpenRead(filePath))
            {
                var memStream = new MemoryStream();

                memStream.SetLength(fileStream.Length);

                fileStream.Read(memStream.GetBuffer(), 0, (int)fileStream.Length);

                return memStream;
            }

        }
        /// <summary>
        /// Retorna un DataSet
        /// </summary>
        /// <param name="query">SENTENCIA SQL QUE SERA ENVIADA A SQL SERVER</param>
        /// <returns>DataSet</returns>
        /// <remarks></remarks>
        public DataSet GetDataSet(string query)
        {

            try
            {
                using (var dbCon = new OleDbConnection(conn.conn))
                {
                    dbCon.Open();

                    string sql = "SET DATEFORMAT DMY ";
                    sql = sql + Microsoft.VisualBasic.Constants.vbCrLf + query;

                    using (var cmd = new OleDbCommand(sql, dbCon))
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.CommandTimeout = 9999;
                        using (var da = new OleDbDataAdapter(cmd))
                        {
                            using (var ds = new DataSet())
                            {
                                da.Fill(ds);
                                return ds;
                            }
                        }
                    }
                }
            }

            catch (Exception ex)
            {
                throw ex;
            }

            return null;
        }
        /// <summary>
        /// Retorna un DataTable
        /// </summary>
        /// <param name="query"></param>
        /// <returns>DataTable</returns>
        /// <remarks></remarks>
        public DataTable GetDataTable(string query)
        {
            try
            {
                using (var dbCon = new OleDbConnection(conn.conn))
                {
                    var dt = new DataTable();
                    dbCon.Open();

                    using (var cmd = new OleDbCommand(query, dbCon))
                    {
                        using (var da = new OleDbDataAdapter())
                        {
                            cmd.CommandType = CommandType.Text;
                            cmd.Connection = dbCon;
                            da.SelectCommand = cmd;
                            da.Fill(dt);
                        }
                    }
                    return dt;
                }
            }

            catch (Exception ex)
            {
                conn.PmsgBox("Error en clase database: " + ex.Message, "error");
            }
            return null;
        }

        /// <summary>
        /// DEVUELVE UN CONTROL DATATABLE
        /// </summary>
        /// <param name="query">SENTENCIA SQL QUE SERA ENVIADA A SQL SERVER</param>
        /// <returns>DataTable</returns>
        /// <remarks></remarks>
        public DataTable GetDataTableProc(string query)
        {

            try
            {
                using (var dbCon = new OleDbConnection(conn.conn))
                {

                    string sql = "SET DATEFORMAT DMY ";

                    sql = sql + Microsoft.VisualBasic.Constants.vbCrLf + query;

                    // Dim daSrc As New System.Data.OleDb.OleDbDataAdapter(sql, dbCon)
                    // Dim dt As New DataTable("Factura")
                    // daSrc.Fill(dt)

                    // Return dt
                    // dt.Dispose()

                    var cmd = new OleDbCommand(sql, dbCon);
                    cmd.CommandTimeout = 9999;

                    var da = new OleDbDataAdapter(cmd);
                    var ds = new DataSet();
                    da.Fill(ds, "DT0");

                    return ds.Tables[0];
                    ds.Dispose();

                }
            }

            catch (Exception ex)
            {
                conn.PmsgBox("Error en clase de database: " + ex.Message, "error");
            }

            return default;
        }

        /// <summary>Esta función ejecuta un query de sql y retorna un OleDbDataReader</summary>
        /// <param name="sql">sql query</param>
        /// <returns>OleDbDataReader</returns>
        /// <remarks></remarks>
        public OleDbDataReader GetDataReader(string sql)
        {
            try
            {
                using (var dbCon = new OleDbConnection(conn.conn))
                {
                    dbCon.Open();

                    using (var cmd = new OleDbCommand(sql, dbCon))
                    {
                        cmd.CommandTimeout = 9999;
                        var dr = cmd.ExecuteReader();
                        return dr;
                    }
                }
            }
            catch (OleDbException ex)
            {
                conn.PmsgBox("Error en clase de database: " + ex.Message, "error");
                return null;
            }
        }

        public OleDbDataReader GetScalar(string sql)
        {
            try
            {
                using (var dbCon = new OleDbConnection(conn.conn))
                {
                    dbCon.Open();

                    using (var cmd = new OleDbCommand(sql, dbCon))
                    {
                        cmd.CommandType = CommandType.Text;
                        var result = cmd.ExecuteScalar();

                        if (!ReferenceEquals(result, DBNull.Value))
                        {
                            return (OleDbDataReader)result;
                        }

                        return null;
                    }
                }
            }
            catch (OleDbException ex)
            {
                conn.PmsgBox("Error en clase de database: " + ex.Message, "error");
                return null;
            }
        }

        /// <summary>
        /// DEVUELVE UN VALOR BOOLEANO QUE ES OBTENIDO ATRAVEZ DE UN DATAREADER. SI EL VALOR OBTENIDO ES "SI" DEVUELVE TRUE EN CASO CONTRARIO DEVUELVE FALSE
        /// </summary>
        /// <param name="Query">Procedimiento de sql con todos los paramentros. NO PASAR DATAFORMAT</param>
        /// <param name="campo">NOMBRE DEL CAMPO QUE SERA FILTRADO EN EL DATAREADER</param>
        /// <returns>BOOLEAN</returns>
        /// <remarks></remarks>
        public bool GetBoleano(string Query, string campo)
        {
            try
            {
                using (var dbCon = new OleDbConnection(conn.conn))
                {
                    dbCon.Open();

                    string sql = "SET DATEFORMAT DMY ";
                    sql = sql + Microsoft.VisualBasic.Constants.vbCrLf + Query;

                    using (var cmd = new OleDbCommand(sql, dbCon))
                    {
                        using (var dr = cmd.ExecuteReader())
                        {
                            if (dr.Read())
                            {
                                return Conversions.ToBoolean(dr[campo]);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                conn.PmsgBox("Error en clase de database: " + ex.Message, "error");
            }

            return false;
        }

        /// <summary>
        /// DEVUELVE UN VALOR STRING QUE ES OBTENIDO ATRAVEZ DE UN DATAREADER
        /// </summary>
        /// <param name="Query">Procedimiento de sql con todos los paramentros. NO PASAR DATAFORMAT</param>
        /// <param name="campo">NOMBRE DEL CAMPO QUE SERA FILTRADO EN EL DATAREADER</param>
        /// <returns>DATAREADER</returns>
        /// <remarks></remarks>
        public string GetString(string Query, string campo)
        {
            string cnn = conn.conn;
            var dbCon = new OleDbConnection(cnn);
            try
            {
                if (dbCon.State == ConnectionState.Closed)
                {
                    dbCon.Open();
                }

                string sql = "SET DATEFORMAT DMY ";

                sql = sql + Microsoft.VisualBasic.Constants.vbCrLf + Query;

                var cmd = new OleDbCommand(sql, dbCon);
                OleDbDataReader dr;
                dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    return dr[campo].ToString();
                }
                else
                {
                    return string.Empty;
                }

                dr.Close();
            }

            catch (Exception ex)
            {
                conn.PmsgBox("Error en clase de database: " + ex.Message, "error");
                return string.Empty;
            }
        }

        public bool SaveToDatabase(string sql)
        {

            try
            {
                using (var dbCon = new OleDbConnection(conn.conn))
                {
                    dbCon.Open();

                    using (var cmd = new OleDbCommand(sql, dbCon))
                    {
                        int affectedRows = cmd.ExecuteNonQuery();

                        if (affectedRows > 0)
                        {
                            return true;
                        }

                        return false;

                    }
                }
            }

            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<int> GetPaisesUsuario(int CodigoUser)
        {

            string sql = $"SELECT * FROM GetDistinctPaisesUsuario({CodigoUser})";
            var reader = GetDataReader(sql);
            var ArrCodigoPais = new List<int>();

            while (reader.Read())
                ArrCodigoPais.Add(Conversions.ToInteger(reader["cod_pais"]));

            Debug.WriteLine(ArrCodigoPais);

            return ArrCodigoPais;

        }

        public List<int> GetEmpresasUsuario(int CodigoUser)
        {

            string sql = $"SELECT * FROM GetDistinctEmpresasUsuario({CodigoUser})";
            var reader = GetDataReader(sql);
            var ArrCodigoEmpresa = new List<int>();

            while (reader.Read())
                ArrCodigoEmpresa.Add(Conversions.ToInteger(reader["cod_empresa"]));

            Debug.WriteLine(ArrCodigoEmpresa);
            return ArrCodigoEmpresa;
        }

        public List<int> GetPuestosUsuario(int CodigoUser)
        {

            string sql = $"SELECT * FROM GetDistinctPuestosUsuario({CodigoUser})";
            var reader = GetDataReader(sql);
            var ArrCodigoPuesto = new List<int>();

            while (reader.Read())
                ArrCodigoPuesto.Add(Conversions.ToInteger(reader["cod_empresa"]));

            Debug.WriteLine(ArrCodigoPuesto);
            return ArrCodigoPuesto;
        }

    }
}