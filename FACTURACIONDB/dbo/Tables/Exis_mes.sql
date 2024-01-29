CREATE TABLE [dbo].[Exis_mes] (
    [Cod_Empresa]  INT             NOT NULL,
    [Cod_Producto] CHAR (6)        COLLATE Modern_Spanish_CI_AS NOT NULL,
    [Numero_Mes]   CHAR (2)        COLLATE Modern_Spanish_CI_AS NOT NULL,
    [Agnio]        CHAR (4)        COLLATE Modern_Spanish_CI_AS NOT NULL,
    [Existencia]   NUMERIC (18, 2) NOT NULL,
    [cod_pais]     INT             NOT NULL,
    [cod_puesto]   INT             NOT NULL,
    CONSTRAINT [PK_Exis_mes] PRIMARY KEY CLUSTERED ([Cod_Empresa] ASC, [Cod_Producto] ASC, [Numero_Mes] ASC, [Agnio] ASC, [cod_pais] ASC, [cod_puesto] ASC)
);

