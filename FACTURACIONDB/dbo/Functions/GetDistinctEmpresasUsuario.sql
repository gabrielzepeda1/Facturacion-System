CREATE FUNCTION GetDistinctEmpresasUsuario
(
    @CodigoUsuario INT
)
RETURNS TABLE
AS
RETURN
(
    SELECT DISTINCT 
		e.cod_empresa as codEmpresa,
		RTRIM(LTRIM(e.descripcion_corta)) AS Empresa
    FROM empresas e 
	INNER JOIN accesos_usuarios a ON a.cod_empresa = e.cod_empresa 
	WHERE a.consecutivo_usuario=@CodigoUsuario 
);
