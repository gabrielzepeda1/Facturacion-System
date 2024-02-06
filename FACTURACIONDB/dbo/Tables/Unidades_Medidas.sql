CREATE TABLE [dbo].[Unidades_Medidas] (
    [cod_und_medida]          INT          NOT NULL,
    [descripcion]             VARCHAR (50) NOT NULL,
    [consecutivo_usuario]     INT          NOT NULL,
    [consecutivo_usuario_ult] INT          NULL,
    CONSTRAINT [PK_unidades_medidas] PRIMARY KEY CLUSTERED ([cod_und_medida] ASC)
);

