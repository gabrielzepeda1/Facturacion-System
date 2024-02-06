CREATE PROCEDURE [dbo].[Cat_ComisVendedores]
--@cod_siglas int, 
@opcion INT,
@codigoPais INT,
@codigoEmp INT,
@codigoPuesto INT,
@codigoFamilia INT,
@codigoVendedor INT,
@comision NUMERIC(18,2), 
@Activo BIT, 
@BUSQUEDAD VARCHAR(200)
 
AS

BEGIN TRANSACTION;
BEGIN TRY

declare @Consulta nvarchar(max)
if @opcion=1 BEGIN 	
    IF EXISTS (SELECT Cod_vendedor FROM comis_VendFamilia  WHERE cod_vendedor = @codigoVendedor
                            AND cod_pais=@codigoPais AND cod_empresa=@codigoEmp AND 
                            cod_puesto=@codigoPuesto AND cod_familia=@codigoFamilia) BEGIN 
	    UPDATE comis_VendFamilia SET comision=@comision,activo =@Activo 
	                             WHERE cod_pais=@codigoPais and cod_empresa=@codigoEmp and cod_puesto =@codigoPuesto
	                                   and cod_Vendedor=@codigoVendedor
	 END ELSE BEGIN  
	 	INSERT INTO comis_VendFamilia(cod_pais,cod_empresa,cod_puesto,cod_familia,cod_vendedor,comision,activo)
	 	                     VALUES (@codigoPais,@codigoEmp,@codigoPuesto,@codigoFamilia,@codigoVendedor,@comision,@Activo)
	 	        

	end
END
if @opcion=2 BEGIN 	
	delete from comis_VendFamilia WHERE cod_vendedor = @codigoVendedor
                            AND cod_pais=@codigoPais AND cod_empresa=@codigoEmp AND 
                            cod_puesto=@codigoPuesto AND cod_familia=@codigoFamilia
                            	
END

if @opcion=3 BEGIN 	
	 SET @Consulta = '
	SELECT cf.cod_vendedor as Codigo,rtrim(ltrim(UPPER(nombres)))  +'' ''+ rtrim(ltrim(UPPER(Apellidos))) AS Vendedor,
	       p.descripcion as Pais,e.descripcion as Empresa,pu.descripcion as Puesto,f.descripcion as Familia,
	       cf.Comision as comision,CASE WHEN activo = 1 THEN ''Si'' ELSE ''No'' END AS Activo,
	       v.Cod_pais AS Cod_pais,v.cod_empresa,v.Cod_puesto,f.cod_familia
	FROM comis_VendFamilia cf inner join vendedores v on cf.cod_vendedor=v.cod_vendedor 
	     inner join Paises p on cf.cod_pais=p.cod_pais inner join 
	     Empresas e on cf.cod_empresa=e.cod_empresa inner join Puestos pu
	     on pu.cod_puesto=cf.cod_puesto inner join familias_productos f on f.cod_familia=cf.cod_familia  '
 IF @BUSQUEDAD='0' BEGIN 
		 SET @Consulta =@Consulta + ' where  cf.cod_pais= '+ CONVERT(NVARCHAR,@codigoPais,103)+' order by v.descripcion'
		 EXECUTE  sp_executesql @Consulta;
	     PRINT @Consulta;
 END ELSE BEGIN 
 		SET @Consulta =@Consulta + ' where cf.cod_pais= '+ CONVERT(NVARCHAR,@codigoPais,103)+' and  nombres  LIKE ''%' + @BUSQUEDAD + '%''  ' + 'order by nombres'----nom_empleado
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