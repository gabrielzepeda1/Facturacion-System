CREATE PROCEDURE [dbo].[ComboCuentasBanco]
--@cod_siglas int, 
@opcion INT,
@codigo INT,
@codigo_banco INT
AS

BEGIN TRANSACTION;
BEGIN TRY

if @opcion=1 BEGIN ----COMBO CUENTAS DE BANCO 
	SELECT cod_banco_cta AS codBancoCuenta, RTRIM(ltrim(descripcion)) AS Cuenta 
	FROM [dbo].[Bancos_cuenta] WHERE cod_pais = @codigo AND cod_banco = @codigo_banco

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
