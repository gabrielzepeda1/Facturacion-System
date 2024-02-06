CREATE TABLE [dbo].[Productos_Maestro] (
    [cod_producto]            VARCHAR (6)  NOT NULL,
    [num_producto]            INT          NOT NULL,
    [descripcion]             VARCHAR (30) NOT NULL,
    [sigla]                   VARCHAR (20) NOT NULL,
    [cod_origen]              INT          NOT NULL,
    [cod_calidad]             INT          NOT NULL,
    [cod_presentacion]        INT          NOT NULL,
    [cod_familia]             INT          NOT NULL,
    [consecutivo_usuario]     INT          NOT NULL,
    [consecutivo_usuario_ult] INT          NOT NULL,
    [produccion_sm]           BIT          NOT NULL,
    CONSTRAINT [PK_Productos_maestro] PRIMARY KEY CLUSTERED ([cod_producto] ASC),
    CONSTRAINT [FK__Productos__conse__787EE5A0] FOREIGN KEY ([consecutivo_usuario]) REFERENCES [dbo].[sys_usuario] ([consecutivo_usuario]),
    CONSTRAINT [FK__Productos__conse__797309D9] FOREIGN KEY ([consecutivo_usuario_ult]) REFERENCES [dbo].[sys_usuario] ([consecutivo_usuario]),
    CONSTRAINT [FK_Productos_maestro_Calidades_productos] FOREIGN KEY ([cod_calidad]) REFERENCES [dbo].[Calidades_productos] ([cod_calidad]),
    CONSTRAINT [FK_Productos_maestro_Familias_productos] FOREIGN KEY ([cod_familia]) REFERENCES [dbo].[Familias_Productos] ([cod_familia]),
    CONSTRAINT [FK_Productos_maestro_Origenes_productos] FOREIGN KEY ([cod_origen]) REFERENCES [dbo].[Origenes_productos] ([cod_origen]),
    CONSTRAINT [FK_Productos_maestro_Presentaciones_productos] FOREIGN KEY ([cod_presentacion]) REFERENCES [dbo].[Presentaciones_Productos] ([cod_presentacion]),
    CONSTRAINT [FK_Productos_maestro_Siglas] FOREIGN KEY ([sigla]) REFERENCES [dbo].[Siglas] ([sigla])
);

