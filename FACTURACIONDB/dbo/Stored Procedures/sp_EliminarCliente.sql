CREATE PROC sp_EliminarCliente( 

@CodigoCliente VARCHAR(4),
@CodigoPais INT,
@CodigoEmpresa INT



)

AS 
BEGIN 
	SET DATEFORMAT DMY 
	
	DELETE FROM Clientes 
	
	WHERE CodigoCliente = @CodigoCliente
	AND CodigoPais = @CodigoPais 
	AND CodigoEmpresa = @CodigoEmpresa
		
END