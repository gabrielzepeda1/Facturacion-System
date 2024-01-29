CREATE PROCEDURE [dbo].[Cat_BancosCtas]
--@cod_siglas int, 
@opcion INT,
@codigoBancoCta INT,
@codigoPais INT,
@codigoBanco INT,
@codigoEmpresa INT,
@codigoPuesto INT,
@codigoMoneda INT,
@descripcion VARCHAR(50),
@BUSQUEDAD VARCHAR(200) 
AS

BEGIN TRANSACTION;
BEGIN TRY
        --cod_pais = @codigoPais AND cod_banco=@codigoBanco
	    --AND cod_empresa=@codigoEmpresa AND cod_puesto=@codigoPuesto 
	    --AND cod_moneda=@codigoMoneda
declare @Consulta nvarchar(max)
if @opcion=1 BEGIN 	
	DECLARE @Consecutivo INT
	SET @Consecutivo = (SELECT ISNULL(MAX(k.Cod_banco_cta)+1,1) FROM Bancos_cuenta k WHERE Cod_banco_cta=@codigoBancoCta)
	IF EXISTS (SELECT Cod_banco_cta FROM Bancos_cuenta  WHERE Cod_banco_cta=@codigoBancoCta) BEGIN 
	    UPDATE Bancos_cuenta SET descripcion=rtrim(LTRIM(upper(@descripcion))),cod_pais = @codigoPais,cod_banco=@codigoBanco,
	                             cod_empresa=@codigoEmpresa,cod_puesto=@codigoPuesto,cod_moneda=@codigoMoneda
	                          WHERE Cod_banco_cta=@codigoBancoCta
	                                                                   
	 END ELSE BEGIN                           
	 	INSERT INTO Bancos_cuenta(cod_pais,descripcion,Cod_banco,cod_empresa,cod_banco_cta,cod_puesto,cod_moneda)	
	 	VALUES (@codigoPais,rtrim(LTRIM(upper(@descripcion))),@codigoBanco,@codigoEmpresa,@codigoBancoCta,@codigoPuesto,@codigoMoneda)
	    --INSERT INTO siglas	VALUES(@Consecutivo,@siglas)
	end
END

if @opcion=2 BEGIN 	
	delete from Bancos_cuenta WHERE Cod_banco_cta=@codigoBancoCta	
END

if @opcion=3 BEGIN 	
	--SELECT cod_origen AS codigo,UPPER(descripcion) AS Descripcion,cod_usuario AS usuario,cod_usuario_ult AS UsuarioUltiMod 
	--from Origenes_productos s
	 SET @Consulta = '
	SELECT cod_banco_cta AS codigoCtaBanco,UPPER(s.descripcion) AS Descripcion,
	       s.cod_pais AS codPais,RTRIM(ltrim(p.Descripcion)) AS Pais,
	       s.cod_empresa as codEmpresa,RTRIM(ltrim(e.descripcion)) AS Empresa,
	       s.cod_puesto as codPuesto,RTRIM(ltrim(t.descripcion)) AS Puesto,
	       s.cod_banco as codBanco,RTRIM(ltrim(c.Descripcion)) AS Banco,
	       s.cod_moneda as codMoneda,RTRIM(ltrim(m.Descripcion)) AS Moneda
	FROM Bancos_cuenta s inner join paises p on s.cod_pais=p.cod_pais inner join Empresas e on s.cod_empresa=e.cod_empresa 
	      inner join puestos t on t.cod_puesto=s.cod_puesto inner join bancos c on s.cod_banco=c.cod_banco
	      inner join Monedas m on m.cod_moneda=s.cod_moneda'
 IF @BUSQUEDAD='0' BEGIN 
		 SET @Consulta =@Consulta + ' where  s.cod_pais= '+ CONVERT(NVARCHAR,@codigoPais,103)+' order by s.descripcion'
		 EXECUTE  sp_executesql @Consulta;
	     PRINT @Consulta;
 END ELSE BEGIN 
 		SET @Consulta =@Consulta + ' where s.cod_pais= '+ CONVERT(NVARCHAR,@codigoPais,103) + ' and s.descripcion  LIKE ''%' + @BUSQUEDAD + '%''  ' + 'order by s.descripcion'----nom_empleado
 		--SET @Consulta =@Consulta + ' where MD.Cod_empleado LIKE ''%' + @BUSQUEDAD + '%'' OR e.nom_empleado LIKE ''%' + @BUSQUEDAD + '%'' OR e.ape_empleado LIKE ''%' + @BUSQUEDAD + '%'' OR cd.des_deduccion LIKE ''%' + @BUSQUEDAD + '%'' '
 		EXECUTE  sp_executesql @Consulta;
	    PRINT @Consulta;
	    
	   
 END	
end	
 if @opcion=4 BEGIN 	
	SELECT cod_banco_cta, cod_pais, cod_empresa, cod_puesto, cod_banco, cod_moneda,
	       descripcion
	  from Bancos_cuenta WHERE Cod_banco_cta=@codigoBancoCta	
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