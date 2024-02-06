CREATE TABLE [dbo].[Donaciones] (
    [cod_puesto]    INT             NOT NULL,
    [cod_pais]      INT             NOT NULL,
    [cod_empresa]   INT             NOT NULL,
    [Num_Docum]     VARCHAR (10)    NOT NULL,
    [Fecha_Docum]   DATETIME        NOT NULL,
    [Es_Remision]   BIT             NULL,
    [Institucion]   VARCHAR (MAX)   NULL,
    [Funcionario]   VARCHAR (MAX)   NULL,
    [AutorizadoPor] INT             NOT NULL,
    [Cantidad]      NUMERIC (7, 2)  NULL,
    [Cod_Producto]  VARCHAR (6)     NOT NULL,
    [Valor]         NUMERIC (18, 2) NULL,
    [FechaDia]      DATETIME        NULL,
    [Codigo]        VARCHAR (50)    COLLATE Modern_Spanish_CI_AS NULL,
    CONSTRAINT [PK_Donaciones] PRIMARY KEY CLUSTERED ([cod_puesto] ASC, [cod_pais] ASC, [cod_empresa] ASC, [Num_Docum] ASC, [Fecha_Docum] ASC, [Cod_Producto] ASC)
);

