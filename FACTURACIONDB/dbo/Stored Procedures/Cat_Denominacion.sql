CREATE PROCEDURE [dbo].[Cat_Denominacion]
--@cod_siglas int, 
@opcion INT,
@codigoDenominacion INT,
@codigoPais INT,
@codigoMoneda INT,
@denominacion INT,
@descripcion VARCHAR(50),
@BUSQUEDAD VARCHAR(200) 
AS

BEGIN TRANSACTION;
BEGIN TRY

declare @Consulta nvarchar(max)
if @opcion=1 BEGIN 	
	DECLARE @Consecutivo INT
	SET @Consecutivo = (SELECT ISNULL(MAX(k.cod_denominacion)+1,1) FROM Denominaciones k )
	IF EXISTS (SELECT cod_denominacion FROM Denominaciones WHERE cod_denominacion = @codigoDenominacion) BEGIN 
	    UPDATE Denominaciones SET cod_pais=@codigoPais,cod_moneda=@codigoMoneda,denominacion =@denominacion,
	                       descripcion = (LTRIM(upper(@descripcion)))
	                       WHERE cod_denominacion=@codigoDenominacion
	 END ELSE BEGIN  
	 	INSERT INTO Denominaciones(cod_denominacion,cod_pais,cod_moneda,denominacion,descripcion)
	 	 VALUES (@codigoDenominacion,@codigoPais,@codigoMoneda,@denominacion,rtrim(LTRIM(upper(@descripcion))))
	end
END
if @opcion=2 BEGIN 	
	delete from Denominaciones WHERE cod_denominacion=@codigoDenominacion	
END

if @opcion=3 BEGIN 	
	--SELECT cod_origen AS codigo,UPPER(descripcion) AS Descripcion,cod_usuario AS usuario,cod_usuario_ult AS UsuarioUltiMod 
	--from Origenes_productos s and pu.cod_pais=v.cod_pais
	 SET @Consulta = '
	SELECT cod_denominacion as Codigo,UPPER(v.descripcion) AS descripcion,denominacion,
	       p.descripcion as Pais,e.descripcion as moneda,v.Cod_pais AS Cod_pais,v.cod_moneda
	FROM Denominaciones v inner join Paises p on v.cod_pais=p.cod_pais inner join Monedas e on v.cod_moneda=e.cod_moneda
	       '
 IF @BUSQUEDAD='0' BEGIN 
		 SET @Consulta =@Consulta + ' where v.cod_pais= '+ CONVERT(NVARCHAR,@codigoPais,103) + ' order by v.descripcion'
		 EXECUTE  sp_executesql @Consulta;
	     PRINT @Consulta;
 END ELSE BEGIN 
 		SET @Consulta =@Consulta + ' where v.cod_pais= '+ CONVERT(NVARCHAR,@codigoPais,103) + ' and  v.descripcion  LIKE ''%' + @BUSQUEDAD + '%''  ' + 'order by v.descripcion'----nom_empleado
 		--SET @Consulta =@Consulta + ' where MD.Cod_empleado LIKE ''%' + @BUSQUEDAD + '%'' OR e.nom_empleado LIKE ''%' + @BUSQUEDAD + '%'' OR e.ape_empleado LIKE ''%' + @BUSQUEDAD + '%'' OR cd.des_deduccion LIKE ''%' + @BUSQUEDAD + '%'' '
 	    EXECUTE  sp_executesql @Consulta;
	    PRINT @Consulta;
	    
	   
 END
END
if @opcion=4 BEGIN 	
	SELECT cod_denominacion,v.cod_pais, v.cod_moneda, v.denominacion, v.descripcion
	 from Denominaciones v WHERE cod_denominacion=@codigoDenominacion	
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