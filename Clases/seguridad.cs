using System.Net;
using static System.Net.Dns;
using System.Security.Principal;
using Microsoft.VisualBasic.CompilerServices;
using Microsoft.VisualBasic.ApplicationServices;

namespace FACTURACION_CLASS
{

    public class seguridad
    {

        #region PROPIEDADES DE LA CLASE

        private string _conn = string.Empty;
        private string _key = string.Empty;

        public static readonly string ConnectionString = @"Data Source=localhost;Server=GILDEDSWORD\SQLEXPRESS;Database=Facturacion;UID=sa;PWD=ga1234";

        public string Sql_conn
        {
            get
            {
                _conn = @"Data Source=localhost;Server=GILDEDSWORD\SQLEXPRESS;Database=Facturacion;UID=sa;PWD=ga1234";
                return _conn;
            }
            set
            {
                _conn = value;
            }
        }

        public string conn
        {
            get
            {
                _conn = @"Provider=SQLOLEDB.1;Server=GILDEDSWORD\SQLEXPRESS;Database=Facturacion;UID=sa;PWD=ga1234";
                return _conn;
            }
            set
            {
                _conn = value;
            }
        }

        public string CodigoSesion
        {
            get
            {
                return Conversions.ToString(CodigoSesion);
            }
            set
            {
                CodigoSesion = value;
            }
        }

        public string CodigoUsuario
        {
            get
            {
                return Conversions.ToString(CodigoUsuario);
            }
            set
            {
                CodigoUsuario = value;
            }
        }

        public string Nombre_Usuario
        {
            get
            {
                return Conversions.ToString(Nombre_Usuario);
            }
            set
            {
                Nombre_Usuario = value;
            }
        }

        public string Persona
        {
            get
            {
                return Conversions.ToString(Persona);
            }
            set
            {
                Persona = value;
            }
        }

        public string Password
        {
            get
            {
                return Conversions.ToString(Password);
            }
            set
            {
                Password = value;
            }
        }

        public string Key
        {
            get
            {
                _key = "SMARTIN2019";

                return _key;
            }
            set
            {
                _key = value;
            }
        }

        #endregion
        /// <summary>Returns Alert-Success-Error Message.</summary>
        /// <param name="Mensaje">Message</param>
        /// <param name="Tipo">Message Type: ["info", "exito", "alerta", "error"]</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public string PmsgBox(string Mensaje, string Tipo)
        {
            switch (Tipo ?? "")
            {
                case "info":
                    {
                        return "<div class='info'>" + Mensaje + "</div>";
                    }

                case "exito":
                    {
                        return "<div class='exito'>" + Mensaje + "</div>";
                    }

                case "alerta":
                    {
                        return "<div class='alerta'>" + Mensaje + "</div>";
                    }

                case "error":
                    {
                        return "<div class='error'>" + Mensaje + "</div>";
                    }

                default:
                    {
                        return "<div class='info'>" + Mensaje + "</div>";
                    }
            }
        }
        #region OBTENER DATOS DE LA MAQUINA CLIENTE

        // Obtener la direccion IP del PC del usuario.
        public string IpHost()
        {

            string nombrePc;
            IPHostEntry entradasIp;

            nombrePc = GetHostName();
            entradasIp = GetHostEntry(nombrePc);

            string direccionIp = entradasIp.AddressList[0].ToString();

            return direccionIp;
        }

        // Nombre del PC del usuario logueado
        public string NameHost()
        {
            return GetHostName();
        }

        // Retorna el nombre de la maquina cliente que inicia sesion.
        public string UserHost()
        {
            return WindowsIdentity.GetCurrent().Name;
        }

        #endregion

    }
}