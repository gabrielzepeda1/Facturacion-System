create PROCEDURE [dbo].[CombosClientes]
--@cod_siglas int, 
@opcion INT,
@codigo INT
AS

BEGIN TRANSACTION;
BEGIN TRY
if @opcion=1 BEGIN 	
	SELECT cod_pais as codPais,Descripcion AS DesPais
	  FROM  Paises 
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