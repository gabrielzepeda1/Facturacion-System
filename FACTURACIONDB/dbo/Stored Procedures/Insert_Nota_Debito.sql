CREATE PROCEDURE [dbo].[Insert_Nota_Debito]
	@Valor_Neto NUMERIC(18,2),
	@Cod_Cliente CHAR(4),
	@Cod_Puesto CHAR(3),
	@CodigoUser CHAR(10),
	@Fecha_Canc DATETIME, 
	@Concepto CHAR	(200), 
	@Interno_Externo CHAR(1), 
	@Cod_Empresa CHAR(3),
	@Codigo_Pais CHAR(3)

AS

BEGIN TRANSACTION; 
	BEGIN TRY 

--INSERTAR NOTA DE DEBITO EN LA TABLA C_Not_De 

		IF @Valor_Neto IS NULL OR @Cod_Cliente IS NULL OR @Cod_Puesto IS NULL OR @CodigoUser IS NULL
			BEGIN 
				RAISERROR('Input parameters cannot be NULL', 16, 1);
				RETURN; 
			END
			

DECLARE @Fecha DATETIME = GETDATE()
DECLARE @Nombre_Cliente VARCHAR(30) = (SELECT Nombres FROM Clientes WHERE CodigoCliente = @Cod_Cliente) + (' ') + (SELECT Apellidos FROM Clientes WHERE CodigoCliente = @Cod_Cliente)


If EXISTS ( 
	SELECT 1 
	FROM Paridad 
	WHERE CONVERT(DATE, Fecha) = CONVERT(DATE, @Fecha))

	BEGIN

DECLARE @Paridad NUMERIC(18,4) = (SELECT Paridad 
			FROM Paridad 
			WHERE CONVERT(DATE, Fecha) = CONVERT(DATE, @Fecha)
			AND cod_pais = @Codigo_Pais)


If @Cod_Puesto IS NOT NULL 
	BEGIN 

INSERT INTO dbo.C_Not_De ( 	
Fecha,	
Valor_Neto,
Cod_Cliente,	
Nombre_Cliente,		
Paridad,	
Cod_Puesto,	
CodigoUser,
Fecha_Canc,	
Concepto,	
Interno_Externo,	
Cod_Empresa)

	VALUES (@Fecha, 
	@Valor_Neto, 
	@Cod_Cliente, 
	@Nombre_Cliente,
	@Paridad, 
		@Cod_Puesto, 
	@CodigoUser, 
	@Fecha_Canc, 
	@Concepto, 
	@Interno_Externo,
	@Cod_Empresa) 


	UPDATE dbo.Puestos

	SET no_nota_debito = no_nota_debito + 1 
	WHERE cod_puesto = @Cod_Puesto 
	AND cod_empresa = @Cod_Empresa 
	AND cod_pais = @Codigo_Pais
		

END
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