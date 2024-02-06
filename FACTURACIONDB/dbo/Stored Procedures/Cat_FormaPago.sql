CREATE PROCEDURE [dbo].[Cat_FormaPago]
--@cod_siglas int, 
@opcion INT,
@codigoPais INT,
@codigoEmpresa INT,
@codigoPuesto INT,
@codigoFormaPago VARCHAR(1),
@descripcion VARCHAR(50),
@BUSQUEDAD VARCHAR(200),
@PorDefecto int
AS

BEGIN TRANSACTION;
BEGIN TRY

declare @Consulta nvarchar(max)

IF @opcion=1 

	BEGIN 	

	DECLARE @Consecutivo int
	SET @Consecutivo = (SELECT ISNULL(count(k.Cod_FormaPago)+1,1) FROM Forma_pago  k )

	IF EXISTS 
	(SELECT Cod_FormaPago FROM Forma_pago  
	WHERE cod_FormaPago = @codigoFormaPago
	and cod_pais=@codigoPais
	and cod_empresa=@codigoEmpresa
	and cod_puesto=@codigoPuesto)

	BEGIN 
	    UPDATE Forma_pago 
		SET cod_pais=@codigoPais,cod_empresa=@codigoEmpresa,cod_puesto =@codigoPuesto,
	       descripcion = (LTRIM(upper(@descripcion))),PorDefecto=@PorDefecto
	                       WHERE cod_FormaPago=@codigoFormaPago and cod_pais=@codigoPais 
						         and cod_empresa=@codigoEmpresa
						         and cod_puesto=@codigoPuesto
	 END ELSE BEGIN  
	 	INSERT INTO Forma_pago(Cod_FormaPago,cod_pais,cod_empresa,cod_puesto,descripcion,PorDefecto)
	 	 VALUES (@codigoFormaPago,@codigoPais,@codigoEmpresa,@codigoPuesto,rtrim(LTRIM(upper(@descripcion))),@PorDefecto)
	end
END
if @opcion=2 BEGIN 	
	delete from Forma_pago WHERE cod_FormaPago=@codigoFormaPago	
END

if @opcion=3 BEGIN 	
	--SELECT cod_origen AS codigo,UPPER(descripcion) AS Descripcion,cod_usuario AS usuario,cod_usuario_ult AS UsuarioUltiMod 
	--from Origenes_productos s and pu.cod_pais=v.cod_pais
	 SET @Consulta = '
	SELECT cod_FormaPago as Codigo,UPPER(v.descripcion) AS descripcion,
	p.descripcion as Pais,e.descripcion as Empresa,pu.descripcion as Puesto,
	v.Cod_pais AS Cod_pais,v.cod_empresa,v.Cod_puesto,
	CASE WHEN PorDefecto = 1 THEN ''Si'' ELSE ''No'' END AS  PorDefecto
	FROM Forma_pago v inner join Paises p on v.cod_pais=p.cod_pais inner join Empresas e on v.cod_empresa=e.cod_empresa
	     inner join Puestos pu on pu.cod_puesto=v.cod_puesto   '
 IF @BUSQUEDAD='0' BEGIN 
		 SET @Consulta =@Consulta + 'where v.cod_pais= '+ CONVERT(NVARCHAR,@codigoPais,103) + ' order by v.descripcion'
		 EXECUTE  sp_executesql @Consulta;
	     PRINT @Consulta;
 END ELSE BEGIN 
 		SET @Consulta =@Consulta + ' where v.cod_pais= '+ CONVERT(NVARCHAR,@codigoPais,103) + ' and  v.descripcion  LIKE ''%' + @BUSQUEDAD + '%''  ' + 'order by v.descripcion'----nom_empleado
 		--SET @Consulta =@Consulta + ' where MD.Cod_empleado LIKE ''%' + @BUSQUEDAD + '%'' OR e.nom_empleado LIKE ''%' + @BUSQUEDAD + '%'' OR e.ape_empleado LIKE ''%' + @BUSQUEDAD + '%'' OR cd.des_deduccion LIKE ''%' + @BUSQUEDAD + '%'' '
 	    EXECUTE  sp_executesql @Consulta;
	    PRINT @Consulta;
	    
	   
 END
