CREATE PROCEDURE [dbo].[sp_show_puesto]
@cod_puesto INT
AS 
SELECT 
	p.cod_puesto
	, p.cod_empresa
	, p.cod_pais
	, p.descripcion
	, p.no_nota_debito
	, p.no_nota_credito
	, p.no_recibo
	, p.no_factura
	, p.no_nota_credito_retencion
	, p.formato_impresion
	, p.lineas_imprimir
	, p.consecutivo_usuario
	, p.consecutivo_usuario_ult
	, p.numero_cuotas
	, CASE WHEN p.verificar_inventario = 1 THEN 'si' ELSE 'no' END AS verificar_inventario
	, p.telefono
	, p.descripcion_corta
FROM Puestos p
WHERE
	p.cod_puesto = @cod_puesto