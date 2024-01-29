CREATE PROCEDURE [dbo].[sp_usuario]
@consecutivo_usuario INT,
@cuenta NVARCHAR(30),
@contrasenia NVARCHAR(3000),
/* DATOS DE LA PERSONA */
@nombre NVARCHAR(50),
@apellido NVARCHAR(50),
@cedula NVARCHAR(20),
@correo NVARCHAR(100),
@telefono NVARCHAR(20),
@direccion NVARCHAR(500),
@cod_rol NVARCHAR(20), 
@tipo NVARCHAR(30)

AS 
BEGIN TRANSACTION;
BEGIN TRY
	DECLARE @cod_persona INT
	DECLARE @activo BIT 
	DECLARE @fecha_registro DATETIME = GETDATE() 
	
	IF @tipo = 'NUEVO' BEGIN
		
		IF NOT EXISTS (SELECT su.usuario FROM sys_usuario su WHERE su.usuario = @cuenta) BEGIN

			SET @consecutivo_usuario = (SELECT ISNULL(MAX(su.consecutivo_usuario)+1, 1) FROM sys_usuario su)
			SET @activo = 1
			
			INSERT INTO sys_usuario(
			usuario,
			contrasenia,
			nombre, --added 
			apellido, --added
			cedula_ruc, --added
			email, --added
			telefono, --added
			direccion,--added
			cod_rol,
			activo, 
			Fecha_Registro)

			VALUES(	
			@cuenta,
			@contrasenia,
			@nombre, 
			@apellido, 
			@cedula, 
			@correo, 
			@telefono, 
			@direccion, 
			@cod_rol,
			@activo,
			@fecha_registro)
				
			--SET @cod_persona = (SELECT ISNULL(MAX(sup.cod_persona)+1, 1) FROM sys_usuario_persona sup)
				
			--INSERT INTO sys_usuario_persona
			--VALUES
			--(
			--	@cod_persona,
			--	@consecutivo_usuario,
			--	@cedula,
			--	@nombre,
			--	@apellido,
			--	@telefono,
			--	@correo,
			--	@direccion,
			--	1,
			--	GETDATE(),
			--	NULL
			--)
			
		END /* PREGUNTA SI EXISTE EL NOMBRE DE USUARIO */
		
	END /* NUEVO */
	
	IF @tipo = 'ACTUALIZAR' BEGIN

	UPDATE sys_usuario 
	SET 
		nombre = @nombre, 
		apellido = @apellido, 
		cedula_ruc = @cedula, 
		email = @correo, 
		telefono = @telefono, 
		direccion = @direccion, 
		cod_rol = @cod_rol
		
	WHERE consecutivo_usuario = @consecutivo_usuario
		
		--SET @cod_persona = (SELECT sup.cod_persona 
		--					FROM sys_usuario_persona sup 
		--					WHERE sup.consecutivo_usuario = @consecutivo_usuario 
		--					AND sup.Activo = 1)
		
		--UPDATE sys_usuario_persona

		--SET
		--    Telefono = @telefono,
		--    Correo = @correo,
		--    Direccion = @direccion

		--WHERE cod_persona = @cod_persona
		
	END /* ACTUALIZAR */
	
	IF @tipo = 'CHANGE PASSWORD' BEGIN
		
		UPDATE sys_usuario 
		
		SET contrasenia = @contrasenia 
		WHERE consecutivo_usuario = @consecutivo_usuario 
		AND activo = 1
		
	END /* CAMBIAR PASSWORD */
	
	
	IF @tipo = 'BAJA' BEGIN
		
		UPDATE sys_usuario 
		SET activo = 0, 
		Fecha_Inactivo = GETDATE() 

		WHERE consecutivo_usuario = @consecutivo_usuario
		
		--UPDATE sys_usuario_persona
		--SET
		--    Activo = 0,
		--    Fecha_Salida = GETDATE()
		--WHERE 6
		--	consecutivo_usuario = @consecutivo_usuario
		--	AND Activo = 1
		
	END /* BAJA DE USUARIO */
	
	IF @tipo = 'CONSULTA' BEGIN
		
		SELECT 
			u.consecutivo_usuario AS CODIGO,
			u.usuario AS USUARIO,
			(u.nombre + ' ' + u.apellido) AS NOMBRE,
			u.telefono AS TELEFONO,
			u.email AS CORREO,
			r.descripcion AS ROL,
			CASE WHEN u.activo = 1 THEN 'SI' ELSE 'BAJA EL ' + ISNULL(CONVERT(NVARCHAR, u.Fecha_Inactivo, 103), '') END AS ACTIVO,
			CONVERT(NVARCHAR, u.Fecha_Registro, 103) AS FECHA,
			dbo.ftn_GetUltimaConexion(u.consecutivo_usuario) AS ULTIMA_CONEXION
		FROM sys_usuario AS u
		INNER JOIN sys_roles_usuarios AS r ON r.cod_rol = u.cod_rol
		--INNER JOIN sys_usuario_persona up ON up.consecutivo_usuario = su.consecutivo_usuario
		--WHERE u.activo = 1 
		ORDER BY
			activo DESC,
			usuario
		
	END /* CONSULTA */

	IF @tipo = 'READ' BEGIN
		
		SELECT 
			u.consecutivo_usuario AS CODIGO, 
			u.usuario AS USUARIO,
			u.cedula_ruc,
			(nombre + ' ' + apellido) AS NOMBRE,
			u.telefono AS TELEFONO,
			u.email AS CORREO,
			u.direccion AS DIRECCION,
			r.descripcion AS ROL,
			CONVERT(NVARCHAR, u.Fecha_Registro, 103) AS FECHA,
			dbo.ftn_GetUltimaConexion(u.consecutivo_usuario) AS ULTIMA_CONEXION,
			CASE WHEN u.activo = 1 THEN 'SI' ELSE 'BAJA EL ' + ISNULL(CONVERT(NVARCHAR, u.Fecha_Inactivo, 103), '') END AS ACTIVO
		FROM sys_usuario AS u
		INNER JOIN sys_roles_usuarios AS r ON r.cod_rol = u.cod_rol
		WHERE
			u.consecutivo_usuario = @consecutivo_usuario
		
	END /* CONSULTA */
	
	IF @tipo = 'COMBOBOX' BEGIN
		
		SELECT 
			consecutivo_usuario AS Codigo,
			usuario AS NombreUsuario,
			UPPER(usuario + ' - ' + nombre + ' ' + apellido) AS Nombre
		FROM sys_usuario
		WHERE
			activo = 1
		ORDER BY
			usuario, 
			Nombre
		
	END
	
END TRY
BEGIN CATCH
    SELECT 
        ERROR_NUMBER() AS ErrorNumber,
        ERROR_SEVERITY() AS ErrorSeverity,
        ERROR_STATE() as ErrorState,
        ERROR_PROCEDURE() as ErrorProcedure,
        ERROR_LINE() as ErrorLine,
        ERROR_MESSAGE() as ErrorMessage;
    IF @@TRANCOUNT > 0
        ROLLBACK TRANSACTION;
END CATCH;
IF @@TRANCOUNT > 0
    COMMIT TRANSACTION;