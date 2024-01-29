CREATE PROCEDURE [dbo].[Factura]

--@cod_siglas int, 
@opcion INT,
@codigoPais INT,
@codigoPuesto INT,
@codigoEmpresa int,
@no_factura int,
@fecha datetime,
@cod_producto varchar(6),
@consecutivoUsuario int,
@porc_descuento numeric(18,2),
@valor_descuento numeric(18,2),
@porc_iva numeric(18,2),
@valor_iva numeric(18,2),
@anulada bit,
@fechaHora_anulacion datetime,
@sub_total numeric(18,2),
@paridad numeric(18,4),
@codcliente VARCHAR(4),
@cantidad numeric(18,2),
@bultos int,
@cod_und_medida int,
@precio_unidad numeric(18,2),
@Notas varchar(60),
@Contado int,
@codvendedor int,
@cedularuc varchar(30),
@externo bit,
@desc_imprimir varchar(25),
@NombreCliente varchar(max)

AS

BEGIN TRANSACTION;
BEGIN TRY
--select CONVERT(VARCHAR(10), GETDATE(), 103)
--select round(123.456, 2, 1)
if @opcion=1 BEGIN 	
   declare @valorCord as numeric(18,2),@valorDol as numeric(18,2),@Moneda int,@FormaPago varchar(2),@fechacancela datetime
   		 if @codcliente='0' OR @codcliente='' begin
			  set @fechacancela=CONVERT(VARCHAR(10), GETDATE(), 103)
		 end else begin
			  set @fechacancela=(select GETDATE() + DiasCredito as fecCan
				  				from clientes where CodigoCliente=@codcliente and externo=@externo)
		 end 
   IF not EXISTS (SELECT no_factura FROM  factura_Tmp WHERE no_factura = @no_factura and 
                  CONVERT(VARCHAR(10),fecha,103)=CONVERT(VARCHAR(10), GETDATE(), 103) and
                  cod_pais=@codigoPais and cod_empresa=@codigoEmpresa and cod_puesto=@codigoPuesto
				  and ltrim(cod_producto)=ltrim(@cod_producto) ) BEGIN 
	
		insert into factura_Tmp (cod_pais,cod_empresa,cod_puesto,no_factura,fecha,cod_producto,consecutivo_usuario,
		                        consecutivo_usuario_ult,porc_descuento,valor_descuento,porc_iva,valor_iva
					            ,sub_total,paridad,cod_cliente,cantidad,bultos,cod_und_medida,precio_unidad
					            ,desc_imprimir,Nombre_comercial,externo,cedula_ruc,cod_vendedor,Notas,contado,fecha_cancelacion)
		
		values (@codigoPais,@codigoEmpresa,@codigoPuesto,@no_factura,CONVERT(VARCHAR(10), GETDATE(), 103),
		         @cod_producto,@consecutivoUsuario,@consecutivoUsuario,@porc_descuento
				,@valor_descuento,@porc_iva,@valor_iva,@sub_total,@paridad,@codcliente,@cantidad,@bultos
				,@cod_und_medida,@precio_unidad,@desc_imprimir,upper(@NombreCliente),@externo,@cedularuc,@codvendedor
				,upper(@Notas),@Contado,@fechacancela)

			
   end else begin 
  
       update factura_Tmp set porc_descuento=@porc_descuento,valor_descuento=@valor_descuento,sub_total=@sub_total,
	            valor_iva=@valor_iva,cantidad=@cantidad,bultos=@bultos,precio_unidad=@precio_unidad,
				Nombre_comercial=@NombreCliente,externo=@externo,cedula_ruc=@cedularuc
        WHERE no_factura = @no_factura and 
               CONVERT(VARCHAR(10),fecha,103)=CONVERT(VARCHAR(10), GETDATE(), 103) and
               cod_pais=@codigoPais and cod_empresa=@codigoEmpresa and cod_puesto=@codigoPuesto
			   and ltrim(cod_producto)=ltrim(@cod_producto)
   end 

   UPDATE factura_Tmp SET valor_descuento = sub_total * (@porc_descuento/100) 
          where   no_factura = @no_factura and 
                  CONVERT(VARCHAR(10),fecha,103)=CONVERT(VARCHAR(10), GETDATE(), 103) and
                  cod_pais=@codigoPais and cod_empresa=@codigoEmpresa and cod_puesto=@codigoPuesto
   UPDATE factura_Tmp SET valor_iva = (sub_total-valor_descuento) * (porc_iva/100) 
           WHERE @valor_iva <>0 and no_factura = @no_factura and 
                  CONVERT(VARCHAR(10),fecha,103)=CONVERT(VARCHAR(10), GETDATE(), 103) and
                  cod_pais=@codigoPais and cod_empresa=@codigoEmpresa and cod_puesto=@codigoPuesto
				   and ltrim(cod_producto)=ltrim(@cod_producto)
    UPDATE factura_Tmp SET neto =((sub_total)-(valor_descuento))+(valor_iva)
           WHERE  no_factura = @no_factura and 
                  CONVERT(VARCHAR(10),fecha,103)=CONVERT(VARCHAR(10), GETDATE(), 103) and
                  cod_pais=@codigoPais and cod_empresa=@codigoEmpresa and cod_puesto=@codigoPuesto
				   and ltrim(cod_producto)=ltrim(@cod_producto)

   	--set @valorCord=(select round((sum(sub_total)-SUM(VALOR_DESCUENTO))+sum(Valor_Iva),2,1) as Neto
	   --             from factura_Tmp 
	   --             WHERE no_factura = @no_factura and 
				--		  CONVERT(VARCHAR(10),fecha,103)=CONVERT(VARCHAR(10), GETDATE(), 103) and
				--		  cod_pais=@codigoPais and cod_empresa=@codigoEmpresa and cod_puesto=@codigoPuesto)
    --set @valorDol=(select round(((sum(sub_total)-SUM(VALOR_DESCUENTO))+sum(Valor_Iva))/@paridad,2,1) as NetoDol
    --              from factura_Tmp 
		  --        WHERE no_factura = @no_factura and 
				--		  CONVERT(VARCHAR(10),fecha,103)=CONVERT(VARCHAR(10), GETDATE(), 103) and
				--		  cod_pais=@codigoPais and cod_empresa=@codigoEmpresa and cod_puesto=@codigoPuesto)
						  
	if @Contado=1 begin
	   set  @Moneda=1
	   set @FormaPago='E'
	end else begin 
	   set  @Moneda=0
	   set @FormaPago=''
	end
    
	 --declare @tab table (CantTota numeric(18,2),Subtotal numeric(18,2),Iva numeric(18,2),Descuento numeric(18,2),
	 --                   Neto numeric(18,2),NetoDol numeric(18,2),no_factura int,fecha datetime,cod_pais int,
		--				cod_empresa int,cod_puesto int)
  --  insert into @tab

 
   	select sum(Cantidad) as Cantidad,sum(sub_total) as Subtotal,sum(Valor_Iva) as Iva,SUM(VALOR_DESCUENTO) AS Descuento,
			   round((sum(sub_total)-SUM(VALOR_DESCUENTO))+sum(Valor_Iva),2,1) as Neto,
			   round(((sum(sub_total)-SUM(VALOR_DESCUENTO))+sum(Valor_Iva))/@paridad,2,1) as NetoDol,
			   @no_factura as no_factura,CONVERT(VARCHAR(10), GETDATE(), 103) as fecha,
			   @codigoPais as cod_pais,@codigoEmpresa as cod_empresa,@codigoPuesto as cod_puesto
		from factura_Tmp 
		WHERE no_factura = @no_factura and 
						  CONVERT(VARCHAR(10),fecha,103)=CONVERT(VARCHAR(10), GETDATE(), 103) and
						  cod_pais=@codigoPais and cod_empresa=@codigoEmpresa and cod_puesto=@codigoPuesto

 --   select t.*, d.no_factura as NoFactura, d.cod_producto as Codigo, d.desc_imprimir as Producto,d.cantidad as Cantidad, 
	--       d.bultos as Bultos,d.precio_unidad as Precio,d.sub_total as SubTotal,d.valor_descuento as Descuento, 
	--	   d.valor_iva as Iva
	--from @tab t inner join factura_Tmp d on 
	--                      d.no_factura = t.no_factura and 
	--					  CONVERT(VARCHAR(10),t.fecha,103)=CONVERT(VARCHAR(10), d.fecha, 103) and
	--					  t.cod_pais=d.cod_pais and t.cod_empresa=d.cod_empresa and t.cod_puesto=d.cod_puesto
	
	
