CREATE FUNCTION [dbo].[fn_cliente](@CodigoCliente int, @CodigoPais int, @CodigoEmpresa int ) 
returns table 
as 
return 
	SELECT c.Nombres, 
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
	c.CodigoVendedor,
	RTRIM(ltrim(v.nombres))+' '+RTRIM(ltrim(v.apellidos)) AS Vendedor, 
	c.CodigoSectorMercado, 
	c.CodigoUser, 
	c.CodigoUserUlt


	FROM Clientes c
	INNER JOIN Vendedores v on v.cod_vendedor = c.CodigoVendedor 
	WHERE c.CodigoCliente= @CodigoCliente
	AND c.CodigoPais = @CodigoPais 
	AND c.CodigoEmpresa = @CodigoEmpresa