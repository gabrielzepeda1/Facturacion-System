CREATE PROCEDURE [dbo].[Cat_ProductosMaestro]
--@cod_siglas int, 
@opcion INT,
@codigo VARCHAR(6),
@numproducto INT,
@descripcion VARCHAR(50),
@SIGLA VARCHAR(20),
@codorigen INT,
@codcalidad INT,
@codpresentacion INT,
@codfamilia INT,
@codusuario int, 
@codusuarioUlt int ,
@produccionSm BIT,
@BUSQUEDAD VARCHAR(200) 
AS

--SET DATEFORMAT DMY 
--EXEC Cat_ProductosMaestro @opcion=1,@codigo =  cvs05 ,@numproducto =  85 ,@descripcion =  'colas' ,@SIGLA =  'COLAS' ,
--@codorigen =  '1' ,@codcalidad =  '1' ,@codpresentacion =  '1' ,@codfamilia =  '1' ,@codusuario =  1 ,@codusuarioUlt =  1 ,
--@BUSQUEDAD = '0'  

BEGIN TRANSACTION;
BEGIN TRY

declare @Consulta nvarchar(max)
if @opcion=1 BEGIN 	
	IF EXISTS (SELECT Cod_producto FROM  Productos_maestro  WHERE RTRIM(LTRIM(cod_producto)) = RTRIM(LTRIM(@codigo))) BEGIN 
	    UPDATE Productos_maestro SET descripcion=rtrim(LTRIM(upper(@descripcion))), consecutivo_usuario_ult = @codusuarioUlt, 
                                      SIGLA=@SIGLA,
                                      cod_origen=@codorigen,
                                      cod_calidad=@codcalidad,
                                      cod_presentacion=@codpresentacion,
                                      cod_familia=@codfamilia,
	                                  num_producto=@numproducto,produccion_sm=@produccionSm
	                                  WHERE rtrim(LTRIM(cod_producto))=rtrim(LTRIM(upper(@codigo)))
	                                  
	PRINT 'HELLO'
	 END ELSE BEGIN     
	 	INSERT INTO Productos_maestro(cod_producto,num_producto,descripcion,sigla,cod_origen,cod_calidad,cod_presentacion,
	 	                              cod_familia,consecutivo_usuario,consecutivo_usuario_ult,produccion_sm)	
	 	VALUES (rtrim(LTRIM(upper(@codigo))),@numproducto,rtrim(LTRIM(upper(@descripcion))),@SIGLA,@codorigen,@codcalidad,@codpresentacion,
	 	         @codfamilia,@codusuario,@codusuarioUlt,@produccionSm)
	             
	         
       
	
	end
END

if @opcion=2 BEGIN 	
	delete from Productos_maestro WHERE rtrim(LTRIM(cod_producto))=rtrim(LTRIM(upper(@codigo)))	
END

if @opcion=3 BEGIN 	
	--SELECT cod_origen AS codigo,UPPER(descripcion) AS Descripcion,cod_usuario AS usuario,cod_usuario_ult AS UsuarioUltiMod 
	--from Origenes_productos s
	 SET @Consulta = '
	SELECT Cod_producto AS codigo,num_producto,UPPER(p.descripcion) AS producto,sigla,
	       P.COD_ORIGEN AS CodOrigen,O.DESCRIPCION AS origen,p.cod_calidad as CodCalidad,c.descripcion as calidad,
	       p.cod_presentacion as CodPresentacion,sen.descripcion as presentacion,p.cod_familia as CodFamilia,
	       f.descripcion as familia, CASE WHEN produccion_sm = 1 THEN ''Si'' ELSE ''No'' END AS produccion_sm,
	p.consecutivo_usuario AS usuario,p.consecutivo_usuario_ult AS UsuarioUltiMod 
	FROM Productos_maestro p INNER JOIN Origenes_productos O ON P.COD_ORIGEN=O.COD_ORIGEN
	     inner join Calidades_productos c on c.cod_calidad=p.cod_calidad inner join Presentaciones_productos sen 
	     on sen.cod_presentacion=p.cod_presentacion inner join Familias_productos f on f.cod_familia=p.cod_familia'
 IF @BUSQUEDAD='0' BEGIN 
		 SET @Consulta =@Consulta + ' order by p.descripcion'
		 EXECUTE  sp_executesql @Consulta;
	     --PRINT @Consulta;
 END ELSE BEGIN 
 		SET @Consulta =@Consulta + ' where  p.descripcion  LIKE ''%' + @BUSQUEDAD + '%''  ' + 'order by p.descripcion'----nom_empleado
 		--SET @Consulta =@Consulta + ' where MD.Cod_empleado LIKE ''%' + @BUSQUEDAD + '%'' OR e.nom_empleado LIKE ''%' + @BUSQUEDAD + '%'' OR e.ape_empleado LIKE ''%' + @BUSQUEDAD + '%'' OR cd.des_deduccion LIKE ''%' + @BUSQUEDAD + '%'' '
 		EXECUTE  sp_executesql @Consulta;
	    PRINT @Consulta;
	    
	   
 END	
end	
if @opcion=4 BEGIN 	
	SELECT p.cod_producto, p.num_producto, p.descripcion, p.sigla, p.cod_origen,
	       p.cod_calidad, p.cod_presentacion, p.cod_familia, p.consecutivo_usuario,
	       p.consecutivo_usuario_ult, CASE WHEN produccion_sm = 1 THEN 'Si' ELSE 'No' END AS produccion_sm
	  from Productos_maestro p WHERE rtrim(LTRIM(cod_producto))=rtrim(LTRIM(upper(@codigo)))	
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