﻿Imports System.Data
Imports System.Data.SqlClient
Imports System.Diagnostics
Imports FACTURACION_CLASS

Namespace FACTURACION_CLASS
    Public Class DropdownsClass

        Private Shared ReadOnly _database As New database
        Private Shared ReadOnly _seguridad As New seguridad

        Public Shared Sub BindDropDownListClientes(ddlCliente As DropDownList)
            Try

                Dim sql = $"SELECT CodigoCliente, TRIM(Nombres) + ' ' +TRIM(Apellidos) AS NombreCompleto
                    FROM Clientes WHERE Externo = {1}"
                If HttpContext.Current.Request.Cookies("CodigoPais") IsNot Nothing Then
                    sql &= $" AND CodigoPais = {HttpContext.Current.Request.Cookies("CodigoPais").Value}"
                End If
                If HttpContext.Current.Request.Cookies("CodigoEmpresa").Value IsNot Nothing Then
                    sql &= $" AND CodigoEmpresa = {HttpContext.Current.Request.Cookies("CodigoEmpresa").Value}"
                End If
                sql &= $" ORDER BY TRIM(Nombres)"

                Dim ds As DataSet = _database.GetDataSet(sql)

                If ds.Tables(0).Rows.Count > 0 Then
                    ddlCliente.DataSource = ds.Tables(0)
                    ddlCliente.DataTextField = "NombreCompleto"
                    ddlCliente.DataValueField = "CodigoCliente"
                    ddlCliente.DataBind()
                    ddlCliente.Items.Insert(0, New ListItem("", "0"))
                End If

            Catch ex As Exception
                'AlertifyErrorMessage(, "Ocurrio un error al intentar cargar los clientes. " & ex.Message)
            End Try
        End Sub

        Public Shared Sub BindDropDownListVendedor(ddlVendedor As DropDownList)
            Try
                Dim sql = $"SELECT cod_vendedor AS CodigoVendedor, TRIM(nombres) + ' ' +TRIM(apellidos) AS NombreCompleto
                    FROM Vendedores"
                If HttpContext.Current.Request.Cookies("CodigoPais") IsNot Nothing Then
                    sql &= $" WHERE cod_pais= {HttpContext.Current.Request.Cookies("CodigoPais").Value}"
                End If
                If HttpContext.Current.Request.Cookies("CodigoEmpresa") IsNot Nothing Then
                    sql &= $" AND cod_pais= {HttpContext.Current.Request.Cookies("CodigoEmpresa").Value}"
                End If
                If HttpContext.Current.Request.Cookies("CodigoPuesto") IsNot Nothing Then
                    sql &= $" AND cod_empresa= {HttpContext.Current.Request.Cookies("CodigoPuesto").Value} "
                End If
                sql &= $" ORDER BY TRIM(Nombres)"

                Dim ds As DataSet = _database.GetDataSet(sql)

                If ds.Tables(0).Rows.Count > 0 Then
                    ddlVendedor.DataSource = ds.Tables(0)
                    ddlVendedor.DataTextField = "NombreCompleto"
                    ddlVendedor.DataValueField = "CodigoVendedor"
                    ddlVendedor.DataBind()
                    ddlVendedor.Items.Insert(0, New ListItem("", "0"))
                End If

            Catch ex As Exception
                Throw ex
            End Try
        End Sub

        Public Shared Sub BindDropDownListFormaPago(ddlFormaPago As DropDownList)
            Try
                Dim sql = $"SELECT cod_FormaPago as CodigoFormaPago,
                    TRIM(descripcion) as Descripcion FROM Forma_Pago
                    WHERE cod_pais= {HttpContext.Current.Request.Cookies("CodigoPais").Value}
                    AND cod_empresa= {HttpContext.Current.Request.Cookies("CodigoEmpresa").Value}
                    AND cod_puesto= {HttpContext.Current.Request.Cookies("CodigoPuesto").Value}
                    ORDER BY TRIM(Descripcion)"

                Dim ds As DataSet = _database.GetDataSet(sql)

                If ds.Tables(0).Rows.Count > 0 Then
                    ddlFormaPago.DataSource = ds.Tables(0)
                    ddlFormaPago.DataTextField = "Descripcion"
                    ddlFormaPago.DataValueField = "CodigoFormaPago"
                    ddlFormaPago.DataBind()
                    ddlFormaPago.Items.Insert(0, New ListItem("--Seleccione Forma Pago--", "0"))
                End If

            Catch ex As Exception
                Throw ex
            End Try
        End Sub

        Public Shared Sub BindDropDownListMoneda(ddl As DropDownList)
            Try
                Dim sql = $"SELECT cod_moneda AS CodigoMoneda, descripcion
                        FROM Monedas WHERE cod_pais = {HttpContext.Current.Request.Cookies("CodigoPais").Value}"

                Dim ds As DataSet = _database.GetDataSet(sql)

                If ds.Tables(0).Rows.Count > 0 Then
                    ddl.DataSource = ds.Tables(0)
                    ddl.DataTextField = "descripcion"
                    ddl.DataValueField = "CodigoFormaPago"
                    ddl.DataBind()
                    ddl.Items.Insert(0, New ListItem("--Seleccione Forma Pago--", "0"))
                End If

            Catch ex As Exception
                Throw ex
            End Try
        End Sub

        Public Shared Sub BindDropDownListBanco(ddl As DropDownList)
            Try
                Dim sql = $"SELECT cod_banco AS CodigoBanco, descripcion
                        FROM Bancos WHERE cod_pais = {HttpContext.Current.Request.Cookies("CodigoPais").Value}"

                Dim ds As DataSet = _database.GetDataSet(sql)

                If ds.Tables(0).Rows.Count > 0 Then
                    ddl.DataSource = ds.Tables(0)
                    ddl.DataTextField = "descripcion"
                    ddl.DataValueField = "CodigoBanco"
                    ddl.DataBind()
                    ddl.Items.Insert(0, New ListItem("--Seleccione Banco--", "0"))
                End If

            Catch ex As Exception
                Throw ex
            End Try
        End Sub

        Public Shared Sub BindDropDownListCuentaBanco(ddl As DropDownList, selectedValue As Integer)
            Try
                Dim sql = $"SELECT cod_banco_cta AS CodigoCuentaBanco, descripcion
                        FROM Bancos_cuenta
                        WHERE cod_banco = {selectedValue}
                        AND cod_pais = {HttpContext.Current.Request.Cookies("CodigoPais").Value}
                        AND cod_empresa= {HttpContext.Current.Request.Cookies("CodigoEmpresa").Value}
                        AND cod_puesto= {HttpContext.Current.Request.Cookies("CodigoPuesto").Value}"

                Dim ds As DataSet = _database.GetDataSet(sql)

                If ds.Tables(0).Rows.Count > 0 Then
                    ddl.DataSource = ds.Tables(0)
                    ddl.DataTextField = "descripcion"
                    ddl.DataValueField = "CodigoCuentaBanco"
                    ddl.DataBind()
                    ddl.Items.Insert(0, New ListItem("--Seleccione Cuenta --", "0"))
                End If

            Catch ex As Exception
                Throw ex
            End Try
        End Sub

        Public Shared Sub BindDropDownList(ddl As DropDownList, sqlQuery As String, valueField As String, textField As String, defaultText As String)
            Try
                Using connection As New SqlConnection(_seguridad.Sql_conn)
                    connection.Open()

                    Using cmd As New SqlCommand(sqlQuery, connection)
                        Using dr As SqlDataReader = cmd.ExecuteReader()
                            ddl.Items.Clear()

                            While dr.Read()
                                Dim listItem As New ListItem()
                                listItem.Value = dr(valueField).ToString()
                                listItem.Text = dr(textField).ToString()
                                ddl.Items.Add(listItem)

                            End While

                            ddl.Items.Insert(0, New ListItem(defaultText, "0"))

                        End Using
                    End Using
                End Using
            Catch ex As Exception
                ' Handle the exception, log, or throw it as needed
                ' For example: Console.WriteLine(ex.Message)
                Throw ex
                Debug.WriteLine(ex.Message)
            End Try
        End Sub

    End Class
End Namespace