CREATE PROCEDURE [dbo].[Cat_CalidadesProductos]
--@cod_siglas int, 
@opcion INT,
@codigo INT,
@descripcion VARCHAR(50),
@codusuario int, 
@codusuarioUlt int ,
@BUSQUEDAD VARCHAR(200) 
AS

BEGIN TRANSACTION;
BEGIN TRY

declare @Consulta nvarchar(max)
if @opcion=1 BEGIN 	
	DECLARE @Consecutivo INT
	SET @Consecutivo = (SELECT ISNULL(MAX(k.Cod_calidad)+1,1) FROM Calidades_productos k )
	IF EXISTS (SELECT Cod_calidad FROM  Calidades_productos  WHERE cod_calidad = @codigo) BEGIN 
	    UPDATE Calidades_productos SET descripcion=rtrim(LTRIM(upper(@descripcion))), consecutivo_usuario_ult = @codusuarioUlt 
	                                  WHERE cod_calidad=@codigo
	 END ELSE BEGIN     
	 	INSERT INTO Calidades_productos(cod_Calidad,descripcion,consecutivo_usuario,consecutivo_usuario_ult)	
	 	VALUES (@Consecutivo,rtrim(LTRIM(upper(@descripcion))),@codusuario,@codusuarioUlt)
	    --INSERT INTO siglas	VALUES(@Consecutivo,@siglas)
	end
END

if @opcion=2 BEGIN 	
	delete from Calidades_productos WHERE cod_Calidad=@codigo	
END

if @opcion=3 BEGIN 	
	--SELECT cod_origen AS codigo,UPPER(descripcion) AS Descripcion,cod_usuario AS usuario,cod_usuario_ult AS UsuarioUltiMod 
	--from Origenes_productos s
	 SET @Consulta = '
	SELECT cod_Calidad AS codigo,UPPER(descripcion) AS Descripcion,consecutivo_usuario AS usuario,consecutivo_usuario_ult AS UsuarioUltiMod 
	FROM Calidades_productos s'
 IF @BUSQUEDAD='0' BEGIN 
		 SET @Consulta =@Consulta + ' order by descripcion'
		 EXECUTE  sp_executesql @Consulta;
	     --PRINT @Consulta;
 END ELSE BEGIN 
 		SET @Consulta =@Consulta + ' where  descripcion  LIKE ''%' + @BUSQUEDAD + '%''  ' + 'order by descripcion'----nom_empleado
 		--SET @Consulta =@Consulta + ' where MD.Cod_empleado LIKE ''%' + @BUSQUEDAD + '%'' OR e.nom_empleado LIKE ''%' + @BUSQUEDAD + '%'' OR e.ape_empleado LIKE ''%' + @BUSQUEDAD + '%'' OR cd.des_deduccion LIKE ''%' + @BUSQUEDAD + '%'' '
 		EXECUTE  sp_executesql @Consulta;
	    PRINT @Consulta;
	    
	   
 END	
end	
 
 if @opcion=4 BEGIN	
    SELECT cod_Calidad AS codigo,UPPER(descripcion) AS Descripcion,
    consecutivo_usuario AS usuario,consecutivo_usuario_ult AS UsuarioUltiMod 
	FROM Calidades_productos s WHERE s.cod_calidad=@codigo
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