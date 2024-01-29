CREATE TABLE [dbo].[C_Not_De] (
    [Num_Notadeb]     NUMERIC (18)    IDENTITY (1, 1) NOT NULL,
    [Fecha]           DATETIME        NOT NULL,
    [Valor_Neto]      NUMERIC (18, 2) NOT NULL,
    [Cod_Cliente]     CHAR (4)        NOT NULL,
    [Nombre_Cliente]  VARCHAR (50)    NULL,
    [Paridad]         NUMERIC (18, 4) NOT NULL,
    [Cod_Puesto]      CHAR (3)        NOT NULL,
    [CodigoUser]      CHAR (10)       NOT NULL,
    [Fecha_Canc]      DATETIME        NULL,
    [Concepto]        VARCHAR (100)   NOT NULL,
    [Interno_Externo] CHAR (1)        NOT NULL,
    [Cod_Empresa]     CHAR (3)        NOT NULL,
    CONSTRAINT [PK_Nota_deb] PRIMARY KEY CLUSTERED ([Num_Notadeb] ASC, [Cod_Cliente] ASC, [CodigoUser] ASC, [Fecha] ASC, [Cod_Puesto] ASC, [Cod_Empresa] ASC) WITH (FILLFACTOR = 90)
);

