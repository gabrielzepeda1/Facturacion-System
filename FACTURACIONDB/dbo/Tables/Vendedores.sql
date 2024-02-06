CREATE TABLE [dbo].[Vendedores] (
    [cod_pais]                INT          NOT NULL,
    [cod_empresa]             INT          NOT NULL,
    [cod_puesto]              INT          NOT NULL,
    [nombres]                 VARCHAR (30) NOT NULL,
    [apellidos]               VARCHAR (30) NOT NULL,
    [num_recibo]              NUMERIC (18) NULL,
    [consecutivo_usuario]     INT          NOT NULL,
    [consecutivo_usuario_ult] INT          NOT NULL,
    [cod_vendedor]            INT          NOT NULL,
    [cta_contable]            VARCHAR (25) NULL,
    [PorDefecto]              BIT          NULL,
    CONSTRAINT [PK_Vendedores] PRIMARY KEY CLUSTERED ([cod_pais] ASC, [cod_empresa] ASC, [cod_puesto] ASC, [cod_vendedor] ASC),
    CONSTRAINT [FK_Vendedores_Empresa] FOREIGN KEY ([cod_pais], [cod_empresa]) REFERENCES [dbo].[Empresas] ([cod_pais], [cod_empresa]),
    CONSTRAINT [FK_Vendedores_Pais] FOREIGN KEY ([cod_pais]) REFERENCES [dbo].[Paises] ([cod_pais])
);

