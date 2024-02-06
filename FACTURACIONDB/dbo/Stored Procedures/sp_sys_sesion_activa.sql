CREATE PROCEDURE [dbo].[sp_sys_sesion_activa]
@cod_usuario INT,
@cod_sesion INT,
@estado NVARCHAR(30)
AS 
BEGIN 

	IF @estado = 'ACTIVAR' 
	BEGIN
		INSERT INTO sys_sesion_activa (consecutivo_usuario, cod_sesion, Fecha, activo)
		VALUES (@cod_usuario, @cod_sesion, GETDATE(), 1)   
	END

	IF @estado = 'INACTIVAR' 
	BEGIN
		UPDATE sys_sesion_activa
		SET activo = 0
		WHERE consecutivo_usuario = @cod_usuario
		AND cod_sesion = @cod_sesion
	END

	IF @estado = 'CERRAR'
	BEGIN 

		UPDATE sys_Control_Sesiones 
		SET Fecha_Final_Sesion = GETDATE() 
		WHERE cod_sesion = @cod_sesion 

		DELETE FROM sys_sesion_activa
		WHERE consecutivo_usuario = @cod_usuario 
		AND cod_sesion = @cod_sesion
	END 

	IF @estado = 'REFRESH' 
	BEGIN 
		UPDATE sys_sesion_activa 
		SET activo = 1 
		WHERE consecutivo_usuario = @cod_usuario  
		AND cod_sesion = @cod_sesion
	END		

	IF @estado = 'CONSULTAR' 
	BEGIN 
    	DECLARE @result BIT 
		IF EXISTS (SELECT 1 FROM sys_sesion_activa WHERE consecutivo_usuario = @cod_usuario AND activo = 1) 
		BEGIN 
			SET @result = 1; 
		END 
		ELSE 
		BEGIN 
			SET @result = 0; 
		END 
        SELECT @result as Result
	END 
	
END -- FINAL
