CREATE FUNCTION GetCod_PaisUsuario(@CodigoUsuario INT)
RETURNS INT 
AS 
BEGIN 
	DECLARE @CodigoPais INT; 
	
	SET @CodigoPais = (
	SELECT u.cod_pais
	FROM sys_usuario u
	WHERE consecutivo_usuario = @CodigoUsuario
	) 

	RETURN @CodigoPais 
	
		
END 