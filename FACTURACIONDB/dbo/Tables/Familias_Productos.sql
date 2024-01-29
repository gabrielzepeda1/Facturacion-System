CREATE TABLE [dbo].[Familias_Productos] (
    [cod_familia]             INT          NOT NULL,
    [descripcion]             VARCHAR (50) NOT NULL,
    [consecutivo_usuario]     INT          NOT NULL,
    [consecutivo_usuario_ult] INT          NOT NULL,
    CONSTRAINT [PK_Tipos_Productos] PRIMARY KEY CLUSTERED ([cod_familia] ASC),
    CONSTRAINT [FK__Familias___conse__72C60C4A] FOREIGN KEY ([consecutivo_usuario]) REFERENCES [dbo].[sys_usuario] ([consecutivo_usuario]),
    CONSTRAINT [FK__Familias___conse__73BA3083] FOREIGN KEY ([consecutivo_usuario_ult]) REFERENCES [dbo].[sys_usuario] ([consecutivo_usuario])
);

