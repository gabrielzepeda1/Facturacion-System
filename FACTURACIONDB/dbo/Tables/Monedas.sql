CREATE TABLE [dbo].[Monedas] (
    [cod_pais]       INT          NOT NULL,
    [cod_moneda]     INT          NOT NULL,
    [simbolo_moneda] VARCHAR (10) NOT NULL,
    [descripcion]    VARCHAR (50) NOT NULL,
    [PorDefecto]     BIT          NULL,
    CONSTRAINT [PK_Monedas_1] PRIMARY KEY CLUSTERED ([cod_pais] ASC, [cod_moneda] ASC)
);

