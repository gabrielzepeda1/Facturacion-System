CREATE TABLE [dbo].[Bancos] (
    [cod_pais]    INT          NOT NULL,
    [cod_banco]   INT          NOT NULL,
    [descripcion] VARCHAR (50) NOT NULL,
    CONSTRAINT [PK_Bancos] PRIMARY KEY CLUSTERED ([cod_pais] ASC, [cod_banco] ASC)
);

