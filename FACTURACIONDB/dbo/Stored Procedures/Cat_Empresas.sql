CREATE PROCEDURE [dbo].[Cat_Empresas]
--@cod_siglas int, 
@opcion INT,
@codPais INT,
@codigo INT,
@descripcion VARCHAR(50),
@codusuario int, 
@codusuarioUlt int ,
@NombImtos VARCHAR(50) ,
@PorcImtos int ,
@DescripCorta VARCHAR(15) ,
@CeduRuc VARCHAR(30) ,
@Direccion VARCHAR(50) ,
@AutoMifin VARCHAR(50) ,
@BUSQUEDAD VARCHAR(200)
 
AS
---SELECT * FROM [dbo].[empresas] 
BEGIN TRANSACTION;
BEGIN TRY
--SELECT * FROM [dbo].[Empleados]
declare @Consulta nvarchar(max)
if @opcion=1 BEGIN 	
	DECLARE @Consecutivo INT
	SET @Consecutivo = (SELECT ISNULL(MAX(k.Cod_empresa)+1,1) FROM empresas k )
	IF EXISTS (SELECT Cod_empresa FROM  empresas  WHERE cod_empresa = @codigo) BEGIN 
	    UPDATE empresas SET descripcion=rtrim(LTRIM(upper(@descripcion))),cod_pais=@codPais,nom_impuesto =rtrim(LTRIM(upper(@NombImtos))),
	                        porc_impuesto = @PorcImtos,consecutivo_usuario_ult = @codusuarioUlt,descripcion_corta =rtrim(LTRIM(upper(@DescripCorta))),
	                        cedula_ruc =rtrim(LTRIM(upper(@CeduRuc))),direccion =rtrim(LTRIM(upper(@Direccion))),autorizacion_mifin =rtrim(LTRIM(upper(@AutoMifin))) 
	                      WHERE cod_empresa=@codigo
	 END ELSE BEGIN     
	 	INSERT INTO empresas(cod_PAIS,COD_EMPRESA,descripcion,consecutivo_usuario,consecutivo_usuario_ult,nom_impuesto,
	 	            porc_impuesto,descripcion_corta, cedula_ruc, direccion,autorizacion_mifin)	
	 	VALUES (@codPais,@Consecutivo,rtrim(LTRIM(upper(@descripcion))),@codusuario,@codusuarioUlt,rtrim(LTRIM(upper(@NombImtos))),
	 	        @PorcImtos,rtrim(LTRIM(upper(@DescripCorta))),rtrim(LTRIM(upper(@CeduRuc))),rtrim(LTRIM(upper(@Direccion))),
	 	        rtrim(LTRIM(upper(@AutoMifin))))
	    --INSERT INTO siglas	VALUES(@Consecutivo,@siglas)
	end
END

if @opcion=2 BEGIN 	
	delete from empresas WHERE COD_EMPRESA=@codigo	
END

if @opcion=3 BEGIN 	
	--SELECT cod_origen AS codigo,UPPER(descripcion) AS Descripcion,cod_usuario AS usuario,cod_usuario_ult AS UsuarioUltiMod 
	--from Origenes_productos s
	 SET @Consulta = '
	SELECT cod_EMPRESA AS codigo,UPPER(s.descripcion) AS Empresa,s.COD_PAIS,P.DESCRIPCION AS Pais,nom_impuesto as Impuesto,
	        porc_impuesto,
	       s.descripcion_corta,cedula_ruc,s.direccion,s.autorizacion_mifin,s.consecutivo_usuario AS usuario,s.consecutivo_usuario_ult AS UsuarioUltiMod 
	FROM empresas s INNER JOIN PAISES P ON S.COD_PAIS=P.COD_PAIS '
 IF @BUSQUEDAD='0' BEGIN 
		 SET @Consulta =@Consulta + ' where s.cod_pais= '+ CONVERT(NVARCHAR,@codPais,103) + ' order by s.descripcion'
		 EXECUTE  sp_executesql @Consulta;
	     --PRINT @Consulta;
 END ELSE BEGIN 
 		SET @Consulta =@Consulta + ' where s.cod_pais= '+ CONVERT(NVARCHAR,@codPais,103) + ' and s.descripcion  LIKE ''%' + @BUSQUEDAD + '%''  ' + 'order by s.descripcion'----nom_empleado
 		--SET @Consulta =@Consulta + ' where MD.Cod_empleado LIKE ''%' + @BUSQUEDAD + '%'' OR e.nom_empleado LIKE ''%' + @BUSQUEDAD + '%'' OR e.ape_empleado LIKE ''%' + @BUSQUEDAD + '%'' OR cd.des_deduccion LIKE ''%' + @BUSQUEDAD + '%'' '
 		EXECUTE  sp_executesql @Consulta;
	    PRINT @Consulta;
	    
	   
 END	
end	
if @opcion=4 BEGIN 	
	SELECT v.cod_pais, v.cod_empresa, descripcion, v.nom_impuesto, v.porc_impuesto,descripcion_corta,cedula_ruc,
	       v.direccion,autorizacion_mifin
	 from Empresas v WHERE COD_EMPRESA=@codigo	
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