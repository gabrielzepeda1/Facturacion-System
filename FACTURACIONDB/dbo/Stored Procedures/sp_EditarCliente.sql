
CREATE PROC [dbo].[sp_EditarCliente]( 

@CodigoCliente VARCHAR(4),
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
@Activo BIT,
@Distribuidor BIT, 
@PersonaJuridica BIT, 
@Externo BIT,
@CodigoPais INT,
@CodigoEmpresa INT,
@CodigoSectorMercado INT,
@CodigoVendedor INT,
@CodigoUser INT, 
@CodigoUserUlt INT

)

AS 
BEGIN 
	SET DATEFORMAT DMY 
	
	UPDATE Clientes SET  
	Nombres = @Nombres, 
	Apellidos = @Apellidos, 
	NumeroIdentificacion = @NumeroIdentificacion,
	RazonSocial = @RazonSocial, 
	Direccion = @Direccion, 
	Telefono = @Telefono, 
	CorreoElectronico = @CorreoElectronico, 
	CuentaContable = @CuentaContable, 
	LimiteCredito = @LimiteCredito, 
	DiasCredito = @DiasCredito, 
	ExcentoImpuestos = @ExcentoImpuestos, 
	Activo = @Activo, 
	Distribuidor = @Distribuidor, 
	PersonaJuridica = @PersonaJuridica, 
	Externo = @Externo, 
	CodigoPais = @CodigoPais, 
	CodigoEmpresa = @CodigoEmpresa, 
	CodigoSectorMercado = @CodigoSectorMercado, 
	CodigoVendedor = @CodigoVendedor, 
	CodigoUser = @CodigoUser, 
	CodigoUserUlt = @CodigoUserUlt
	
	WHERE CodigoCliente = @CodigoCliente
	AND CodigoPais = @CodigoPais 
	AND CodigoEmpresa = @CodigoEmpresa
		
END