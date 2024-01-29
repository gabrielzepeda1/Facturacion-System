CREATE FUNCTION [dbo].[fn_clientes]() 
returns table 
as 
return 
	SELECT 
			c.CodigoCliente,
			c.Nombres, 
			c.Apellidos, 
			c.NumeroIdentificacion, 
			c.RazonSocial,
			c.Direccion, 
			c.Telefono, 
			c.CorreoElectronico,
			c.CuentaContable, 
			c.LimiteCredito, 
			c.DiasCredito,
			c.ExcentoImpuestos, 
			c.Activo,
			c.Distribuidor,
			c.PersonaJuridica,
			c.Externo,
			c.CodigoPais, 
			c.CodigoEmpresa, 
			c.CodigoSectorMercado, 
			c.CodigoVendedor, 
			RTRIM(ltrim(v.nombres))+' '+RTRIM(ltrim(v.apellidos)) AS Vendedor,
			c.CodigoUser, 
			c.CodigoUserUlt
	FROM Clientes c
	INNER JOIN Vendedores v on v.cod_vendedor = c.CodigoVendedor 
