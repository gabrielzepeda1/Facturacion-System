CREATE PROCEDURE [dbo].[GuardarFomaPago]
@opcion INT,
@codigoPais INT,
@codigoPuesto INT,
@codigoEmpresa int,
@no_factura int,
@Fecha_Factura datetime,
@Cod_ForPago char(1),
@Desc_ForPago varchar(50),
@Cod_Moneda int,
@Contado INT,
@ValorFacturaCor numeric(18, 2),
@ValorFacturaDol numeric(18, 2),
@ValorRecibidoCor numeric(18, 2),
@ValorRecibidoDol numeric(18, 2),
@Vuelto numeric(18, 2),
@Paridad numeric(18, 4),
@Anulada bit,
@ComisionTarjeta numeric(18, 2),
@Cod_tarjeta int 
AS

BEGIN TRANSACTION;
BEGIN TRY
--select CONVERT(VARCHAR(10), GETDATE(), 103)
--select round(123.456, 2, 1)
if @opcion=1 BEGIN 	
   declare @Consecutivo int,@valorCord as numeric(18,2),@valorDol as numeric(18,2),@Moneda int,@FormaPago varchar(2)
   set @Fecha_Factura=CONVERT(VARCHAR(10), GETDATE(), 103)
   set @Consecutivo =(select case when count(*)=0 then 1 else count(*)+1 end as conse  from Hist_DistribucionPago where 
                      cod_pais=@codigoPais and cod_empresa=@codigoEmpresa and cod_puesto=@codigoPuesto and No_Factura=@no_factura
					  and year(Fecha_Factura)=year(getdate()))
   set @Desc_ForPago=(select descripcion from [dbo].[Forma_pago] where cod_formapago=@Cod_ForPago)

		INSERT INTO [dbo].[Hist_DistribucionPago]
                   (cod_pais,cod_empresa,cod_puesto,Consecutivo,No_Factura,Fecha_Factura,Cod_ForPago,Desc_ForPago
                   ,Cod_Moneda,Contado,ValorFacturaCor,ValorFacturaDol,ValorRecibidoCor,ValorRecibidoDol
                    ,Vuelto,Paridad,Anulada,ComisionTarjeta,Cod_tarjeta)
        VALUES     (@codigoPais,@codigoEmpresa,@codigoPuesto,@Consecutivo,@no_factura,@Fecha_Factura,@Cod_ForPago,@Desc_ForPago,
                    @Cod_Moneda,@Contado,@ValorFacturaCor,@ValorFacturaDol,@ValorRecibidoCor,@ValorRecibidoDol,@Vuelto,@Paridad,
                    @Anulada,@ComisionTarjeta,@Cod_tarjeta)  
			
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
