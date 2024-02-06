CREATE TABLE [dbo].[comis_VendFamilia] (
    [cod_pais]     INT            NOT NULL,
    [cod_empresa]  INT            NOT NULL,
    [cod_puesto]   INT            NOT NULL,
    [cod_familia]  INT            NOT NULL,
    [cod_vendedor] INT            NOT NULL,
    [comision]     NUMERIC (5, 2) NOT NULL,
    [activo]       BIT            NOT NULL,
    CONSTRAINT [PK_comis_VendFamilia] PRIMARY KEY CLUSTERED ([cod_pais] ASC, [cod_empresa] ASC, [cod_puesto] ASC, [cod_familia] ASC, [cod_vendedor] ASC)
);