end 
if @opcion=2 BEGIN 	
   insert into [dbo].[factura_det]
           (cod_pais,cod_empresa,cod_puesto,no_factura,fecha,cod_producto,consecutivo_usuario,consecutivo_usuario_ult
           ,porc_descuento,valor_descuento,porc_iva,valor_iva,anulada,fechaHora_anulacion
           ,sub_total,neto,cantidad,bultos,cod_und_medida,precio_unidad,desc_imprimir)
    select cod_pais,cod_empresa,cod_puesto,no_factura,fecha,cod_producto,consecutivo_usuario,consecutivo_usuario_ult,
	        porc_descuento,valor_descuento,porc_iva,valor_iva,0,'1900/01/01'
			,sub_total,(sub_total-valor_descuento)+valor_iva,cantidad,bultos
            ,cod_und_medida,precio_unidad,desc_imprimir
	from factura_Tmp
	WHERE no_factura = @no_factura and 
						  CONVERT(VARCHAR(10),fecha,103)=CONVERT(VARCHAR(10), GETDATE(), 103) and
						  cod_pais=@codigoPais and cod_empresa=@codigoEmpresa and cod_puesto=@codigoPuesto
	--SELECT  @codcliente
	declare @tcliente table (cod_sector_mercado int,nombre_comercial varchar(50),fechaCance datetime)
	if @codcliente='0' OR @codcliente='' begin 					  
          insert into @tcliente(cod_sector_mercado,nombre_comercial,fechaCance)
	                  values (0,@NombreCliente,CONVERT(VARCHAR(10), GETDATE(), 103))
    end else begin 
	      insert into @tcliente
		  select CodigoSectorMercado, Nombres + '' + Apellidos AS NombreCompleto , 
				  GETDATE() + DiasCredito  fecCan
		  from clientes where CodigoCliente=@codcliente and externo=@externo
    end 
	--SELECT * FROM @tcliente
    insert into [dbo].[factura_enc] (cod_pais,cod_empresa,cod_puesto,no_factura,fecha,cod_sector_mercado
              ,consecutivo_usuario,consecutivo_usuario_ult
           ,porc_descuento,valor_descuento,valor_iva,anulada,fechaHora_anulacion
           ,sub_total,neto,paridad,cod_vendedor,cod_cliente,contado,fecha_cancelacion,cedula_ruc,
           Nombre_comercial,externo,notas)
    select cod_pais,cod_empresa,cod_puesto,no_factura,fecha,t.cod_sector_mercado,
	       consecutivo_usuario,consecutivo_usuario,porc_descuento,SUM(valor_descuento),
		   SUM(valor_iva),0,'1900/01/01',sum(sub_total),(sum(sub_total)-sum(valor_descuento))+sum(valor_iva),
	       @paridad,cod_vendedor,@codcliente,@Contado,fecha_Cancelacion,cedula_ruc,d.nombre_comercial,@externo,@notas
    from factura_Tmp d, @tcliente t
    WHERE no_factura = @no_factura and 
						  CONVERT(VARCHAR(10),fecha,103)=CONVERT(VARCHAR(10), GETDATE(), 103) and
						  cod_pais=@codigoPais and cod_empresa=@codigoEmpresa and cod_puesto=@codigoPuesto
   group by cod_pais,cod_empresa,cod_puesto,no_factura,fecha,t.cod_sector_mercado,consecutivo_usuario,
            porc_descuento,porc_iva,t.fechaCance,cedula_ruc,d.nombre_comercial,cod_vendedor,fecha_Cancelacion
   if @Contado=0 begin
      insert  into [dbo].[factura_cxc] (cod_pais,cod_empresa,cod_puesto,no_factura,fecha,cod_sector_mercado
                                       ,consecutivo_usuario,consecutivo_usuario_ult,porc_descuento
                                       ,valor_descuento,porc_iva,valor_iva,anulada,fechaHora_anulacion
                                       ,sub_total,neto,paridad,cod_vendedor,cod_cliente,contado_credito,
									   fecha_cancelacion,limite_excedido,facturas_vencidas,cedula_ruc,Nombre_comercial,
									   externo)
	  select cod_pais,cod_empresa,cod_puesto,no_factura,fecha,t.cod_sector_mercado,
	        consecutivo_usuario,consecutivo_usuario,porc_descuento,
			SUM(valor_descuento),porc_iva,SUM(valor_iva),0 as anula,'1900/01/01',
		   sum(sub_total),(sum(sub_total)-sum(valor_descuento))+sum(valor_iva),@paridad,
	       @codvendedor,@codcliente,@Contado,t.fechaCance,0,0,@cedularuc,d.nombre_comercial,@externo
       from factura_Tmp d, @tcliente t
       WHERE no_factura = @no_factura and 
						  CONVERT(VARCHAR(10),fecha,103)=CONVERT(VARCHAR(10), GETDATE(), 103) and
						  cod_pais=@codigoPais and cod_empresa=@codigoEmpresa and cod_puesto=@codigoPuesto
      group by cod_pais,cod_empresa,cod_puesto,no_factura,fecha,t.cod_sector_mercado,consecutivo_usuario,
            porc_descuento,porc_iva,t.fechaCance,d.nombre_comercial

      INSERT INTO [dbo].[Hist_DistribucionPago]
                   (cod_pais,cod_empresa,cod_puesto,Consecutivo,No_Factura,Fecha_Factura,Cod_ForPago,Desc_ForPago
                   ,Cod_Moneda,Contado,ValorFacturaCor,ValorFacturaDol,ValorRecibidoCor,ValorRecibidoDol
                    ,Vuelto,Paridad,Anulada,ComisionTarjeta,Cod_tarjeta)
     select cod_pais,cod_empresa,cod_puesto,1 as cons,no_factura,fecha,0 as cod_forma,'' as desfor,
	        0 as cod_moneda,0 as contado,
			(sum(sub_total)-sum(valor_descuento))+sum(valor_iva) as ValorFact,(sum(sub_total)-sum(valor_descuento))+sum(valor_iva)/@paridad,
			0 as valorRecibido,0 as valorRecDol,0 as vuelto,@paridad as paridad,0 as anulada,0 as comiTarje,0 as cod_tarje 
	   from factura_Tmp d
       WHERE no_factura = @no_factura and 
						  CONVERT(VARCHAR(10),fecha,103)=CONVERT(VARCHAR(10), GETDATE(), 103) and
						  cod_pais=@codigoPais and cod_empresa=@codigoEmpresa and cod_puesto=@codigoPuesto
      group by cod_pais,cod_empresa,cod_puesto,no_factura,fecha
            
				                  
   end 
