using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using SM.Entity;
using SM.Business;
using FACTURACION_CLASS;
using SM.Data;
using System.Globalization;
using System.Activities.Expressions;
using System.Data;
using System.Data.SqlClient;

public partial class catalogos_CatalogoClientes : System.Web.UI.Page
{
    //Variable para almacenar el codigo del cliente seleccionado
    ClienteBL clienteBL = new ClienteBL(); //Crear nueva instancia de CLIENTE Business Layer

    private Cliente cliente;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            LoadGV();
            AddAtributes();
            DdlMercado();
            DdlVendedor();

        }

        ScriptManager.RegisterStartupScript(this, this.GetType(), "setCheckBoxScript", "setCheckBox()", true);

        txtApellido.Enabled = true;
        txtApellido.Visible = true;
        lblApellido.Visible = true;

    }
    protected void btnNew_Click(object sender, EventArgs e)
    {
        Clear();
        
        lblTitulo.Text = "Nuevo Cliente";
        BtnGuardar.Text = "Guardar";
        txtCodigoCliente.Enabled = true;
        ScriptManager.RegisterStartupScript(this, this.GetType(), "script", "open_popup()", true);

    }
    protected void BtnGuardar_Click(object sender, EventArgs e)
    {
        int CodigoCliente = Convert.ToInt32(txtCodigoCliente.Text);
        int CodigoPais = Convert.ToInt32(Session["cod_pais"]);
        int CodigoEmpresa = Convert.ToInt32(Session["cod_empresa"]);
        bool nuevoCliente = false;

        // Necesitamos validaciones que impidan que el proceso continue si los campos requeridos no han sido llenados 
        // Validacion que verifique que el CodigoPais y CodigoEmpresa != 0 

        try
        {
            if (clienteBL.ClienteExists(CodigoCliente, CodigoPais, CodigoEmpresa))
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ClientIDExistsAlert", "alert(`El Codigo Cliente ${CodigoCliente} ya existe en la base de datos. Por favor ingrese un código diferente.`);", true);
                return;

            }
            else if (clienteBL.ClienteExists(CodigoCliente, CodigoPais, CodigoEmpresa) == false)
            {
                nuevoCliente = true;
            }

            if (nuevoCliente)
            {
                Cliente cliente = new Cliente()
                {
                    CodigoCliente = Convert.ToInt32(txtCodigoCliente.Text),
                    Nombres = txtNombre.Text,
                    Apellidos = txtApellido.Text,
                    RazonSocial = txtRazonSocial.Text,
                    Direccion = txtDireccion.Text,
                    Telefono = txtTelefono.Text,
                    CorreoElectronico = txtCorreoElectronico.Text,
                    NumeroIdentificacion = txtIdentificacion.Text,
                    CuentaContable = txtCuentaContable.Text,
                    DiasCredito = Convert.ToInt32(txtDiasCredito.Text),
                    LimiteCredito = Convert.ToDecimal(txtLimiteCredito.Text),
                    CodigoSectorMercado = Convert.ToInt32(ddlMercado.SelectedValue),
                    CodigoVendedor = Convert.ToInt32(ddlVendedor.SelectedValue),
                    CodigoPais = Convert.ToInt32(Session["cod_pais"]),
                    CodigoEmpresa = Convert.ToInt32(Session["cod_empresa"]),
                    CodigoUser = Convert.ToInt32(Context.Request.Cookies["CKSMFACTURA"]["cod_usuario"]),
                    CodigoUserUlt = Convert.ToInt32(Context.Request.Cookies["CKSMFACTURA"]["cod_usuario"]),
                    Activo = chkActivo.Checked,
                    ExcentoImpuestos = chkExcentoImpuestos.Checked,
                    Externo = chkExterno.Checked,
                    Distribuidor = chkDistribuidor.Checked,
                    PersonaJuridica = chkPersonaJuridica.Checked,
                };

                if (cliente != null && CodigoCliente != 0 && CodigoPais != 0 && CodigoEmpresa != 00)
                {
                    clienteBL.Create(cliente);

                    ScriptManager.RegisterStartupScript(this, this.GetType(), "closePopupScript", "close_popup()", true);
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "successfulEdit", "alert('Los cambios al cliente se han guardado con exito.')", true);

                }
            }

            else if (!nuevoCliente) //Proceso ACTUALIZAR
            {
           
                cliente = clienteBL.GetCliente(CodigoCliente, CodigoPais, CodigoEmpresa);

                cliente.Nombres = txtNombre.Text;
                cliente.Apellidos = txtApellido.Text;
                cliente.RazonSocial = txtRazonSocial.Text;
                cliente.Direccion = txtDireccion.Text;
                cliente.Telefono = txtTelefono.Text;
                cliente.CorreoElectronico = txtCorreoElectronico.Text;
                cliente.NumeroIdentificacion = txtIdentificacion.Text;
                cliente.CuentaContable = txtCuentaContable.Text;
                cliente.DiasCredito = Convert.ToInt32(txtDiasCredito.Text);
                cliente.LimiteCredito = Convert.ToDecimal(txtLimiteCredito.Text);
                cliente.CodigoSectorMercado = Convert.ToInt32(ddlMercado.SelectedValue);
                cliente.CodigoVendedor = Convert.ToInt32(ddlVendedor.SelectedValue);
                cliente.CodigoPais = Convert.ToInt32(Session["cod_pais"]);
                cliente.CodigoEmpresa = Convert.ToInt32(Session["cod_empresa"]);
                cliente.CodigoUser = Convert.ToInt32(Context.Request.Cookies["CKSMFACTURA"]["cod_usuario"]);
                cliente.CodigoUserUlt = Convert.ToInt32(Context.Request.Cookies["CKSMFACTURA"]["cod_usuario"]);
                cliente.Activo = chkActivo.Checked;
                cliente.ExcentoImpuestos = chkExcentoImpuestos.Checked;
                cliente.Externo = chkExterno.Checked;
                cliente.Distribuidor = chkDistribuidor.Checked;
                cliente.PersonaJuridica = chkPersonaJuridica.Checked;

                if (cliente != null && CodigoCliente != 0 && CodigoPais != 0 && CodigoEmpresa != 00)
                {
                    clienteBL.Edit(cliente);

                    ScriptManager.RegisterStartupScript(this, this.GetType(), "closePopupScript", "close_popup()", true);
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "successfulEdit", "alert('Los cambios al cliente se han guardado con exito.')", true);
                }

            }
        } 
        catch (Exception)
        {
            throw;
        }

    }

    protected void GVMain_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "MyEdit")
        {
            int CodigoCliente = Convert.ToInt32(e.CommandArgument);
            int CodigoPais = Convert.ToInt32(Session["cod_pais"]);
            int CodigoEmpresa = Convert.ToInt32(Session["cod_empresa"]);
            int CodigoUser = Convert.ToInt32(Context.Request.Cookies["CKSMFACTURA"]["cod_usuario"]);
            int CodigoUserUlt = Convert.ToInt32(Context.Request.Cookies["CKSMFACTURA"]["cod_usuario"]);

            if (CodigoCliente != 0)
            {
                lblTitulo.Text = "Editar Cliente";
                BtnGuardar.Text = "Actualizar";

                cliente = clienteBL.GetCliente(CodigoCliente, CodigoPais, CodigoEmpresa);

                txtCodigoCliente.Enabled = false;
                txtCodigoCliente.Text = CodigoCliente.ToString();
                txtNombre.Text = cliente.Nombres;
                txtApellido.Text = cliente.Apellidos;
                txtRazonSocial.Text = cliente.RazonSocial;
                txtDireccion.Text = cliente.Direccion;
                txtTelefono.Text = cliente.Telefono;
                txtCorreoElectronico.Text = cliente.CorreoElectronico;
                txtIdentificacion.Text = cliente.NumeroIdentificacion;
                txtCuentaContable.Text = cliente.CuentaContable;
                txtDiasCredito.Text = cliente.DiasCredito.ToString();
                txtLimiteCredito.Text = cliente.LimiteCredito.ToString();
                ddlMercado.SelectedValue = cliente.CodigoSectorMercado.ToString();
                ddlVendedor.SelectedValue = cliente.CodigoVendedor.ToString();
                chkActivo.Checked = cliente.Activo;
                chkExcentoImpuestos.Checked = cliente.ExcentoImpuestos;
                chkExterno.Checked = cliente.Externo;
                chkDistribuidor.Checked = cliente.Distribuidor;
                chkPersonaJuridica.Checked = cliente.PersonaJuridica;
                CodigoUser = cliente.CodigoUser;
                CodigoUserUlt = cliente.CodigoUserUlt;

                ScriptManager.RegisterStartupScript(this, this.GetType(), "setCheckBoxScript", "setCheckBox()", true);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "openPopupScript", "open_popup()", true);



            }
            else
            {
                lblTitulo.Text = "Nuevo Cliente";
                BtnGuardar.Text = "Guardar";

            }

        }

    }

    protected void btnEliminar_Click(object sender, EventArgs e)
    {
        LinkButton btn = (LinkButton)sender;
        int CodigoCliente = Convert.ToInt32(btn.CommandArgument);
        int CodigoPais = Convert.ToInt32(Session["cod_pais"]);
        int CodigoEmpresa = Convert.ToInt32(Session["cod_empresa"]);

        clienteBL.Delete(CodigoCliente, CodigoPais, CodigoEmpresa);
        ScriptManager.RegisterStartupScript(this, this.GetType(), "successfulEdit", "alert('Cliente ha sido eliminado.')", true);
        LoadGV();

    }




    protected void AddAtributes()
    {

        TextBox[] textBoxes = new TextBox[] { txtCodigoCliente, txtNombre, txtApellido, txtRazonSocial, txtDireccion, txtTelefono, txtCorreoElectronico, txtIdentificacion, txtCuentaContable, txtDiasCredito, txtLimiteCredito };

        DropDownList[] dropDowns = new DropDownList[] { ddlMercado, ddlVendedor };

        CheckBox[] checkBoxes = new CheckBox[] { chkPersonaJuridica, chkActivo, chkExterno, chkExcentoImpuestos, chkDistribuidor };

        //Dictionary<string, string> attributes = new Dictionary<string, string>
        //{
        //    { "required", "true" },
        //    { "background-color", "#F5F5F5" }
        //}

        foreach (TextBox textBox in textBoxes)
        {
            textBox.Attributes.Add("required", "true");
            textBox.Attributes.CssStyle.Add("background-color", "#F5F5F5");


        }

        foreach (DropDownList dropDown in dropDowns)
        {

            dropDown.Attributes.Add("required", "true");
            dropDown.Attributes.CssStyle.Add("background-color", "#F5F5F5");
        }

        foreach (CheckBox checkBox in checkBoxes)
        {
            checkBox.Attributes.Add("required", "true");

        }
    }
    protected void Clear()
    {
        txtCodigoCliente.Text = "";
        txtNombre.Text = "";
        txtApellido.Text = "";
        txtRazonSocial.Text = "";
        txtDireccion.Text = "";
        txtTelefono.Text = "";
        txtCorreoElectronico.Text = "";
        txtIdentificacion.Text = "";
        txtCuentaContable.Text = "";
        txtDiasCredito.Text = "";
        txtLimiteCredito.Text = "";
        ddlMercado.SelectedIndex = 0;
        ddlVendedor.SelectedIndex = 0;
        chkActivo.Checked = false;
        chkExcentoImpuestos.Checked = false;
        chkExterno.Checked = false;
        chkDistribuidor.Checked = false;
        chkPersonaJuridica.Checked = false;
    }
    private void LoadGV()
    {
        List<Cliente> lista = clienteBL.Lista(); //obtenemos la lista de clientes

        GVMain.DataSource = lista;
        GVMain.DataBind();
    }
    private void DdlMercado()
    {
        DataSet dataSet = new DataSet();

        using (SqlConnection connection = new SqlConnection(Conexion.ConnectionString))
        {
            SqlCommand cmd = new SqlCommand("CombosProductos", connection);
            cmd.Parameters.AddWithValue("@opcion", 13);
            cmd.Parameters.AddWithValue("@codigo", Session["cod_pais"]);
            cmd.CommandType = CommandType.StoredProcedure;

            try
            {

                connection.Open();
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                adapter.Fill(dataSet);

                ddlMercado.DataSource = dataSet;
                ddlMercado.DataTextField = "Mercado";
                ddlMercado.DataValueField = "codigoSecMerc";
                ddlMercado.DataBind();
                ddlMercado.Items.Insert(0, new ListItem("--SELECCIONE--", "0"));
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                

            }

        }
    }
    private void DdlVendedor()
    {

        DataSet dataSet = new DataSet();

        using (SqlConnection connection = new SqlConnection(Conexion.ConnectionString))
        {
            SqlCommand cmd = new SqlCommand("CombosProductos", connection);
            cmd.Parameters.AddWithValue("@opcion", 10);
            cmd.Parameters.AddWithValue("@codigo", Session["cod_pais"]);
            cmd.CommandType = CommandType.StoredProcedure;

            try
            {

                connection.Open();
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                adapter.Fill(dataSet);

                ddlVendedor.DataSource = dataSet;
                ddlVendedor.DataTextField = "Vendedor";
                ddlVendedor.DataValueField = "codVendedor";
                ddlVendedor.DataBind();
                ddlVendedor.Items.Insert(0, new ListItem("--SELECCIONE--", "0"));
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                

            }

        }

    }

    protected void chkPersonaJuridica_CheckedChanged(object sender, EventArgs e)
    {
        if (chkPersonaJuridica.Checked)
        {
            txtApellido.Enabled = false;
            txtApellido.Visible = false;
            lblApellido.Visible = false;
            //ScriptManager.RegisterStartupScript(this, this.GetType(), "hideApellidosScript", "hideApellidos()", true);

        } else

        {
            txtApellido.Enabled= true;
            txtApellido.Visible = true;
            lblApellido.Visible = true;
        }

    }

    protected void GVMain_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        GVMain.PageIndex = e.NewPageIndex;
        LoadGV();
    }
}