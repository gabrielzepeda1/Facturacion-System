CREATE PROCEDURE [dbo].[sp_Menu_Permisos]
@cod_menu INT,
@cod_usuario INT,
@Tipo NVARCHAR(20) /* INSERTAR, ACTUALIZAR, ELIMINAR, FULL */
AS
BEGIN TRANSACTION;
BEGIN TRY
	IF @Tipo = 'INSERTAR' BEGIN
		
		IF NOT EXISTS(SELECT smp.cod_menu FROM sys_Menu_Permisos_web smp WHERE smp.cod_menu = @cod_menu AND smp.consecutivo_usuario = @cod_usuario) BEGIN
			INSERT INTO sys_Menu_Permisos_web
			VALUES
			(
				@cod_menu,
				@cod_usuario
			)             	
		END
		
	END

	IF @Tipo = 'ELIMINAR' BEGIN
		
		DELETE FROM sys_Menu_Permisos_web 
		WHERE 
			consecutivo_usuario = @cod_usuario
			AND cod_menu IN (SELECT smw.cod_menu FROM sys_menu_web smw WHERE smw.cod_padre = @cod_menu)
		
		DELETE FROM sys_Menu_Permisos_web WHERE cod_menu = @cod_menu AND consecutivo_usuario = @cod_usuario
		
	END

	IF @Tipo = 'FULL' BEGIN
		
		SELECT 
			sm.cod_menu 
			, ISNULL(CONVERT(NVARCHAR,sm.cod_Padre),'') AS cod_Padre 
			, sm.Posicion
			, sm.Etiqueta
			, ISNULL(sm.Ruta,'') AS Ruta
			, ISNULL(sm.ruta_icono, '') AS Icono
		FROM sys_menu_web sm
		INNER JOIN sys_Menu_Permisos_web smp ON smp.cod_menu = sm.cod_menu
		WHERE 
			sm.Activo = 1
			AND smp.consecutivo_usuario = @cod_usuario
		ORDER BY
			sm.cod_Padre
			, sm.Posicion ASC
		
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