END
if @opcion=4 BEGIN 	
	SELECT cod_FormaPago,v.cod_pais, v.cod_empresa, v.cod_puesto, v.descripcion,PorDefecto
	--CASE WHEN PorDefecto = 1 THEN 'Si' ELSE 'No' END AS  PorDefecto
	from Forma_pago v WHERE cod_FormaPago=@codigoFormaPago	
END	


IF @opcion=5

BEGIN
    DECLARE @paridad NUMERIC(18, 4),
            @Formapag VARCHAR(2),
            @moneda INT

    -- Get paridad, default to 0 if not found
    SELECT @paridad = COALESCE((SELECT TOP 1 paridad
                                FROM paridad
                                WHERE cod_pais = @codigoPais
                                    AND CONVERT(VARCHAR(10), fecha, 103) = CONVERT(VARCHAR(10), GETDATE(), 103)
                                ), 0)

    -- Get Formapag, default to '99' if not found
    SELECT @Formapag = COALESCE((SELECT TOP 1 cod_FormaPago
                                FROM Forma_pago
                                WHERE cod_pais = @codigoPais
                                    AND cod_empresa = @codigoEmpresa
                                    AND cod_puesto = @codigoPuesto
                                    AND PorDefecto = 1
                                ), '99')
								
    -- Get moneda, default to 99 if not found
    SELECT @moneda = COALESCE((SELECT TOP 1 cod_moneda
                                FROM monedas
                                WHERE cod_pais = @codigoPais
                                    AND PorDefecto = 1
                                ), 99)

    SELECT @Formapag AS codformapago,
           @moneda AS moneda,
           @paridad AS paridad
END





	--BEGIN
   
	--DECLARE 
	--@paridad NUMERIC(18,4),
	--@Formapag VARCHAR(2),
	--@moneda INT,
	--@contParidad INT,
	--@cantFormaPago INT,
	--@cantMoneda INT

 --  SET @contParidad=(SELECT count(*) AS cantidad 
	--				 FROM paridad 
	--				 WHERE cod_pais=@codigoPais 
	--				 AND CONVERT(VARCHAR(10), fecha, 103)=CONVERT(VARCHAR(10), GETDATE(), 103))

 --  SET @cantFormaPago=(SELECT count(*) AS cantidad
	--				   FROM Forma_pago  
	--				   WHERE cod_pais=@codigoPais
	--				   AND cod_empresa=@codigoEmpresa
	--				   AND cod_puesto=@codigoPuesto 
	--				   AND PorDefecto=1)


 --  SET @cantMoneda=(SELECT count(*) AS cantidad FROM Monedas  
	--				WHERE cod_pais=@codigoPais
	--				AND PorDefecto=1)

	--IF @contParidad=0 
	--	BEGIN
	--	   SET @paridad=0
	--END 
	--ELSE 
	--	BEGIN 
	--		SET @paridad=(SELECT paridad FROM paridad WHERE cod_pais=@codigoPais AND CONVERT(VARCHAR(10), fecha, 103)=CONVERT(VARCHAR(10), GETDATE(), 103))
	--END 

	--IF @cantFormaPago = 0 
	--	BEGIN
	--	   SET @Formapag='99'
	--	END 
	--ELSE 
	--	BEGIN 
	--		SET @Formapag=(
	--						SELECT cod_FormaPago 
	--						FROM Forma_pago 
	--						WHERE cod_pais=@codigoPais
	--						AND cod_empresa=@codigoEmpresa
	--						AND cod_puesto=@codigoPuesto 
	--						AND PorDefecto=1)
	--END 

	--IF  @cantMoneda=0 
	--	BEGIN
	--	   SET @moneda=99
	--	END 
	--ELSE 
	--	BEGIN 
	--		SET @moneda=(SELECT cod_moneda
	--					 FROM monedas 
	--					 WHERE cod_pais=@codigoPais 
	--					 AND PorDefecto=1)
	--	END 	
	
 --  SELECT
	--@Formapag AS codformapago,
	--@moneda AS moneda,
	--@paridad AS paridad 




   	

		
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