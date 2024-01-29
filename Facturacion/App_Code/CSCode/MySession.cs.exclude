using System;
using System.Activities.Statements;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;

/// <summary>
/// Esta clase contiene el Objeto Session del usuario.
/// Definimos las propiedades que necesitamos para utilizarse tanto como en los forms .aspx u otras clases .vb.
/// </summary>
///

namespace HelperClasses
{
    public class MySession
    {
        //Properties
        public string Username { get; set; }
        public int CodigoUser { get; set; }
        public int CodigoRol { get; set; }
        public int CodigoSesion { get; set; }
        public Object CodigoPais { get; set; }
        public Object CodigoEmpresa { get; set; }
        public Object CodigoPuesto { get; set; }

        private MySession()
        {
            Username = HttpContext.Current.Session["Username"].ToString();
            CodigoUser = Convert.ToInt32(HttpContext.Current.Session["CodigoUser"]);
            CodigoRol = Convert.ToInt32(HttpContext.Current.Session["CodigoRol"]);
            CodigoSesion = Convert.ToInt32(HttpContext.Current.Session["CodigoSesion"]);
            CodigoPais = Convert.ToInt32(HttpContext.Current.Session["CodigoPais"]);
            CodigoEmpresa = Convert.ToInt32(HttpContext.Current.Session["CodigoEmpresa"]);
            CodigoPuesto = Convert.ToInt32(HttpContext.Current.Session["CodigoPuesto"]);
        }

        //Get Current Session
        public static MySession Current
        {
            get
            {
                MySession session = (MySession)HttpContext.Current.Session["__MySession__"];
                if (session == null)
                {
                    session = new MySession();
                    HttpContext.Current.Session["__MySession__"] = session;
                }
                return session;
            }
        }

    }
}
//```
//Y en mi form aspx.cs lo utilizo de esta manera:
//```
//protected void Page_Load(object sender, EventArgs e)
//{
//    if (!Page.IsPostBack)
//    {
//        if (MySession.Current.CodigoUser != null)
//        {
//            //Do something
//        }
//        else
//        {
//            Response.Redirect("~/Login.aspx");
//        }
//    }
//}
//```
//Espero te sea de utilidad.
//Saludos.