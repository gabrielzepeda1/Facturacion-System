CREATE TABLE [dbo].[Sector_Mercados] (
    [cod_pais]                INT           NOT NULL,
    [cod_sector_mercado]      INT           NOT NULL,
    [descripcion]             NVARCHAR (50) NOT NULL,
    [consecutivo_usuario]     INT           NOT NULL,
    [consecutivo_usuario_ult] INT           NOT NULL,
    CONSTRAINT [PK_sector_mercados] PRIMARY KEY CLUSTERED ([cod_pais] ASC, [cod_sector_mercado] ASC)
);

