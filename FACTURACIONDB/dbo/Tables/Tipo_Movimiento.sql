CREATE TABLE [dbo].[Tipo_Movimiento] (
    [Tipo_movim]  INT           CONSTRAINT [DF_Tipo_Movimiento_Tipo_movim] DEFAULT ((1)) NOT NULL,
    [Descripcion] VARCHAR (MAX) NOT NULL,
    [Suma]        BIT           NOT NULL,
    CONSTRAINT [PK_Tipo_Movimiento] PRIMARY KEY CLUSTERED ([Tipo_movim] ASC)
);