end 	

if @opcion=3 BEGIN 	 
    select  d.no_factura, d.cod_producto, d.desc_imprimir as Producto,d.cantidad as Cantidad, 
	       d.bultos as Bultos,d.precio_unidad as Precio,d.sub_total as SubTotal,d.valor_descuento as Descuento, 
		   d.valor_iva as Iva,cod_pais,fecha,cod_empresa,cod_puesto
	from  factura_Tmp d 
    WHERE no_factura = @no_factura and 
			  CONVERT(VARCHAR(10),fecha,103)=CONVERT(VARCHAR(10), GETDATE(), 103) and
			  cod_pais=@codigoPais and cod_empresa=@codigoEmpresa and cod_puesto=@codigoPuesto

end 
--if @opcion=5 BEGIN 	---eliminar factura 
--        delete from factura_Tmp 
--        where   no_factura = @no_factura and 
--                  CONVERT(VARCHAR(10),fecha,103)=CONVERT(VARCHAR(10), GETDATE(), 103) and
--                  cod_pais=@codigoPais and cod_empresa=@codigoEmpresa and cod_puesto=@codigoPuesto
--				  --and  ltrim(cod_producto)=ltrim(@cod_producto)
--		update [dbo].[Puestos]	set no_factura=	no_factura+1  
--		where  cod_pais=@codigoPais and cod_empresa=@codigoEmpresa and cod_puesto=@codigoPuesto   
--end 
if @opcion=4 BEGIN 	---borrar por producto
        
		delete from factura_Tmp 
        where   no_factura = @no_factura and 
                  CONVERT(VARCHAR(10),fecha,103)=CONVERT(VARCHAR(10), GETDATE(), 103) and
                  cod_pais=@codigoPais and cod_empresa=@codigoEmpresa and cod_puesto=@codigoPuesto
				  and  ltrim(cod_producto)=ltrim(@cod_producto)
				   
     	select isnull(sum(Cantidad),0) as Cantidad,isnull(sum(sub_total),0) as Subtotal,
		       isnull(sum(Valor_Iva),0) as Iva,isnull(SUM(VALOR_DESCUENTO),0) AS Descuento,
			   isnull(round((sum(sub_total)-SUM(VALOR_DESCUENTO))+sum(Valor_Iva),2,1),0) as Neto,
			   isnull(round(((sum(sub_total)-SUM(VALOR_DESCUENTO))+sum(Valor_Iva))/@paridad,2,1),0) as NetoDol,
			   @no_factura as no_factura,CONVERT(VARCHAR(10), GETDATE(), 103) as fecha,
			   @codigoPais as cod_pais,@codigoEmpresa as cod_empresa,@codigoPuesto as cod_puesto
		from factura_Tmp 
		WHERE no_factura = @no_factura and 
						  CONVERT(VARCHAR(10),fecha,103)=CONVERT(VARCHAR(10), GETDATE(), 103) and
						  cod_pais=@codigoPais and cod_empresa=@codigoEmpresa and cod_puesto=@codigoPuesto


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

	--declare
