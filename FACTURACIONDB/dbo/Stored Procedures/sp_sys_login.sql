
CREATE PROC [dbo].[sp_sys_login] 

@Username NVARCHAR(50),
@Password NVARCHAR(100)

AS
BEGIN 
	SET NOCOUNT ON; 
	DECLARE @CodigoUser INT, @LastLoginDate DATETIME, @CodigoRol INT

	SELECT @CodigoUser = consecutivo_usuario, @LastLoginDate = ultima_conexion, @CodigoRol = cod_rol
	FROM sys_usuario WHERE usuario = @Username AND contrasenia = @Password

	IF @CodigoUser IS NOT NULL  
	BEGIN 
		IF NOT EXISTS (SELECT consecutivo_usuario FROM sys_sesion_activa WHERE consecutivo_usuario = @CodigoUser AND activo = 1)
		BEGIN 
			UPDATE sys_usuario 
			SET ultima_conexion = GETDATE() 
			WHERE consecutivo_usuario = @CodigoUser 
			SELECT @CodigoUser [CodigoUser], 
											(SELECT cod_rol FROM sys_roles_usuarios 
											 WHERE cod_rol = @CodigoRol) [CodigoRol]
											--Usuario es valido 
		END 
		ELSE 
		BEGIN 
			SELECT -2 [CodigoUser], 0 [CodigoRol] -- Usuario ya tiene una sesion activa. 
		END 
	END
	ELSE
	BEGIN 
		SELECT -1 [CodigoUser], 0 [CodigoRol] -- Usuario invalido.
	END 

END
