CREATE PROCEDURE [dbo].[CombosProductos]
--@cod_siglas int, 
@opcion INT,
@codigo INT
AS

BEGIN TRANSACTION;
BEGIN TRY

if @opcion=1 BEGIN 	
	SELECT cod_origen as codOrigen,RTRIM(ltrim(Descripcion)) AS DesOrigen
	  FROM  Origenes_productos 
END

if @opcion=2 BEGIN 	
	SELECT cod_calidad as codCalidad,RTRIM(ltrim(Descripcion)) AS DesCalidad
	  FROM  Calidades_productos 
END
if @opcion=3 BEGIN 	
	SELECT cod_presentacion as codPresentacion,RTRIM(ltrim(Descripcion)) AS DesPresentacion
	  FROM  Presentaciones_productos 
END
if @opcion=4 BEGIN 	
	SELECT cod_familia as codFamilia,RTRIM(ltrim(Descripcion)) AS DesFamilia
	  FROM  Familias_productos 
END
if @opcion=5 BEGIN 	
	SELECT sigla  FROM  siglas 
END

IF @opcion=6 BEGIN 	
	SELECT DISTINCT p.cod_pais [CodigoPais], RTRIM(LTRIM(p.descripcion)) [Pais]
	  FROM Paises p 
	  INNER JOIN accesos_usuarios a ON a.cod_pais = p.cod_pais 
	  WHERE a.consecutivo_usuario = @codigo
END

if @opcion=7 BEGIN 	
	SELECT consecutivo_usuario as codUsuario, usuario AS Usuario
	FROM  sys_usuario 
	WHERE activo = 1 
END

if @opcion=8 BEGIN 	
	SELECT distinct e.cod_empresa as codEmpresa,RTRIM(ltrim(e.descripcion_corta)) AS Empresa
    FROM empresas AS e 
	INNER JOIN accesos_usuarios a ON a.cod_empresa = e.cod_empresa 
	WHERE a.consecutivo_usuario=@codigo 
END
 
if @opcion=9 BEGIN 	
	SELECT p.cod_puesto as codPuesto,RTRIM(ltrim(descripcion)) AS Puesto
	FROM puestos AS p
	INNER JOIN accesos_usuarios AS a ON a.cod_empresa = p.cod_empresa WHERE a.consecutivo_usuario=@codigo 
END

if @opcion=10 BEGIN 	
	SELECT cod_vendedor as codVendedor,RTRIM(ltrim(nombres))+' '+RTRIM(ltrim(apellidos)) AS Vendedor
	FROM  vendedores
END

if @opcion=11 BEGIN 	
	SELECT cod_und_medida as cod_und_medida,RTRIM(ltrim(Descripcion)) AS UnidadMedida
	FROM  Unidades_Medidas
END

if @opcion=12 BEGIN 	
	SELECT cod_Producto as codigo,RTRIM(ltrim(Descripcion)) AS Producto
	FROM  Productos_Maestro WHERE cod_producto NOT IN (SELECT cod_producto FROM Productos_pais WHERE Productos_pais.cod_empresa=@codigo)
END

if @opcion=13 BEGIN 	
	SELECT cod_sector_mercado as codigoSecMerc,RTRIM(ltrim(Descripcion)) AS Mercado
	FROM  [dbo].[Sector_Mercados] WHERE cod_pais=@codigo
END
if @opcion=14 BEGIN 	
	SELECT cod_banco as codBanco,RTRIM(ltrim(Descripcion)) AS Banco
	FROM  bancos WHERE cod_pais=@codigo
END

if @opcion=15 BEGIN 	
	SELECT cod_moneda as codMoneda,RTRIM(ltrim(Descripcion)) AS Moneda
	FROM Monedas WHERE cod_pais=@codigo
END
if @opcion=16 
	BEGIN 	
		SELECT 
		Externo, CodigoCliente AS CodigoCliente,
		TRIM(Nombres) + ' ' + TRIM(Apellidos) AS Cliente,
		Externo
		FROM Clientes WHERE CodigoPais=@codigo 
		ORDER BY Nombres ASC 
END

if @opcion=17 BEGIN 	
	SELECT cod_Producto as codProduto,
	RTRIM(ltrim(desc_imprimir)) AS Descripcion,RTRIM(ltrim(desc_imprimir))+'-'+RTRIM(ltrim(cod_Producto)) AS Producto 
	FROM Productos_pais WHERE cod_pais=@codigo order by RTRIM(ltrim(desc_imprimir))
END

if @opcion=18 BEGIN   ----COMBO ACCESO_USARIOS 	
	SELECT distinct e.cod_empresa as codEmpresa,RTRIM(ltrim(e.descripcion_corta)) AS Empresa
    FROM  empresas e WHERE e.cod_pais=@codigo--INNER JOIN accesos_usuarios a ON a.cod_empresa = e.cod_empresa WHERE a.consecutivo_usuario=@codigo 
END

if @opcion=19 BEGIN   ----COMBO ACCESO_USARIOS 	
    SELECT cod_puesto as codPuesto,RTRIM(ltrim(descripcion)) AS Puesto
	FROM  puestos WHERE cod_empresa=@codigo
END 	

IF @opcion=20  ----pantalla de ingreso al sistema ACCESO_USARIOS 	
	BEGIN  
	   SELECT 
			CONVERT(VARCHAR(10),au.cod_pais) + '-'+ p.descripcion AS Pais,
			CONVERT(VARCHAR(10),au.cod_empresa)+ '-' + e.descripcion_corta AS Empresa,
		    convert(VARCHAR(10),au.cod_puesto)+'-'+pu.descripcion_corta AS Puesto,
		    au.cod_pais,
			au.cod_empresa,
			au.cod_puesto,

		    CASE WHEN pu.cambiar_precio=1 
				THEN 'Si' 
				ELSE 'No' 
			END AS cambiar_precio, 
		    ISNULL(pu.lista_predeterminada,0) AS lista_predeterminada,

		    CASE WHEN pu.verificar_inventario=1 
				THEN 'Si' 
				ELSE 'No' 
			END AS verificar_inventario,

		    porc_impuesto AS PorcImtos,
			cedula_ruc,
			autorizacion_mifin,
			direccion,
			telefono

		    FROM Accesos_Usuarios au 
			INNER JOIN Paises p ON p.cod_pais = au.cod_pais 
			INNER JOIN Empresas e ON e.cod_empresa = au.cod_empresa
			INNER JOIN Puestos pu ON pu.cod_puesto = au.cod_puesto 
			WHERE au.consecutivo_usuario=@codigo
      
END 
if @opcion=21 BEGIN   ----COMBO ACCESO_USARIOS 	
    SELECT cod_FormaPago as codFormaPago,RTRIM(ltrim(descripcion)) AS FormaPago
	FROM [dbo].[Forma_pago]  WHERE cod_pais=@codigo
END

if @opcion=22 BEGIN   ----COMBO ACCESO_USARIOS 	
    SELECT cod_Tarjeta as codTarjeta,RTRIM(ltrim(descripcion)) AS Tarjeta
	FROM [dbo].[Tarjeta]  WHERE cod_pais=@codigo

END

IF @opcion=23 BEGIN 

	SELECT cod_banco_cta as codBancoCta, RTRIM(LTRIM(Descripcion)) AS Cuenta 
	FROM Bancos_cuenta WHERE cod_pais=@codigo

END 

IF @opcion = 24 BEGIN --DROPDOWN ROL 
	SELECT cod_rol AS codRol, RTRIM(LTRIM(descripcion)) AS Rol 
	FROM sys_roles_usuarios WHERE activo = 1

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