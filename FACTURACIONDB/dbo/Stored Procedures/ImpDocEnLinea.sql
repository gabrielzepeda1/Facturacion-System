CREATE PROCEDURE [dbo].[ImpDocEnLinea]

--@cod_siglas int, 
@opcion INT,
@codigoPais INT,
@codigoPuesto INT,
@codigoEmpresa int,
@no_factura int,
@fecha datetime,
@codcliente VARCHAR(max)

AS
--SET DATEFORMAT DMY EXEC ImpDocEnLinea  @opcion = 1,   @codigoPais=1,   @codigoPuesto=1,   @codigoEmpresa=4,  
-- @no_factura='31',   @fecha='21/02/2022',   @codcliente='Factura es de Contado' 
BEGIN TRANSACTION;
BEGIN TRY
  declare @telefono varchar(20),@nomVende varchar(max),@apell varchar(max)
  set @apell=(select apellidos from Vendedores where cod_puesto=@codigoPuesto and cod_empresa=@codigoEmpresa and cod_pais=@codigoPais)
  set @nomVende=(select nombres from Vendedores where cod_puesto=@codigoPuesto and cod_empresa=@codigoEmpresa and cod_pais=@codigoPais)
  set @telefono=(select telefono from puestos where cod_puesto=@codigoPuesto and cod_empresa=@codigoEmpresa and cod_pais=@codigoPais)
if @opcion=1 BEGIN 	 ---impresion Factura
    select  d.no_factura, d.cod_producto, d.desc_imprimir as Producto,d.cantidad as Cantidad,d.bultos as Bultos
	     ,d.valor_iva as Iva,d.precio_unidad as Precio,d.sub_total as SubTotal,d.valor_descuento as Descuento
		  ,d.cod_pais,d.fecha,d.cod_empresa,d.cod_puesto,e.cedula_ruc as cedRucEmpresa,e.autorizacion_mifin,e.direccion
		   ,@telefono as telefono,neto,@codcliente as condicion,rtrim(ltrim(cod_cliente))+' '+rtrim(ltrim(Nombre_comercial)) as Cliente
		   ,d.cedula_ruc as cedRucCliente,Notas,fecha_Cancelacion
		   ,CONVERT(VARCHAR(10),cod_vendedor) +' '+ rtrim(ltrim(@nomVende)) +' '+rtrim(ltrim(@apell)) as vendedor
		   ,case when Contado=1 then LeyendaUno else '' end as LeyendaUno,
		   case when Contado=1 then LeyendaDos else '' end as LeyendaDos, 
		   case when Contado=1 then LeyendaTres else '' end as LeyendaTres, 
		   case when Contado=1 then LeyendaCuatro else '' end as LeyendaCuatro
		   ,convert(char(8), getdate(), 108) as Hora  
	from  factura_Tmp d  INNER JOIN Empresas e ON e.cod_empresa = d.cod_empresa and e.cod_pais = d.cod_pais
	       inner join Paises p on p.cod_pais = d.cod_pais
    WHERE d.no_factura = @no_factura and 
			  CONVERT(VARCHAR(10),d.fecha,103)=CONVERT(VARCHAR(10), GETDATE(), 103) and
			  d.cod_pais=@codigoPais and d.cod_empresa=@codigoEmpresa and d.cod_puesto=@codigoPuesto

  delete from factura_Tmp 
  where   no_factura = @no_factura and 
                  CONVERT(VARCHAR(10),fecha,103)=CONVERT(VARCHAR(10), GETDATE(), 103) and
                  cod_pais=@codigoPais and cod_empresa=@codigoEmpresa and cod_puesto=@codigoPuesto
				  
  update [dbo].[Puestos]	set no_factura=	no_factura+1  
  where  cod_pais=@codigoPais and cod_empresa=@codigoEmpresa and cod_puesto=@codigoPuesto  
        
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
