CREATE FUNCTION GetPaisEmpresaPuesto()
RETURNS TABLE
AS
RETURN
(
    SELECT 
        p.cod_pais AS cod_pais, 
        p.descripcion AS pais, 
        e.cod_empresa AS cod_empresa, 
        e.descripcion AS empresa, 
        pu.cod_puesto AS cod_puesto, 
        pu.descripcion AS puesto 
    FROM 
        Paises p
        INNER JOIN Empresas e ON e.cod_pais = p.cod_pais
        INNER JOIN Puestos pu ON pu.cod_empresa = e.cod_empresa
		
);