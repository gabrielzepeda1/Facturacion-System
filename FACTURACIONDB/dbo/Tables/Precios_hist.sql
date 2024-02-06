CREATE TABLE [dbo].[Precios_hist] (
    [cod_pais]            INT             NOT NULL,
    [cod_empresa]         INT             NOT NULL,
    [cod_puesto]          INT             NOT NULL,
    [cod_sector_mercado]  INT             NOT NULL,
    [cod_producto]        VARCHAR (6)     NOT NULL,
    [precio]              DECIMAL (10, 2) NOT NULL,
    [consecutivo_usuario] INT             NOT NULL,
    [fecha_registro]      DATETIME        NOT NULL,
    CONSTRAINT [PK_Precios_hist] PRIMARY KEY CLUSTERED ([cod_pais] ASC, [cod_empresa] ASC, [cod_puesto] ASC, [cod_sector_mercado] ASC, [cod_producto] ASC, [fecha_registro] ASC)
);

