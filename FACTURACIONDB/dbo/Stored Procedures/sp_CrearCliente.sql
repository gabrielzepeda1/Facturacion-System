
CREATE PROC [dbo].[sp_CrearCliente]( 

@CodigoPais INT,
@CodigoEmpresa INT,
@Externo BIT,
@CodigoCliente VARCHAR(4),
@Activo BIT,
@PersonaJuridica BIT, 
@Distribuidor BIT, 
@Nombres VARCHAR(30),
@Apellidos VARCHAR(30),
@NumeroIdentificacion VARCHAR(30),
@RazonSocial VARCHAR(50),
@Direccion VARCHAR(50),
@Telefono VARCHAR(15),
@CorreoElectronico VARCHAR(30),
@CuentaContable VARCHAR(25),
@LimiteCredito NUMERIC(18,2),
@DiasCredito INT,
@ExcentoImpuestos BIT,
@CodigoSectorMercado INT,
@CodigoVendedor INT,
@CodigoUser INT, 
@CodigoUserUlt INT
--@Busqueda VARCHAR(200) 

)

AS 
BEGIN 
	SET DATEFORMAT DMY 
	INSERT INTO Clientes (
						CodigoPais, 
						CodigoEmpresa,
						Externo, 
						CodigoCliente,
						Activo,
						PersonaJuridica, 
						Distribuidor, 
						Nombres, 
						Apellidos,
						NumeroIdentificacion,
						RazonSocial, 
						Direccion, 
						Telefono, 
						CorreoElectronico, 
						CuentaContable,
						LimiteCredito, 
						DiasCredito, 
						ExcentoImpuestos, 
						CodigoSectorMercado, 
						CodigoVendedor, 
						CodigoUser, 
						CodigoUserUlt 
						)
		VALUES (
		@CodigoPais, 
		@CodigoEmpresa, 
		@Externo, 
		@CodigoCliente, 
		@Activo, 
		@PersonaJuridica,
		@Distribuidor,
		@Nombres, 
		@Apellidos,
		@NumeroIdentificacion, 
		@RazonSocial, 
		@Direccion, 
		@Telefono, 
		@CorreoElectronico, 
		@CuentaContable, 
		@LimiteCredito, 
		@DiasCredito, 
		@ExcentoImpuestos, 
		@CodigoSectorMercado, 
		@CodigoVendedor, 
		@CodigoUser, 
		@CodigoUserUlt 
		
		
		)
	
END