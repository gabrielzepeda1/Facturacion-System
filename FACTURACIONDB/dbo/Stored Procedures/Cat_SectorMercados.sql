CREATE PROCEDURE [dbo].[Cat_SectorMercados]
--@cod_siglas int, 
@opcion INT,
@codigo INT,
@descripcion VARCHAR(50),
@codPais INT,
@codusuario int, 
@codusuarioUlt int ,
@BUSQUEDAD VARCHAR(200) 
AS

BEGIN TRANSACTION;
BEGIN TRY

declare @Consulta nvarchar(max)
if @opcion=1 BEGIN 	
	DECLARE @Consecutivo INT
	SET @Consecutivo = (SELECT ISNULL(MAX(k.Cod_sector_mercado)+1,1) FROM sector_mercados k )
	IF EXISTS (SELECT Cod_sector_mercado FROM  sector_mercados  WHERE cod_sector_mercado = @codigo AND cod_pais = @codPais) BEGIN 
	    UPDATE sector_mercados SET descripcion=rtrim(LTRIM(upper(@descripcion))), consecutivo_usuario_ult = @codusuarioUlt 
	                                  WHERE Cod_sector_mercado=@codigo AND cod_pais = @codPais
	 END ELSE BEGIN     
	 	INSERT INTO sector_mercados(cod_sector_mercado,Cod_pais,descripcion,consecutivo_usuario,consecutivo_usuario_ult)	
	 	VALUES (@Consecutivo,@codPais,rtrim(LTRIM(upper(@descripcion))),@codusuario,@codusuarioUlt)
	    --INSERT INTO siglas	VALUES(@Consecutivo,@siglas)
	end
END

if @opcion=2 BEGIN 	
	delete from sector_mercados WHERE Cod_sector_mercado=@codigo	
END

if @opcion=3 BEGIN 	
	--SELECT cod_origen AS codigo,UPPER(descripcion) AS Descripcion,cod_usuario AS usuario,cod_usuario_ult AS UsuarioUltiMod 
	--from Origenes_productos s
	 SET @Consulta = '
	SELECT s.Cod_sector_mercado AS codigo,s.Cod_pais as codPais,UPPER(s.descripcion) AS Descripcion,p.descripcion as pais,
	       s.consecutivo_usuario AS usuario,s.consecutivo_usuario_ult AS UsuarioUltiMod 
	FROM sector_mercados s inner join paises p on s.cod_pais=p.cod_pais'
 IF @BUSQUEDAD='0' BEGIN 
		 SET @Consulta =@Consulta + 'where s.cod_pais= '+ CONVERT(NVARCHAR,@codPais,103) + ' order by s.descripcion'
		 EXECUTE  sp_executesql @Consulta;
	     --PRINT @Consulta;
 END ELSE BEGIN 
 		SET @Consulta =@Consulta + ' where s.cod_pais= '+ CONVERT(NVARCHAR,@codPais,103) + ' and s.descripcion  LIKE ''%' + @BUSQUEDAD + '%''  ' + 'order by s.descripcion'----nom_empleado
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