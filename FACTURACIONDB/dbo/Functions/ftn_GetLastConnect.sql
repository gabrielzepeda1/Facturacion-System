CREATE FUNCTION [dbo].[ftn_GetLastConnect](@cod_usuario INT)
RETURNS DATETIME
AS
BEGIN 
	DECLARE @MaxConexion DATETIME
	
	SET @MaxConexion = (SELECT MAX(scs.Fecha_Inicio_Sesion) FROM sys_Control_Sesiones scs WHERE scs.consecutivo_usuario = @cod_usuario)
	
	IF @MaxConexion IS NULL BEGIN
		SET @MaxConexion = GETDATE()
	END
	
	RETURN @MaxConexion

END