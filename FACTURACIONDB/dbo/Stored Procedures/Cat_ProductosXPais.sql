CREATE PROCEDURE [dbo].[Cat_ProductosXPais]
--@cod_siglas int, 
@opcion INT,
@codPais INT,
@codigoProducto VARCHAR(6),
@codUndMedida INT,
@descripcion VARCHAR(50),
@descripImpr VARCHAR(25),
@numProducto INT,
@activo bit,
@ExcentoImp BIT,
@codEmpresa INT,
@codusuario int, 
@codusuarioUlt int ,
@BUSQUEDAD VARCHAR(200) 
AS

BEGIN TRANSACTION;
BEGIN TRY

declare @Consulta nvarchar(max),@vcodEmpresa INT
set @vcodEmpresa=convert(int,@codEmpresa)
if @opcion=1 BEGIN 	
	IF EXISTS (SELECT Cod_producto FROM Productos_pais  WHERE  
	                           RTRIM(LTRIM(cod_producto)) = RTRIM(LTRIM(@codigoProducto)) AND cod_pais=@codPais) BEGIN 
	    UPDATE Productos_pais SET descripcion=rtrim(LTRIM(upper(@descripcion))), consecutivo_usuario_ult = @codusuarioUlt, 
                                      cod_und_medida=@codUndMedida,
                                      desc_imprimir=@descripImpr,
                                      activo=@activo,
                                      excento_imp=@ExcentoImp
                                     -- num_producto=@numproducto
	                                  WHERE RTRIM(LTRIM(cod_producto)) = RTRIM(LTRIM(@codigoProducto)) AND cod_pais=@codPais
	                                  
	
	 END ELSE BEGIN 
	 	set @numProducto=(SELECT distinct pm.num_producto
	 	                FROM Productos_Maestro pm WHERE RTRIM(LTRIM(cod_producto)) = RTRIM(LTRIM(@codigoProducto)))    
	 	INSERT INTO Productos_pais(cod_producto,cod_pais,cod_empresa,cod_und_medida,desc_imprimir,descripcion,num_producto,activo,excento_imp,consecutivo_usuario,consecutivo_usuario_ult)	
	 	VALUES (rtrim(LTRIM(upper(@codigoProducto))),@codPais,@codEmpresa,@codUndMedida,@descripImpr,rtrim(LTRIM(upper(@descripcion))),@numproducto,@activo,@ExcentoImp,@codusuario,@codusuarioUlt)
	             
	   
	
	end
END

if @opcion=2 BEGIN 	
	delete from Productos_pais WHERE RTRIM(LTRIM(cod_producto)) = RTRIM(LTRIM(@codigoProducto)) AND cod_pais=@codPais	
END

if @opcion=3 BEGIN 	
	--SELECT cod_origen AS codigo,UPPER(descripcion) AS Descripcion,cod_usuario AS usuario,cod_usuario_ult AS UsuarioUltiMod 
	--from Origenes_productos s
	 SET @Consulta = '
	SELECT Cod_producto AS codigo,UPPER(p.descripcion) AS producto,upper(desc_imprimir) as DescripImpresion,
	       o.descripcion as Pais,num_producto,c.descripcion as UnidadMedida,
	       CASE WHEN excento_imp = 1 THEN ''Si'' ELSE ''No'' END AS excento_imp,
	       CASE WHEN activo = 1 THEN ''Si'' ELSE ''No'' END AS activo,p.cod_pais,p.cod_und_medida,
	p.consecutivo_usuario AS usuario,p.consecutivo_usuario_ult AS UsuarioUltiMod 
	FROM Productos_Pais p INNER JOIN Paises O ON p.cod_pais=O.COD_pais
	     inner join Unidades_Medidas c on c.cod_und_medida=p.cod_und_medida '
 IF @BUSQUEDAD='0' BEGIN 
		 SET @Consulta =@Consulta + 'where p.cod_pais= '+ CONVERT(NVARCHAR,@codPais,103) + ' order by p.descripcion'
		 EXECUTE  sp_executesql @Consulta;
	     --PRINT @Consulta;
 END ELSE BEGIN 
 		SET @Consulta =@Consulta + ' where p.cod_pais= '+ CONVERT(NVARCHAR,@codPais,103) + 'and (p.descripcion  LIKE ''%' + @BUSQUEDAD + '%'' OR cod_producto LIKE ''%' + @BUSQUEDAD + '%'' OR desc_imprimir LIKE ''%' + @BUSQUEDAD + '%'') ' + 'order by p.descripcion'----nom_empleado
 		EXECUTE  sp_executesql @Consulta;
	    PRINT @Consulta;
	    
	   
 END	
end	
if @opcion=4 BEGIN 	
	--SELECT cod_origen AS codigo,UPPER(descripcion) AS Descripcion,cod_usuario AS usuario,cod_usuario_ult AS UsuarioUltiMod 
	--from Origenes_productos s
	 SET @Consulta = '
	SELECT cod_Producto as codigo,RTRIM(ltrim(Descripcion)) AS Producto,num_producto
	FROM Productos_Maestro' 
IF @BUSQUEDAD='0' BEGIN 
		 SET @Consulta =@Consulta + '  WHERE cod_producto NOT IN (SELECT cod_producto FROM Productos_pais p 
		                                                          where cod_empresa=' + convert(varchar,@vcodEmpresa)
		                                                          +') order by descripcion'
		 
		
		 EXECUTE  sp_executesql @Consulta;
	     PRINT @Consulta;
 END ELSE BEGIN 
 		SET @Consulta =@Consulta + ' where  cod_producto NOT IN (SELECT cod_producto FROM Productos_pais p where p.cod_empresa=
 		                                                        '+ convert(varchar,@codEmpresa) + ') and 
 		                                descripcion  LIKE ''%' + @BUSQUEDAD + '%'' OR cod_producto LIKE ''%' + @BUSQUEDAD + '%''  ' + 'order by Descripcion'----nom_empleado
 		
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