CREATE PROCEDURE [dbo].[VerifiInventario]

--DECLARE 
@opcion INT,
@codigoPais INT,
@codigoEmpresa INT,
@codigoPuesto INT
AS 

  
--SET  @opcion=1
--set @codigoPais =  1
--set @codigoEmpresa =  4 
--set @codigoPuesto =  1 

BEGIN TRANSACTION;
BEGIN TRY
      --DELETE FROM TmpDetFact WHERE 					  
						--  cod_pais=@codigoPais and cod_empresa=@codigoEmpresa and cod_puesto=@codigoPuesto
 
 --INTO CURSOR tmpexism ORDER BY cod_producto,Numero_Mes,Ano
	DECLARE @PrimDiaMes datetime
	SET @PrimDiaMes=(SELECT DATEADD(MONTH, DATEDIFF(MONTH, 0, GETDATE()), 0))

	--select @PrimDiaMes
	DECLARE @prodc TABLE (cod_producto VARCHAR(20),salida numeric(18,2),entrada NUMERIC(18,2),
						 venta NUMERIC(18,2),donaciones NUMERIC(18,2),CanInicial NUMERIC(18,2),neto NUMERIC(18,2) )
	INSERT INTO @prodc(cod_producto) 
	SELECT COD_PRODUCTO FROM Productos_pais pp 
	WHERE cod_empresa=@codigoEmpresa  AND cod_pais=@codigoPais

	DECLARE @CanInicial TABLE (cod_producto VARCHAR(20),CanInicial numeric(18,2))
	INSERT INTO @CanInicial 
	SELECT cod_producto,SUM(Existencia) from Exis_mes  
	WHERE numero_mes=MONTH(GETDATE())and agnio=year(GETDATE()) AND cod_pais=@codigoPais and 
	cod_empresa=@codigoEmpresa and cod_puesto=@codigoPuesto  --ORDER BY Cod_Producto
	GROUP BY cod_producto

	DECLARE @donaciones TABLE (cod_producto VARCHAR(20),donaciones numeric(18,2))
	INSERT INTO @donaciones 
	select cod_producto,SUM(d.Cantidad)
	  from dbo.Donaciones d where  FechaDia >= @PrimDiaMes AND FechaDia<=DATEADD(dd, 0, DATEDIFF(dd, 0, GETDATE())) AND 
				cod_pais=@codigoPais and cod_empresa=@codigoEmpresa and cod_puesto=@codigoPuesto  
	GROUP BY d.Cod_Producto 


	---cabeza de loma gold3-
	DECLARE @venta TABLE (cod_producto VARCHAR(20),venta numeric(18,2))
	INSERT INTO @venta 
	SELECT cod_producto,SUM(cantidad) from factura_det --ORDER BY cod_producto 
	WHERE fecha >= @PrimDiaMes AND fecha <= DATEADD(dd, 0, DATEDIFF(dd, 0, GETDATE())) AND 
		   anulada=0 AND cod_empresa=@codigoEmpresa and cod_puesto=@codigoPuesto AND cod_pais=@codigoPais 
	GROUP BY Cod_Producto --INTO CURSOR tmpdet  &&tmpem.cod_empresa



	DECLARE @ingreso TABLE (cod_producto VARCHAR(20),entrada numeric(18,2))
	INSERT INTO @ingreso
	SELECT cod_producto,sum(cantidad) AS ingreso from mov_inventario r INNER JOIN Tipo_Movimiento t ON  t.Tipo_movim=r.Tipo_movim
	 where fecha >= @PrimDiaMes AND 
	       --fecha <= CONVERT(NVARCHAR, GETDATE(),103) AND 
	       anulada=0 AND t.suma=1 
		   AND cod_empresa=@codigoEmpresa and cod_puesto=@codigoPuesto AND cod_pais=@codigoPais
	GROUP BY cod_producto

	DECLARE @salida TABLE (cod_producto VARCHAR(20),salida numeric(18,2))
	INSERT INTO @salida
	SELECT cod_producto,sum(cantidad) AS salida from mov_inventario r  INNER JOIN Tipo_Movimiento t ON  t.Tipo_movim=r.Tipo_movim
	 where fecha >= @PrimDiaMes AND fecha <= DATEADD(dd, 0, DATEDIFF(dd, 0, GETDATE())) AND anulada=0 AND t.suma=0 
		   AND cod_empresa=@codigoEmpresa and cod_puesto=@codigoPuesto AND cod_pais=@codigoPais
	GROUP BY cod_producto  



	UPDATE @prodc SET salida=s.salida FROM @prodc p JOIN @salida s ON rtrim(ltrim(s.cod_producto)) = rtrim(ltrim(p.cod_producto))
	UPDATE @prodc SET entrada=s.entrada FROM @prodc p JOIN @ingreso s ON rtrim(ltrim(s.cod_producto)) = rtrim(ltrim(p.cod_producto))  
	UPDATE @prodc SET venta=s.venta FROM @prodc p JOIN @venta s ON rtrim(ltrim(s.cod_producto)) = rtrim(ltrim(p.cod_producto))
	UPDATE @prodc SET donaciones=s.donaciones FROM @prodc p JOIN @donaciones s ON rtrim(ltrim(s.cod_producto)) = rtrim(ltrim(p.cod_producto))
	UPDATE @prodc SET CanInicial=s.CanInicial FROM @prodc p JOIN @CanInicial s ON rtrim(ltrim(s.cod_producto)) = rtrim(ltrim(p.cod_producto))  

	UPDATE @prodc SET SALIDA=ISNULL(SALIDA,0),ENTRADA=ISNULL(ENTRADA,0),venta=ISNULL(venta,0),donaciones =ISNULL(DONACIONES,0),CanInicial =ISNULL(CanInicial,0)
	UPDATE @prodc SET neto =(CanInicial+entrada)-(salida+venta+donaciones)
	UPDATE @prodc SET neto =ISNULL(NETO,0)
	
	--SELECT * FROM precios WHERE cod_empresa=@codigoEmpresa and cod_puesto=@codigoPuesto AND cod_pais=@codigoPais
	--SELECT COD_PRODUCTO,NETO FROM @prodc  WHERE neto<>0 ORDER BY cod_producto
	SELECT p.COD_PRODUCTO,p.NETO,excento_imp,o.desc_imprimir,cod_und_medida as Unidad
	FROM @prodc p inner join Productos_pais o on rtrim(ltrim(p.COD_PRODUCTO))=rtrim(ltrim(o.COD_PRODUCTO))  
	    -- inner join [dbo].[Unidades_Medidas] u on u.cod_und_medida=o.cod_und_medida
	WHERE neto<>0 and o.cod_empresa=@codigoEmpresa  AND o.cod_pais=@codigoPais
	ORDER BY p.cod_producto
	
	--select * from  Productos_pais
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