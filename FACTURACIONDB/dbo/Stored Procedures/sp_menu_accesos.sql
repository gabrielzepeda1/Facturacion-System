CREATE PROCEDURE [dbo].[sp_menu_accesos]
@cod_usuario INT,
@cod_padre INT
AS 
DECLARE @Consulta nvarchar(max), @Filtro nvarchar(20), @ConFiltro BIT

SET @Consulta = '
SELECT 
	smpw.cod_menu, 
	ISNULL(CONVERT(NVARCHAR, smw.cod_padre), '''') AS cod_padre, 
	smw.etiqueta, 
	ISNULL(smw.ruta, '''') AS ruta , 
	ISNULL(smw.ruta_icono, '''') AS Icono, 
	smw.posicion
FROM sys_Menu_Permisos_web smpw
INNER JOIN sys_menu_web smw ON smw.cod_menu = smpw.cod_menu
WHERE
	smw.activo = 1
	AND smw.ocultar_menu = 0 '
	
IF @cod_usuario IS NOT NULL
BEGIN
	IF @ConFiltro = 1 
	BEGIN 
		SET @Filtro = ' AND '	
	END 
	ELSE 
	BEGIN 
		SET @Filtro = ' AND ' 
	END

	SET @Consulta = @Consulta + @Filtro + ' smpw.consecutivo_usuario = ' + CONVERT(NVARCHAR, @cod_usuario)
	SET @ConFiltro = 1
END

IF @cod_padre IS NOT NULL
BEGIN
	IF @ConFiltro = 1 
	BEGIN 
		SET @Filtro = ' AND ' 
	END 
	ELSE 
	BEGIN 
		SET @Filtro = ' AND '	
	END

	SET @Consulta = @Consulta + @Filtro + ' smw.cod_padre = ' + CONVERT(NVARCHAR, @cod_padre)
	SET @ConFiltro = 1
END
	
SET @Consulta = @Consulta + ' ORDER BY smw.cod_padre, smw.posicion '

EXECUTE sp_executesql @Consulta;
PRINT @Consulta; 