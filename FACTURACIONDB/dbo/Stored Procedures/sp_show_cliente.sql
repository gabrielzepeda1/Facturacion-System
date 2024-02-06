CREATE PROCEDURE [dbo].[sp_show_cliente]
@CodigoCliente VARCHAR(4)
AS 
SELECT 
	 CodigoCliente,  Nombres,  Apellidos, RazonSocial,  Direccion,  Telefono,  CorreoElectronico,  NumeroIdentificacion,  CuentaContable,  DiasCredito,  LimiteCredito, 

	 CASE WHEN Activo = 1 THEN 'Si' ELSE 'No' END AS Activo,
     CASE WHEN  Externo = 1 THEN 'Si' ELSE 'No' END AS Externo, 
	 CASE WHEN  ExcentoImpuestos = 1 THEN 'Si' ELSE 'No' END AS ExcentoImpuestos, 
	 CASE WHEN  Distribuidor = 1 THEN 'Si' ELSE 'No' END AS Distribuidor,  
	 CodigoSectorMercado,  CodigoVendedor

FROM Clientes 
WHERE CodigoCliente = @CodigoCliente