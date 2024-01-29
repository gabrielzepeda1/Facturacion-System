CREATE PROCEDURE [dbo].[sp_val_PermisoUsuario]
@CodigoRol INT,
@pageName NVARCHAR(100)
AS 
BEGIN
	DECLARE @tienePermiso BIT;

	--Si la pagina no está oculta
	 IF NOT EXISTS (SELECT 1 FROM sys_menu_web WHERE pagina = @pageName AND ocultar_menu = 1)
	 BEGIN 
 
	 --Verificar si el rol tiene permiso para acceder a la pagina (@pageName) 
		IF EXISTS (SELECT 1 FROM sys_menu_permisos_roles smpr
					INNER JOIN sys_menu_web smw ON smw.cod_menu = smpr.cod_menu 
					WHERE smw.pagina = @pageName AND smpr.cod_rol = @CodigoRol) 
		BEGIN
			SET @tienePermiso = 1 
		END
		ELSE 
		BEGIN
			SET @tienePermiso = 0 
		END
	END
	ELSE 
	BEGIN

	 SET @tienePermiso = 0; -- EL rol no tiene permiso si la pagina está oculta.
	END 

	SELECT @tienePermiso as tienePermiso
END