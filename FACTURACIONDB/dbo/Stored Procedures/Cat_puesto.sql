CREATE PROCEDURE [dbo].[Cat_puesto]
--@cod_siglas int, 
@opcion INT,
@codPuesto INT,
@codEmpresa INT,
@codPais INT,
@descripcion VARCHAR(50),
@noNotaDebito INT,
@noNotaCredito INT,
@noRecibo INT,
@noFactura INT,
@noCreditoRet INT,
@formatoImpresion VARCHAR(50),
@lineasImprimir INT,
@codusuario int, 
@codusuarioUlt int ,
@NoCuotasPlanillas INT,
@VerifInventario BIT,
@telefono VARCHAR(15),
@descripCorta VARCHAR(50), 
@BUSQUEDAD VARCHAR(200)

	 
AS
---SELECT * FROM [dbo].[empresas] 
BEGIN TRANSACTION;
BEGIN TRY
--SELECT * FROM [dbo].[Empleados]
declare @Consulta nvarchar(max)
if @opcion=1 BEGIN 	
	DECLARE @Consecutivo INT
	SET @Consecutivo = (SELECT ISNULL(MAX(k.Cod_puesto)+1,1) FROM puestos k )
	IF EXISTS (SELECT Cod_puesto FROM  puestos  WHERE cod_puesto = @codPuesto) BEGIN 
	    UPDATE puestos SET descripcion=rtrim(LTRIM(upper(@descripcion))),cod_pais=@codPais,cod_empresa =@codEmpresa,
	                         no_nota_debito =@noNotaDebito,no_nota_credito =@noNotaCredito,no_recibo = @noRecibo,
	                         no_factura = @noFactura,no_nota_credito_retencion =@noCreditoRet,
	                         formato_impresion = @formatoImpresion,lineas_imprimir = @lineasImprimir,
	                         consecutivo_usuario_ult = @codusuarioUlt,numero_cuotas=@NoCuotasPlanillas,
	                         verificar_inventario=@VerifInventario,telefono=@telefono,descripcion_corta=@descripCorta 
	    WHERE cod_puesto=@codPuesto

 END ELSE BEGIN     
	 	INSERT INTO puestos(cod_empresa,cod_PAIS,COD_puesto,descripcion,no_nota_debito,
	 	            no_nota_credito, no_recibo, no_factura,no_nota_credito_retencion, formato_impresion, 
	 	            lineas_imprimir,consecutivo_usuario,consecutivo_usuario_ult,numero_cuotas,verificar_inventario,telefono,descripcion_corta)	
	 	VALUES (@codEmpresa,@codPais,@Consecutivo,rtrim(LTRIM(upper(@descripcion))),@noNotaDebito,
	 	        @noNotaCredito,@noRecibo,@noFactura,@noCreditoRet,@formatoImpresion,
	 	        @lineasImprimir,@codusuario,@codusuarioUlt,@NoCuotasPlanillas,@VerifInventario,@telefono,@descripCorta)
	    --INSERT INTO siglas	VALUES(@Consecutivo,@siglas)
	end
END

if @opcion=2 BEGIN 	
	delete from  puestos  WHERE COD_puesto=@codPuesto	
END

if @opcion=3 BEGIN 	
	--SELECT cod_origen AS codigo,UPPER(descripcion) AS Descripcion,cod_usuario AS usuario,cod_usuario_ult AS UsuarioUltiMod 
	--from Origenes_productos sCASE WHEN produccion_sm = 1 THEN ''Si'' ELSE ''No'' END AS produccion_sm
	 SET @Consulta = '
	SELECT cod_puesto AS codigo,UPPER(s.descripcion) AS Puesto,rtrim(ltrim(P.DESCRIPCION)) AS Pais,
	       rtrim(ltrim(e.descripcion)) as Empresa,no_nota_debito as NoNotaDebito,no_nota_credito as NoNotaCredito,
	       no_recibo as NoRecibo,
	       no_factura as NoFactura,no_nota_credito_retencion as NoNotaCredRetencion,
	       isnull(formato_impresion,''.'') as FormatoImpresion,isnull(lineas_imprimir,'''') as LineasImprimir,	       	      
	       numero_cuotas as NoCuotasenPlan,case when verificar_inventario=1 then ''Si'' ELSE ''No'' END AS VerifInventario,
	       telefono,rtrim(ltrim(s.descripcion_corta)) as DesCorta,
	       s.consecutivo_usuario AS usuario,s.consecutivo_usuario_ult AS UsuarioUltiMod,s.COD_PAIS,s.cod_empresa
    FROM puestos s INNER JOIN PAISES P ON S.COD_PAIS=P.COD_PAIS inner join empresas e on e.cod_empresa=s.cod_empresa'
 IF @BUSQUEDAD='0' BEGIN 
		 SET @Consulta =@Consulta + 'where s.cod_pais= '+ CONVERT(NVARCHAR,@codPais,103) + ' order by s.descripcion'
		 EXECUTE  sp_executesql @Consulta;
	     PRINT @Consulta;
 END ELSE BEGIN 
 		SET @Consulta =@Consulta + ' where s.cod_pais= '+ CONVERT(NVARCHAR,@codPais,103) + ' and s.descripcion  LIKE ''%' + @BUSQUEDAD + '%''  ' + 'order by s.descripcion'----nom_empleado
 		--SET @Consulta =@Consulta + ' where MD.Cod_empleado LIKE ''%' + @BUSQUEDAD + '%'' OR e.nom_empleado LIKE ''%' + @BUSQUEDAD + '%'' OR e.ape_empleado LIKE ''%' + @BUSQUEDAD + '%'' OR cd.des_deduccion LIKE ''%' + @BUSQUEDAD + '%'' '
 		EXECUTE  sp_executesql @Consulta;
	    PRINT @Consulta;
	    
	   
 END	
END

if @opcion=4 BEGIN 	
	SELECT p.cod_puesto, p.cod_empresa AS codEmpresa, p.cod_pais, p.descripcion,
	       p.no_nota_debito, p.no_nota_credito, p.no_recibo, p.no_factura,
	       p.no_nota_credito_retencion, p.formato_impresion, p.lineas_imprimir,
	       p.numero_cuotas, p.verificar_inventario, p.telefono,
	       p.descripcion_corta
	  FROM puestos p  WHERE COD_puesto=@codPuesto	
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