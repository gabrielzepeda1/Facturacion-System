CREATE FUNCTION [dbo].[ftn_GetUltimaConexion](@consecutivo INT)
RETURNS NVARCHAR(200)
AS
BEGIN 
	DECLARE @UltimaConexion NVARCHAR(200)
			, @MaxConexion DATETIME
	
	SET @MaxConexion = (SELECT MAX(scs.Fecha_Inicio_Sesion) FROM sys_Control_Sesiones scs WHERE scs.consecutivo_usuario = @consecutivo)
	
	SET @UltimaConexion = (SELECT TOP 1 CONVERT(NVARCHAR, scs.Fecha_Inicio_Sesion, 103) FROM sys_Control_Sesiones scs WHERE scs.consecutivo_usuario = @consecutivo AND scs.Fecha_Inicio_Sesion = @MaxConexion)
	
	IF @UltimaConexion IS NULL BEGIN
		SET @UltimaConexion = 'NO EXISTEN CONEXIONES'
	END
	
	RETURN @UltimaConexion

END