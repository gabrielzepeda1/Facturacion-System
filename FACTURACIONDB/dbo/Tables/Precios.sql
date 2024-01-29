CREATE TABLE [dbo].[Precios] (
    [cod_pais]            INT             NOT NULL,
    [cod_empresa]         INT             NOT NULL,
    [cod_puesto]          INT             NOT NULL,
    [cod_sector_mercado]  INT             NOT NULL,
    [cod_producto]        VARCHAR (6)     NOT NULL,
    [precio]              DECIMAL (10, 2) NOT NULL,
    [consecutivo_usuario] INT             NOT NULL,
    [fecha_registro]      DATETIME        NOT NULL,
    CONSTRAINT [PK_Precios] PRIMARY KEY CLUSTERED ([cod_pais] ASC, [cod_empresa] ASC, [cod_puesto] ASC, [cod_sector_mercado] ASC, [cod_producto] ASC),
    CONSTRAINT [FK_Precios_Pais] FOREIGN KEY ([cod_pais]) REFERENCES [dbo].[Paises] ([cod_pais]),
    CONSTRAINT [FK_Precios_Sector_mercado] FOREIGN KEY ([cod_pais], [cod_sector_mercado]) REFERENCES [dbo].[Sector_Mercados] ([cod_pais], [cod_sector_mercado])
);

