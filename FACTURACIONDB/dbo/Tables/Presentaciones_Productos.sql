CREATE TABLE [dbo].[Presentaciones_Productos] (
    [cod_presentacion]        INT          NOT NULL,
    [descripcion]             VARCHAR (50) NOT NULL,
    [consecutivo_usuario]     INT          NOT NULL,
    [consecutivo_usuario_ult] INT          NOT NULL,
    CONSTRAINT [PK_Presentaciones] PRIMARY KEY CLUSTERED ([cod_presentacion] ASC),
    CONSTRAINT [FK__Presentac__conse__76969D2E] FOREIGN KEY ([consecutivo_usuario]) REFERENCES [dbo].[sys_usuario] ([consecutivo_usuario]),
    CONSTRAINT [FK__Presentac__conse__778AC167] FOREIGN KEY ([consecutivo_usuario_ult]) REFERENCES [dbo].[sys_usuario] ([consecutivo_usuario])
);