--@opcion INT,
--@codigoPais INT,
--@codigoPuesto INT,
--@codigoEmpresa int,
--@no_factura int,
--@fecha datetime,
--@cod_producto varchar(6),
--@consecutivoUsuario int,
--@porc_descuento numeric(18,2),
--@valor_descuento numeric(18,2),
--@porc_iva numeric(18,2),
--@valor_iva numeric(18,2),
--@anulada bit,
--@fechaHora_anulacion datetime,
--@sub_total numeric(18,2),
--@paridad numeric(18,4),
--@codcliente VARCHAR(4),
--@cantidad numeric(18,2),
--@bultos int,
--@cod_und_medida int,
--@precio_unidad numeric(18,2),
--@Contado int,
--@codvendedor int,
--@cedularuc varchar(30),
--@externo bit,
--@desc_imprimir varchar(2)

--set @opcion =1
--set @codigoPais =1
--set @codigoPuesto =1
--set @codigoEmpresa =1
--set @no_factura =2
--set @fecha ='01/01/1900'
--set @cod_producto ='CCS99'
--set @consecutivoUsuario =4
--set @porc_descuento =0
--set @valor_descuento =0
--set @porc_iva =2
--set @valor_iva =0
--set @anulada =0
--set @fechaHora_anulacion ='01/01/1900'
--set @sub_total = 28
--set @paridad = 36.0000 
--set @codcliente ='0'
--set @cantidad =2
--set @bultos =1
--set @cod_und_medida =3
--set @precio_unidad =14
--set @Contado =1
--set @codvendedor ='4'
--set @cedularuc ='45'
--set @externo =1
--set @desc_imprimir ='yuy'

--EXEC Factura @opcion=1,@codigoPais = 1,@codigoPuesto = 1,@codigoEmpresa = 4,@no_factura = 2,@fecha = '01/01/1900',
--@cod_producto = 'CCS99',@consecutivoUsuario = 4,@porc_descuento = 0,@valor_descuento = 0,@porc_iva = 2,
--@valor_iva = 0,@anulada =  0,@fechaHora_anulacion = '01/01/1900',@sub_total = 28,@paridad = 36.0000,@codcliente = '0',
--@cantidad = 2,@bultos = 1,@cod_und_medida = 3,@precio_unidad = 14,@Contado = 1,@codvendedor  = '4',@cedularuc= 45,
--@externo= 1,@desc_imprimir = 'yuy'