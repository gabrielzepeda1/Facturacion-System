create PROCEDURE [dbo].[sp_Menu_Web]
@cod_menu INT,
@cod_usuario INT,
@Tipo NVARCHAR(30)
AS
IF @Tipo = 'MAESTRO' BEGIN
	SELECT 
		sm.cod_menu
		, sm.etiqueta
		, sm.ruta_icono 
	FROM sys_Menu_Permisos_web smp
	INNER JOIN sys_menu_web sm ON sm.cod_menu = smp.cod_menu
	WHERE 
		sm.activo = 1 
		AND sm.cod_padre IS NULL
		AND smp.cod_usuario = @cod_usuario
	ORDER BY sm.posicion
END 
