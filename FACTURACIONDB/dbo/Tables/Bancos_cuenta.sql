CREATE TABLE [dbo].[Bancos_cuenta] (
    [cod_banco_cta] INT          NOT NULL,
    [cod_pais]      INT          NOT NULL,
    [cod_empresa]   INT          NOT NULL,
    [cod_puesto]    INT          NOT NULL,
    [cod_banco]     INT          NOT NULL,
    [cod_moneda]    INT          NOT NULL,
    [descripcion]   VARCHAR (50) NOT NULL,
    CONSTRAINT [PK_Banco_cuenta] PRIMARY KEY CLUSTERED ([cod_banco_cta] ASC, [cod_pais] ASC, [cod_empresa] ASC, [cod_puesto] ASC, [cod_banco] ASC, [cod_moneda] ASC),
    CONSTRAINT [FK_Bancos_cuenta_Bancos] FOREIGN KEY ([cod_pais], [cod_banco]) REFERENCES [dbo].[Bancos] ([cod_pais], [cod_banco])
);

