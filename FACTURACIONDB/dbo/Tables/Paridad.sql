CREATE TABLE [dbo].[Paridad] (
    [Fecha]    DATETIME        NOT NULL,
    [cod_pais] INT             NOT NULL,
    [Paridad]  NUMERIC (18, 4) NOT NULL,
    CONSTRAINT [PK_Paridad] PRIMARY KEY CLUSTERED ([Fecha] ASC, [cod_pais] ASC) WITH (FILLFACTOR = 80),
    CONSTRAINT [FK_Paridad_Pais] FOREIGN KEY ([cod_pais]) REFERENCES [dbo].[Paises] ([cod_pais])
);

