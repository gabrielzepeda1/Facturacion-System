CREATE FUNCTION [dbo].[ftn_GetMoney](@Valor FLOAT)
RETURNS NVARCHAR(200)
AS
BEGIN 
	/*Se crea el numero consecutivo correspondiente al año*/
	DECLARE @CONVERTIDO NVARCHAR(200)
	SET @CONVERTIDO = (SELECT CAST(CONVERT(varchar, CAST(sum(@Valor) AS money), 1) AS varchar))
    
    RETURN @CONVERTIDO
END