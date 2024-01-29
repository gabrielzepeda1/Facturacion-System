CREATE PROCEDURE [dbo].[Cat_Siglas]
@accion NVARCHAR(20),
@cod_sigla INT,
@siglas VARCHAR(20),
@CodigoUser INT 

AS
BEGIN TRANSACTION;
BEGIN TRY

IF @accion='INSERT'
BEGIN 	
	IF NOT EXISTS (SELECT sigla FROM dbo.Siglas WHERE UPPER(sigla) = UPPER(@siglas))
	BEGIN  
		INSERT INTO siglas 
		VALUES(RTRIM(LTRIM(UPPER(@siglas))), @CodigoUser, @CodigoUser)
	END
END

IF @accion='UPDATE' 
BEGIN 
	UPDATE dbo.Siglas 
		SET sigla = RTRIM(LTRIM(UPPER(@siglas))), consecutivo_usuario_ult = @CodigoUser
		WHERE cod_sigla = @cod_sigla
END

IF @accion='DELETE'
BEGIN 	
	DELETE FROM dbo.Siglas 
	WHERE @cod_sigla = @cod_sigla
END

--IF @accion='SELECT'
--BEGIN 	
	
--	 SET @Consulta = '
--		SELECT UPPER(s.sigla) AS sigla,
--		consecutivo_usuario AS usuario,
--		consecutivo_usuario_ult AS UsuarioUltiMod
--		FROM siglas s'

--	 IF @BUSQUEDAD='0'
--	 BEGIN 
--			 SET @Consulta =@Consulta + ' order by sigla'
--			 EXECUTE  sp_executesql @Consulta;
--			 PRINT @Consulta;
--	 END 
--	 ELSE 
--	 BEGIN 
-- 			SET @Consulta =@Consulta + ' where  sigla  LIKE ''%' + @BUSQUEDAD + '%''  ' + 'order by sigla'----nom_empleado
-- 			--SET @Consulta =@Consulta + ' where MD.Cod_empleado LIKE ''%' + @BUSQUEDAD + '%'' OR e.nom_empleado LIKE ''%' + @BUSQUEDAD + '%'' OR e.ape_empleado LIKE ''%' + @BUSQUEDAD + '%'' OR cd.des_deduccion LIKE ''%' + @BUSQUEDAD + '%'' '
-- 			EXECUTE  sp_executesql @Consulta;
--			PRINT @Consulta;
--	END		 
--END		
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