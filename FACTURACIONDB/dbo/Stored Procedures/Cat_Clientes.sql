CREATE PROCEDURE [dbo].[Cat_Clientes]
--@cod_siglas int, 
@opcion INT,
@codigoPais INT,
@razonSocial VARCHAR(50),
@direccion VARCHAR(50),
@telefono VARCHAR(15),
@Email VARCHAR(30),
@Contacto VARCHAR(30),
@CedulaRuc VARCHAR(30),
@NombreComercial VARCHAR(50),
@codCliente VARCHAR(4),
@Nombres VARCHAR(30),
@Apellidos VARCHAR(30),
@codusuario int, 
@codusuarioUlt int ,
@DiasCredito int ,
@LimiteCredito NUMERIC(18,2) ,
@CodSectorMercado int ,
@CodVendedor int ,
@Activo bit ,
@CtaContable VARCHAR(25) ,
@externo BIT,
@excentoImp BIT,
@esDistribuidora BIT,
@codempresa INT,
@BUSQUEDAD VARCHAR(200) 
AS

BEGIN TRANSACTION;
BEGIN TRY

declare @Consulta nvarchar(max)
if @opcion=1 BEGIN 	
	DECLARE @Consecutivo INT
	--SET @Consecutivo = (SELECT ISNULL(MAX(k.Cod_calidad)+1,1) FROM Calidades_productos k )
	IF EXISTS (SELECT Cod_cliente FROM  Clientes  WHERE rtrim(ltrim(cod_cliente)) = rtrim(LTRIM(@codCliente)) AND externo=@externo AND cod_empresa=@codempresa) BEGIN 
	    UPDATE Clientes SET cod_pais=@codigoPais,razon_social=rtrim(LTRIM(upper(@razonSocial))),direccion=rtrim(LTRIM(upper(@direccion))),
	                        telefono=@telefono,email=@Email,contacto=rtrim(LTRIM(upper(@Contacto))),cedula_ruc=@CedulaRuc,
	                        nombre_comercial=rtrim(LTRIM(upper(@NombreComercial))),cod_cliente=@codCliente,
	                        nombres=rtrim(LTRIM(upper(@Nombres))),apellidos=rtrim(LTRIM(upper(@Apellidos))),
	                        consecutivo_usuario=@codusuario,consecutivo_usuario_ult=@codusuarioUlt,
	                        dias_credito=@DiasCredito,limite_credito=@LimiteCredito,cod_sector_mercado=@CodSectorMercado,
	                        cod_vendedor=@CodVendedor,activo=@Activo,cta_contable=rtrim(LTRIM(upper(@CtaContable))),
	                        externo=@externo,excento_imp=@excentoImp,es_distribuidora=@esDistribuidora,cod_empresa=@codempresa
	    WHERE rtrim(ltrim(cod_cliente)) = rtrim(LTRIM(@codCliente)) AND externo=@externo AND cod_empresa=@codempresa
	     
	                                
	 END ELSE BEGIN     
	 	INSERT INTO clientes(cod_pais,razon_social,direccion,telefono,email,contacto,cedula_ruc,nombre_comercial
                            ,cod_cliente,nombres,apellidos,consecutivo_usuario,consecutivo_usuario_ult
                            ,dias_credito,limite_credito,cod_sector_mercado,cod_vendedor,activo,cta_contable
                            ,externo,excento_imp,es_distribuidora,cod_empresa)	
	 	VALUES (@codigoPais,rtrim(LTRIM(upper(@razonSocial))),rtrim(LTRIM(upper(@direccion))),@telefono,@Email,
	 	        rtrim(LTRIM(upper(@Contacto))),@CedulaRuc,rtrim(LTRIM(upper(@NombreComercial))),
	 	        @codCliente,rtrim(LTRIM(upper(@Nombres))),rtrim(LTRIM(upper(@Apellidos))),@codusuario,@codusuarioUlt,
	 	        @DiasCredito,@LimiteCredito,@CodSectorMercado,@CodVendedor,@Activo,rtrim(LTRIM(@CtaContable)),
	 	        @externo,@excentoImp,@esDistribuidora,@codempresa)
	end
END

