CREATE TABLE [dbo].[mov_inventario] (
    [Recibo_Ref]    CHAR (12)       COLLATE Modern_Spanish_CI_AS NOT NULL,
    [Fecha]         DATETIME        NOT NULL,
    [Cod_Empresa]   INT             NOT NULL,
    [Cod_Producto]  CHAR (6)        COLLATE Modern_Spanish_CI_AS NOT NULL,
    [Consecutivo]   NUMERIC (18)    NOT NULL,
    [cod_pais]      INT             NOT NULL,
    [cod_puesto]    INT             NOT NULL,
    [cod_documento] INT             NULL,
    [Cantidad]      NUMERIC (18, 2) NOT NULL,
    [Anulada]       BIT             NOT NULL,
    [Tipo_movim]    INT             NULL,
    [Valor_Dol]     NUMERIC (18, 2) NULL,
    [Paridad]       NUMERIC (18, 4) NULL,
    [ValorMoneNac]  NUMERIC (18, 2) NULL,
    [Consignatario] VARCHAR (50)    COLLATE Modern_Spanish_CI_AS NULL,
    CONSTRAINT [PK_Recepcion] PRIMARY KEY CLUSTERED ([Recibo_Ref] ASC, [Fecha] ASC, [Cod_Empresa] ASC, [Cod_Producto] ASC, [Consecutivo] ASC, [cod_pais] ASC, [cod_puesto] ASC),
    CONSTRAINT [FK_mov_inventario_tipo_movimiento] FOREIGN KEY ([Tipo_movim]) REFERENCES [dbo].[Tipo_Movimiento] ([Tipo_movim])
);

