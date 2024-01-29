CREATE PROCEDURE [dbo].[sp_Menu_web_admin]
@cod_menu INT,
@cod_Padre INT,
@Posicion INT,
@Etiqueta NVARCHAR(100),
@Ruta NVARCHAR(500),
@Pagina NVARCHAR(100),
@icono NVARCHAR(200),
@Activo BIT,
@Ocultar_Menu BIT,
@Tipo NVARCHAR(20) /* INSERTAR, FULL, COMBOBOX, READER */
AS
BEGIN TRANSACTION;
BEGIN TRY

	IF @Tipo = 'FULL' 
	BEGIN
		SELECT sm.cod_menu, 
		ISNULL(CONVERT(NVARCHAR, sm.cod_padre),'') AS cod_Padre, 
		sm.Posicion, 
		sm.Etiqueta, 
		ISNULL(sm.Ruta,'') AS Ruta, 
		ISNULL(sm.ruta_icono, '') AS Icono
		FROM sys_menu_web AS sm
		WHERE sm.Activo = 1
		ORDER BY sm.Posicion ASC
	END

	IF @Tipo = 'INSERTAR' BEGIN
		IF @cod_menu IS NULL BEGIN
			
			SET @cod_menu = (SELECT ISNULL(MAX(smw.cod_menu)+1, 1) FROM sys_menu_web smw)
			
			INSERT INTO sys_menu_web
			VALUES
			(
				@cod_menu,
				@cod_Padre,
				@Posicion,
				@Etiqueta,
				@Ruta,
				@Pagina,
				@icono,
				@Ocultar_Menu,
				1 /*{ activo }*/
			)
			
		END 
		ELSE 
		BEGIN
			UPDATE sys_menu_web
			SET
			    cod_padre = @cod_Padre,
			    posicion = @Posicion,
			    etiqueta = @Etiqueta,
			    ruta = @Ruta,
			    pagina = @Pagina, 
			    ruta_icono = @icono,
			    ocultar_menu = @Ocultar_Menu,
			    activo = @Activo
			WHERE cod_menu = @cod_menu
		END
	END

	IF @Tipo = 'ELIMINAR' 
	BEGIN
		UPDATE sys_menu_web
		SET activo = 0
		WHERE cod_menu = @cod_menu
	END

	IF @Tipo = 'COMBOBOX' BEGIN

		CREATE TABLE #Menu
		(
			cod_menu INT NOT NULL,
			cod_padre INT NOT NULL,
			Etiqueta VARCHAR(100) NULL,
			consecutivoPadre INT NULL,
			consecutivo INT NULL,
			padre BIT NULL
		)

		INSERT INTO #Menu
		SELECT 
			sm.cod_menu
			, sm.cod_menu
			, sm.Etiqueta 
			, ROW_NUMBER() OVER(ORDER BY sm.Posicion ASC) AS consecutivo
			, NULL
			, 1
		FROM sys_menu_web sm
		WHERE 
			sm.cod_Padre IS NULL AND sm.Activo = 1
			
		DECLARE @Num_Rows INT, @CC_Rows INT, @idpadre INT, @consecutivoPadre INT 

		SET	@CC_Rows = 1
		SET	@Num_Rows = (SELECT COUNT(cod_menu) FROM #Menu)

		WHILE(@CC_Rows <= @Num_Rows)
		BEGIN
			
			SET @idpadre = (SELECT p.cod_menu FROM #Menu p WHERE p.consecutivoPadre = @CC_Rows)
			
			SET @consecutivoPadre = @CC_Rows
			
			INSERT INTO #Menu
			SELECT 
				sm.cod_menu
				, @idpadre
				, sm.etiqueta 
				, @consecutivoPadre
				, ROW_NUMBER() OVER(ORDER BY sm.Posicion ASC) AS consecutivo
				, 0
			FROM sys_menu_web sm
			WHERE 
				sm.cod_padre = @idpadre
				AND sm.cod_menu <> @idpadre 
				AND sm.Activo = 1 
				AND sm.ocultar_menu = 0
			ORDER BY sm.posicion
			
			SET @CC_Rows = @CC_Rows + 1
		END

		--SELECT @idpadre, @Num_Rows

		SELECT 
			m.cod_menu
			, CASE WHEN m.padre = 1 THEN m.Etiqueta ELSE ' - - ' + m.Etiqueta END AS Etiqueta
		FROM #Menu m
		ORDER BY 
			m.consecutivoPadre 
			, m.consecutivo

	END
	
	IF @Tipo = 'READER' BEGIN
		SELECT 
			sm.cod_menu 
			, ISNULL(CONVERT(NVARCHAR,sm.cod_Padre),'') AS cod_Padre 
			, sm.Posicion
			, sm.Etiqueta
			, ISNULL(sm.Ruta,'') AS Ruta
			, ISNULL(sm.Pagina,'') AS Pagina
			, ISNULL(sm.ruta_icono,'') AS Icono
			, CASE WHEN sm.Activo = 1 THEN 'Si' ELSE 'No' END AS Activo
			, CASE WHEN sm.Ocultar_Menu = 1 THEN 'Si' ELSE 'No' END AS Ocultar_Menu 
		FROM sys_menu_web sm
		WHERE 
			sm.cod_menu = @cod_menu
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