if @opcion=2 BEGIN 	
	delete from Clientes WHERE rtrim(ltrim(cod_cliente)) = rtrim(LTRIM(@codCliente))  AND 
	                           externo=@externo AND cod_empresa=@codempresa	
END

if @opcion=3 BEGIN 	
    SET @Consulta = '
	SELECT cod_cliente as codigo,CASE WHEN externo = 1 THEN ''Si'' ELSE ''No'' END AS externo,s.cod_empresa,
	    s.cod_pais,p.descripcion AS Pais,s.razon_social,s.direccion,s.telefono,s.email,s.cedula_ruc,
        nombre_comercial,s.nombres,s.apellidos,s.cod_sector_mercado,s.contacto,
        m.descripcion AS Mercado,s.cod_vendedor,dias_credito,s.limite_credito,
        v.Nombres + '' '' + v.apellidos AS Vendedor,CASE WHEN Activo = 1 THEN ''Si'' ELSE ''No'' END AS Activo,
        s.cta_contable,
        CASE WHEN excento_imp = 1 THEN ''Si'' ELSE ''No'' END AS excento_imp,
        CASE WHEN es_distribuidora = 1 THEN ''Si'' ELSE ''No'' END AS es_distribuidora,e.descripcion AS Empresa
FROM [dbo].[Clientes] s INNER JOIN paises p ON s.cod_pais=p.cod_pais INNER JOIN [dbo].[Sector_Mercados] m ON
     s.cod_sector_mercado=m.cod_sector_mercado INNER JOIN vendedores v ON v.cod_vendedor=s.cod_vendedor
	 INNER JOIN Empresas e ON  s.cod_empresa = e.cod_empresa'
 IF @BUSQUEDAD='0' BEGIN 
		 SET @Consulta =@Consulta + 'where s.cod_pais= '+ CONVERT(NVARCHAR,@codigoPais,103) + ' order by s.nombres'
		 EXECUTE  sp_executesql @Consulta;
	     PRINT @Consulta;
 END ELSE BEGIN 
 		SET @Consulta =@Consulta + ' where s.cod_pais= '+ CONVERT(NVARCHAR,@codigoPais,103) + ' and  (s.nombres  LIKE ''%' + @BUSQUEDAD + '%'' or s.cod_cliente  LIKE ''%' + @BUSQUEDAD + '%'' or s.apellidos  LIKE ''%' + @BUSQUEDAD + '%'' or s.nombre_comercial  LIKE ''%' + @BUSQUEDAD + '%'' )' + 'order by s.nombres'----nom_empleado
 		--SET @Consulta =@Consulta + ' where MD.Cod_empleado LIKE ''%' + @BUSQUEDAD + '%'' OR e.nom_empleado LIKE ''%' + @BUSQUEDAD + '%'' OR e.ape_empleado LIKE ''%' + @BUSQUEDAD + '%'' OR cd.des_deduccion LIKE ''%' + @BUSQUEDAD + '%'' '
 		EXECUTE  sp_executesql @Consulta;
	    PRINT @Consulta;
	    
	   
 END	
END
if @opcion=4 BEGIN 	
	SELECT c.cod_pais, c.razon_social, c.direccion, c.telefono, c.email,
	       c.contacto, c.cedula_ruc, c.cod_cliente, c.nombre_comercial, c.nombres,
	       c.apellidos, c.consecutivo_usuario, c.consecutivo_usuario_ult,
	       c.dias_credito, c.limite_credito, c.cod_sector_mercado, c.cod_vendedor,
	       CASE WHEN Activo = 1 THEN 'Si' ELSE 'No' END AS activo, c.cta_contable, 
	       CASE WHEN externo = 1 THEN 'Si' ELSE 'No' END AS externo, 
	       CASE WHEN excento_imp = 1 THEN 'Si' ELSE 'No' END AS excento_imp,
	       CASE WHEN es_distribuidora = 1 THEN 'Si' ELSE 'No' END AS es_distribuidora,
	       c.cod_empresa
	  from Clientes c WHERE rtrim(ltrim(cod_cliente)) = rtrim(LTRIM(@codCliente))  AND 
	                           externo=@externo AND cod_empresa=@codempresa	
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