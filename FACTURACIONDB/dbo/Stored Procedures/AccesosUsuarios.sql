CREATE PROCEDURE [dbo].[AccesosUsuarios]
@opcion INT,
@codusuario INT,
@codPais INT,
@codEmpresa INT,
@codPuesto INT,
@creaMov bit,
@BUSQUEDAD VARCHAR(200) 

AS

BEGIN TRANSACTION;
BEGIN TRY

DECLARE @Consulta NVARCHAR(MAX)

IF @opcion=1 BEGIN 	
	DECLARE @Consecutivo INT

	IF NOT EXISTS (SELECT * 
					FROM Accesos_Usuarios  
					WHERE consecutivo_usuario=@codusuario 
					AND cod_pais=@codPais 
					AND cod_Empresa=@codEmpresa 
					AND cod_Puesto=@codPuesto)
					
		BEGIN 

		INSERT INTO 
		Accesos_Usuarios(
				consecutivo_usuario,
				Cod_pais,
				cod_Empresa,
				cod_Puesto,
				crea_movimientos
				)						
	 	VALUES (
				@codusuario,
				@codPais,
				@codEmpresa,
				@codPuesto,
				@creaMov
				)
	 END	
END

IF @opcion=2 BEGIN --ELIMINAR
	DELETE 
	FROM Accesos_Usuarios
	WHERE consecutivo_usuario=@codusuario 
	AND cod_pais = @codPais 
	AND cod_Empresa = @codEmpresa
	AND cod_Puesto = @codPuesto  	
END

IF @opcion=3 BEGIN 	
	SET @Consulta = '
	SELECT s.consecutivo_usuario AS codigo, 
	rtrim(ltrim(nombre))+'' ''+rtrim(ltrim(apellido)) AS Nombre,
	UPPER(p.descripcion) AS Pais,
	UPPER(e.descripcion) AS Empresa,
	UPPER(t.descripcion) AS Puesto,
	s.Cod_pais AS codigo_pais,
	usuario,
	s.cod_Empresa,
	s.cod_puesto,
	CASE WHEN crea_movimientos = 1 THEN ''Si'' ELSE ''No'' END AS crea_movimientos
	FROM Accesos_Usuarios AS s
	INNER JOIN paises AS p on s.cod_pais = p.cod_pais 
	INNER JOIN Empresas AS e on e.cod_empresa = s.cod_Empresa
	INNER JOIN Puestos AS t on t.cod_puesto = s.cod_puesto 
	INNER JOIN sys_usuario AS su on su.consecutivo_usuario = s.consecutivo_usuario '

	 IF @BUSQUEDAD = '0' BEGIN 
			SET @Consulta = @Consulta + ' ORDER BY su.nombre'
			EXECUTE sp_executesql @Consulta;
			PRINT @Consulta;
	 END 
	 ELSE BEGIN 
 			SET @Consulta = @Consulta + ' WHERE  su.nombre  LIKE ''%' + @BUSQUEDAD + '%''  OR usuario LIKE ''%' + @BUSQUEDAD  + '%''' + ' ORDER BY su.nombre'----nom_empleado
 			--SET @Consulta =@Consulta + ' where MD.Cod_empleado LIKE ''%' + @BUSQUEDAD + '%''      OR e.nom_empleado LIKE ''%' + @BUSQUEDAD + '%'' OR e.ape_empleado LIKE ''%' + @BUSQUEDAD + '%'' OR cd.des_deduccion LIKE ''%' + @BUSQUEDAD + '%'' '
 			EXECUTE  sp_executesql @Consulta;
			PRINT @Consulta;
	END	
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
    
    
--SET DATEFORMAT DMY 
--EXEC AccesosUsuarios @opcion=3,@codusuario =  NULL ,@codPais =  0 ,
--@codEmpresa =  0  ,@codPuesto = 0  ,@BUSQUEDAD = ''  