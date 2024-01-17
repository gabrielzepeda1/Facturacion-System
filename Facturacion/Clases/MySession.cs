using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Esta clase contiene el Objeto Session del usuario.
/// Definimos las propiedades que necesitamos para utilizarse tanto como en los forms .aspx u otras clases .vb.
/// </summary>
public class MySession
{
    //Properties
    public string Username { get; set; }
    public int? CodigoUser { get; set; }
    public int? CodigoRol { get; set; }
    public int? CodigoSesion { get; set; }
    public int? CodigoPais { get; set; }
    public int? CodigoEmpresa { get; set; }
    public int? CodigoPuesto { get; set; }

    private MySession()
    {
        Username = HttpContext.Current.Session["Username"] != null ? HttpContext.Current.Session["Username"].ToString() : null;
        CodigoUser = HttpContext.Current.Session["CodigoUser"] ? Convert.ToInt32(HttpContext.Current.Session["CodigoUser"]) : null;
        CodigoRol = HttpContext.Current.Session["CodigoRol"] != null ? Convert.ToInt32(HttpContext.Current.Session["CodigoRol"]) : null;
        CodigoSesion = HttpContext.Current.Session["CodigoSesion"] != null ? Convert.ToInt32(HttpContext.Current.Session["CodigoSesion"]) : null;
        CodigoPais = HttpContext.Current.Session["CodigoPais"] != null ? Convert.ToInt32(HttpContext.Current.Session["CodigoPais"]) : null;
        CodigoEmpresa = HttpContext.Current.Session["CodigoEmpresa"] != null ? Convert.ToInt32(HttpContext.Current.Session["CodigoEmpresa"]) : null;
        CodigoPuesto = HttpContext.Current.Session["CodigoPuesto"] != null ? Convert.ToInt32(HttpContext.Current.Session["CodigoPuesto"]) : null;

    }

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

    public string GetWelcomeMessage()
    {
        if (!string.IsNullOrEmpty(this.Username))
            return "Bienvenido " + this.Username;
        else
            return "Bienvenido";

    }

}