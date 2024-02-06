CREATE PROCEDURE [dbo].[Cat_monedas]
--@cod_siglas int, 
@opcion INT,
@codigoPais INT,
@codigoMoneda INT,
@simboloMoneda VARCHAR(10),
@descripcion VARCHAR(50),
@BUSQUEDAD VARCHAR(200) 
AS

BEGIN TRANSACTION;
BEGIN TRY

declare @Consulta nvarchar(max)
if @opcion=1 BEGIN 	
	DECLARE @Consecutivo INT
	SET @Consecutivo = (SELECT ISNULL(MAX(k.Cod_moneda)+1,1) FROM monedas k WHERE cod_pais = @codigoPais AND cod_moneda=@codigoMoneda)
	IF EXISTS (SELECT Cod_moneda FROM  monedas  WHERE cod_pais = @codigoPais AND cod_moneda=@codigoMoneda) BEGIN 
	    UPDATE monedas SET descripcion=rtrim(LTRIM(upper(@descripcion))),simbolo_moneda=rtrim(LTRIM(upper(@simboloMoneda)))
	                                  WHERE cod_pais = @codigoPais AND cod_moneda=@codigoMoneda
	 END ELSE BEGIN     
	 	INSERT INTO monedas(cod_pais,descripcion,Cod_moneda,simbolo_moneda)	
	 	VALUES (@codigoPais,rtrim(LTRIM(upper(@descripcion))),@codigoMoneda,rtrim(LTRIM(upper(@simboloMoneda))))
	    --INSERT INTO siglas	VALUES(@Consecutivo,@siglas)
	end
END

if @opcion=2 BEGIN 	
	delete from monedas WHERE cod_pais = @codigoPais AND cod_moneda=@codigoMoneda	
END

if @opcion=3 BEGIN 	
	--SELECT cod_origen AS codigo,UPPER(descripcion) AS Descripcion,cod_usuario AS usuario,cod_usuario_ult AS UsuarioUltiMod 
	--from Origenes_productos s
	 SET @Consulta = '
	SELECT cod_moneda AS codigoMoneda,UPPER(s.descripcion) AS Descripcion,s.cod_pais AS codPais,simbolo_moneda,
	       RTRIM(ltrim(p.Descripcion)) AS Pais 
	FROM Monedas s inner join paises p ON s.cod_pais=p.cod_pais'
 IF @BUSQUEDAD='0' BEGIN 
		 SET @Consulta =@Consulta + 'where s.cod_pais= '+ CONVERT(NVARCHAR,@codigoPais,103) + ' order by s.descripcion'
		 EXECUTE  sp_executesql @Consulta;
	     PRINT @Consulta;
 END ELSE BEGIN 
 		SET @Consulta =@Consulta + ' where s.cod_pais= '+ CONVERT(NVARCHAR,@codigoPais,103) + ' and s.descripcion  LIKE ''%' + @BUSQUEDAD + '%''  ' + 'order by s.descripcion'----nom_empleado
 		--SET @Consulta =@Consulta + ' where MD.Cod_empleado LIKE ''%' + @BUSQUEDAD + '%'' OR e.nom_empleado LIKE ''%' + @BUSQUEDAD + '%'' OR e.ape_empleado LIKE ''%' + @BUSQUEDAD + '%'' OR cd.des_deduccion LIKE ''%' + @BUSQUEDAD + '%'' '
 		EXECUTE  sp_executesql @Consulta;
	    PRINT @Consulta;
	    
	   
 END	
end	
 
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