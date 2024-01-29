CREATE PROCEDURE [dbo].[Cat_Bancos]
--@cod_siglas int, 
@opcion INT,
@codigoPais INT,
@codigoBanco INT,
@descripcion VARCHAR(50),
@BUSQUEDAD VARCHAR(200) 
AS

BEGIN TRANSACTION;
BEGIN TRY

declare @Consulta nvarchar(max)
if @opcion=1 BEGIN 	
	DECLARE @Consecutivo INT
	SET @Consecutivo = (SELECT ISNULL(MAX(k.Cod_banco)+1,1) FROM Bancos k WHERE cod_pais = @codigoPais AND cod_banco=@codigoBanco)
	IF EXISTS (SELECT cod_banco FROM  Bancos  WHERE cod_pais = @codigoPais AND cod_banco=@codigoBanco) BEGIN 
	    UPDATE Bancos SET descripcion=rtrim(LTRIM(upper(@descripcion))) 
	                                  WHERE cod_pais = @codigoPais AND cod_banco=@codigoBanco
	 END ELSE BEGIN     
	 	INSERT INTO Bancos(cod_pais,descripcion,Cod_banco)	
	 	VALUES (@codigoPais,rtrim(LTRIM(upper(@descripcion))),@codigoBanco)
	    --INSERT INTO siglas	VALUES(@Consecutivo,@siglas)
	end
END

if @opcion=2 BEGIN 	
	delete from Bancos WHERE cod_pais = @codigoPais AND cod_banco=@codigoBanco	
END

if @opcion=3 BEGIN 	
	--SELECT cod_origen AS codigo,UPPER(descripcion) AS Descripcion,cod_usuario AS usuario,cod_usuario_ult AS UsuarioUltiMod 
	--from Origenes_productos s
	 SET @Consulta = '
	SELECT cod_banco AS codigoBanco,UPPER(s.descripcion) AS Descripcion,s.cod_pais AS codPais,RTRIM(ltrim(p.Descripcion)) AS Pais 
	FROM Bancos s inner join paises p on s.cod_pais=p.cod_pais'
 IF @BUSQUEDAD='0' BEGIN 
		 SET @Consulta =@Consulta + ' where  s.cod_pais= '+ CONVERT(NVARCHAR,@codigoPais,103) + ' order by s.descripcion'
		 EXECUTE  sp_executesql @Consulta;
	     PRINT @Consulta;
 END ELSE BEGIN 
 		SET @Consulta =@Consulta + ' where  s.cod_pais= '+ CONVERT(NVARCHAR,@codigoPais,103) +' and s.descripcion  LIKE ''%' + @BUSQUEDAD + '%''  ' + 'order by s.descripcion'----nom_empleado
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