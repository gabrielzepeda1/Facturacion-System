CREATE TABLE [dbo].[Denominaciones] (
    [cod_denominacion] INT            NOT NULL,
    [cod_pais]         INT            NOT NULL,
    [cod_moneda]       VARCHAR (20)   NOT NULL,
    [denominacion]     DECIMAL (8, 2) NOT NULL,
    [descripcion]      VARCHAR (50)   NOT NULL,
    CONSTRAINT [PK_Denominaciones] PRIMARY KEY CLUSTERED ([cod_denominacion] ASC, [cod_pais] ASC, [cod_moneda] ASC, [denominacion] ASC)
);

