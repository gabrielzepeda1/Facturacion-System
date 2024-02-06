CREATE FUNCTION GetDistinctPuestosUsuario
(
    @CodigoUsuario INT
)
RETURNS TABLE
AS
RETURN
(
    SELECT DISTINCT 
		p.cod_puesto as codPuesto,
		RTRIM(LTRIM(descripcion)) AS Puesto
	FROM puestos AS p
	INNER JOIN accesos_usuarios AS a ON a.cod_empresa = p.cod_empresa 
	WHERE a.consecutivo_usuario=@CodigoUsuario 
);
