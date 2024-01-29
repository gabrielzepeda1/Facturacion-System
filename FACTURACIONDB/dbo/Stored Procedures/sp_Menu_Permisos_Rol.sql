CREATE PROCEDURE [dbo].[sp_Menu_Permisos_Rol]
@cod_menu INT,
@cod_rol INT,
@Tipo NVARCHAR(20) /* INSERTAR, ACTUALIZAR, ELIMINAR, FULL */
AS
BEGIN TRANSACTION;
BEGIN TRY

	IF @Tipo = 'INSERTAR' 
	BEGIN
		IF NOT EXISTS(
		SELECT smpr.cod_menu 
		FROM sys_menu_permisos_roles smpr 
		WHERE smpr.cod_menu = @cod_menu AND smpr.cod_rol = @cod_rol) 
		
		BEGIN			
			INSERT sys_menu_permisos_roles (cod_menu, cod_rol)
			VALUES (@cod_menu, @cod_rol)
		END	
	END

	IF @Tipo = 'ELIMINAR' 
	BEGIN	
		DELETE FROM sys_menu_permisos_roles 
		WHERE cod_menu = @cod_menu
		AND cod_rol = @cod_rol 
		
		--DELETE FROM sys_menu_permisos_roles WHERE cod_menu = @cod_menu AND cod_rol = @cod_rol
	END

	IF @Tipo = 'FULL' 
	BEGIN	
		SELECT 
			sm.cod_menu, 
			ISNULL(CONVERT(NVARCHAR,sm.cod_Padre),'') AS cod_Padre,
			sm.Posicion,
			sm.Etiqueta,
			ISNULL(sm.Ruta,'') AS Ruta,
			ISNULL(sm.ruta_icono, '') AS Icono
		FROM sys_menu_web sm
		INNER JOIN sys_menu_permisos_roles smpr ON smpr.cod_menu = sm.cod_menu
		WHERE 
			sm.Activo = 1
			AND smpr.cod_rol = @cod_rol
		ORDER BY
			sm.cod_Padre,
			sm.Posicion ASC
	END

	IF @Tipo = 'DISPONIBLES' 
	BEGIN 
		SELECT 
			sm.cod_menu, 
			ISNULL(CONVERT(NVARCHAR, sm.cod_Padre), '') AS cod_Padre,
			sm.Posicion,
			sm.Etiqueta,
			ISNULL(sm.Ruta, '') AS Ruta,
			ISNULL(sm.ruta_icono, '') AS Icono
		FROM sys_menu_web sm 
		WHERE NOT EXISTS (
							SELECT cod_menu
							FROM sys_menu_permisos_roles smpr 
							WHERE sm.cod_menu = smpr.cod_menu
							AND smpr.cod_rol = 1
							AND sm.activo = 1
						) 

		ORDER BY 
		sm.cod_padre
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