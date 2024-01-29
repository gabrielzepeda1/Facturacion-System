CREATE FUNCTION [dbo].[GetPaises]
(
    @codigoUsuario INT
)
RETURNS TABLE
AS
RETURN
(
    SELECT p.cod_pais AS CodigoPais, TRIM(p.descripcion) AS descripcion
    FROM Paises p 

    --INNER JOIN
    --    Accesos_Usuarios a ON a.cod_pais = p.cod_pais
    --WHERE
    --    a.consecutivo_usuario = @codigoUsuario
);
