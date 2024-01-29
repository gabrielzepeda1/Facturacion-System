CREATE PROCEDURE [dbo].[ProdAbonos]


 
@opcion INT,
@Cod_pais INT, --CODIGO PAIS
@Cod_empresa INT, --CODIGO EMPRESA
@Cod_puesto INT,  --CODIGO PUESTO
@Num_Recibo INT,  --NUMERO RECIBO
@Tipo_Docum VARCHAR(20),  --TIPO DOCUMENTO 
@Num_Docum INT,  --NUMERO DOCUMENTO
@fechadia DATETIME, -- FECHA DIA?
@Interno_Externo BIT, -- INTERNO, EXTERNO
@Cod_cliente INT,  --CODIGO CLIENTE
@Valor_Pend numeric(18,2), --VALOR PENDIENTE
@Valor_Apli numeric(18,2), --VALOR APLICADO
@Saldo numeric(18,2),  --NUEVO SALDO 
@consecutivo_usuario INT, --CONSECUTIVO USUARIO
@Fecha_docum DATETIME --FECHA DOCUMENTO


AS 

BEGIN TRANSACTION;

BEGIN TRY 

--IF @opcion=1 
--BEGIN 







--READ 
--OPCION 3 del procedimiento se encarga de seleccionar los datos que se encuentran en la tabla, filtrados por numero de recibo. 
--Para luego cargarlos en el GridView y mostrar las facturas pendientes por el cliente. 


IF @opcion=3 BEGIN 
	
	SELECT Clientes.nombres, Tipo_Docum, Num_Docum, Fecha_docum, Valor_Pend, Valor_Apli, Saldo

	FROM abonos
	INNER JOIN Clientes ON abonos.Cod_Cliente = Clientes.CodigoCliente
	
	WHERE abonos.Num_Recibo = @Num_Recibo 
	AND abonos.cod_pais=@Cod_pais 
	AND abonos.cod_empresa=@Cod_empresa 
	AND abonos.cod_puesto=@Cod_puesto
	AND CONVERT(VARCHAR(10),fechadia,103)=CONVERT(VARCHAR(10), GETDATE(), 103)

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
