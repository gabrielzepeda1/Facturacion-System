CREATE PROCEDURE [dbo].[Cat_Vendedores]
--@cod_siglas int, 
@opcion INT,
@codigoPais INT,
@codigoEmp INT,
@codigoPuesto INT,
@nombre VARCHAR(30),
@Apellido VARCHAR(30),
@codusuario int, 
@codusuarioUlt int ,
@codigoVendedor INT,
@ctaContable VARCHAR(25),
@BUSQUEDAD VARCHAR(200) 
AS

BEGIN TRANSACTION;
BEGIN TRY

declare @Consulta nvarchar(max)
if @opcion=1 BEGIN 	
	DECLARE @Consecutivo INT
	SET @Consecutivo = (SELECT ISNULL(MAX(k.Cod_vendedor)+1,1) FROM Vendedores  k )
	IF EXISTS (SELECT Cod_vendedor FROM Vendedores  WHERE cod_vendedor = @codigoVendedor) BEGIN 
	    UPDATE Vendedores SET cod_pais=@codigoPais,cod_empresa=@codigoEmp,cod_puesto =@codigoPuesto,
	                       nombres = (LTRIM(upper(@nombre))),
	                       apellidos = (LTRIM(upper(@Apellido))),
	                       consecutivo_usuario_ult = (LTRIM(upper(@codusuarioUlt))),
	                       cta_contable = (LTRIM(upper(@ctaContable))) 
	                      WHERE cod_Vendedor=@codigoVendedor
	 END ELSE BEGIN  
	 	INSERT INTO Vendedores(cod_pais,cod_empresa,cod_puesto,nombres,apellidos,consecutivo_usuario,
	 	                       consecutivo_usuario_ult,cod_vendedor,cta_contable)	
	 	VALUES (@codigoPais,@codigoEmp,@codigoPuesto,rtrim(LTRIM(upper(@nombre))),
	 	        rtrim(LTRIM(upper(@Apellido))),rtrim(LTRIM(upper(@codusuario))),
	 	        rtrim(LTRIM(upper(@codusuarioUlt))),@codigoVendedor,ltrim(rtrim(@ctaContable)))

	end
END
if @opcion=2 BEGIN 	
	delete from Vendedores WHERE cod_vendedor=@codigoVendedor	
END

if @opcion=3 BEGIN 	
	--SELECT cod_origen AS codigo,UPPER(descripcion) AS Descripcion,cod_usuario AS usuario,cod_usuario_ult AS UsuarioUltiMod 
	--from Origenes_productos s and pu.cod_pais=v.cod_pais
	 SET @Consulta = '
	SELECT cod_vendedor as Codigo,UPPER(nombres) AS Nombres,UPPER(Apellidos) AS Apellidos,
	p.descripcion as Pais,e.descripcion as Empresa,pu.descripcion as Puesto,cta_contable as Cta_Contable,
	v.Cod_pais AS Cod_pais,v.cod_empresa,v.Cod_puesto,
	v.consecutivo_usuario AS usuario,v.consecutivo_usuario_ult AS UsuarioUltiMod
	FROM vendedores v inner join Paises p on v.cod_pais=p.cod_pais inner join Empresas e on v.cod_empresa=e.cod_empresa
	     inner join Puestos pu on pu.cod_puesto=v.cod_puesto   '
 IF @BUSQUEDAD='0' BEGIN 
		 SET @Consulta =@Consulta + 'where v.cod_pais= '+ CONVERT(NVARCHAR,@codigoPais,103) + ' order by v.descripcion'
		 EXECUTE  sp_executesql @Consulta;
	     PRINT @Consulta;
 END ELSE BEGIN 
 		SET @Consulta =@Consulta + ' where v.cod_pais= '+ CONVERT(NVARCHAR,@codigoPais,103) + ' and  nombres  LIKE ''%' + @BUSQUEDAD + '%''  ' + 'order by nombres'----nom_empleado
 		--SET @Consulta =@Consulta + ' where MD.Cod_empleado LIKE ''%' + @BUSQUEDAD + '%'' OR e.nom_empleado LIKE ''%' + @BUSQUEDAD + '%'' OR e.ape_empleado LIKE ''%' + @BUSQUEDAD + '%'' OR cd.des_deduccion LIKE ''%' + @BUSQUEDAD + '%'' '
 	    EXECUTE  sp_executesql @Consulta;
	    PRINT @Consulta;
	    
	   
 END
END
if @opcion=4 BEGIN 	
	SELECT v.cod_pais, v.cod_empresa, v.cod_puesto, v.nombres, v.apellidos,
	       v.cod_vendedor, v.cta_contable, v.consecutivo_usuario,
	       v.consecutivo_usuario_ult
	  from Vendedores v WHERE cod_vendedor=@codigoVendedor	
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