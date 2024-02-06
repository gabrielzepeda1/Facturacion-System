
CREATE FUNCTION [dbo].[GetCodigoEmpresaUsuario](@CodigoUsuario INT)
RETURNS INT 
AS 
BEGIN  
	DECLARE @CodigoEmpresa INT; 
	
	SET @CodigoEmpresa = (
	SELECT u.cod_empresa
	FROM sys_usuario u
	WHERE consecutivo_usuario = 4
	) 

	RETURN @CodigoEmpresa 
	
		
END 
