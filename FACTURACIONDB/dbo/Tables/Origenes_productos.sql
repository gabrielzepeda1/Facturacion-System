CREATE TABLE [dbo].[Origenes_productos] (
    [cod_origen]              INT          NOT NULL,
    [Descripcion]             VARCHAR (50) NOT NULL,
    [consecutivo_usuario]     INT          NOT NULL,
    [consecutivo_usuario_ult] INT          NOT NULL,
    CONSTRAINT [PK_Origenes_productos] PRIMARY KEY CLUSTERED ([cod_origen] ASC),
    CONSTRAINT [FK__Origenes___conse__6EF57B66] FOREIGN KEY ([consecutivo_usuario]) REFERENCES [dbo].[sys_usuario] ([consecutivo_usuario]),
    CONSTRAINT [FK__Origenes___conse__6FE99F9F] FOREIGN KEY ([consecutivo_usuario_ult]) REFERENCES [dbo].[sys_usuario] ([consecutivo_usuario])
);

