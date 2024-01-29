CREATE TABLE [dbo].[Calidades_productos] (
    [cod_calidad]             INT          NOT NULL,
    [descripcion]             VARCHAR (50) NOT NULL,
    [consecutivo_usuario]     INT          NOT NULL,
    [consecutivo_usuario_ult] INT          NOT NULL,
    CONSTRAINT [PK_Calidades_producto] PRIMARY KEY CLUSTERED ([cod_calidad] ASC),
    CONSTRAINT [FK__Calidades__conse__70DDC3D8] FOREIGN KEY ([consecutivo_usuario]) REFERENCES [dbo].[sys_usuario] ([consecutivo_usuario]),
    CONSTRAINT [FK__Calidades__conse__71D1E811] FOREIGN KEY ([consecutivo_usuario_ult]) REFERENCES [dbo].[sys_usuario] ([consecutivo_usuario])
);

