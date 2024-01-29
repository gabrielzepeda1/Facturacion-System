CREATE TABLE [dbo].[Paises] (
    [cod_pais]                INT           NOT NULL,
    [descripcion]             VARCHAR (50)  NOT NULL,
    [consecutivo_usuario]     INT           NOT NULL,
    [consecutivo_usuario_ult] INT           NOT NULL,
    [LeyendaUno]              VARCHAR (MAX) NULL,
    [LeyendaDos]              VARCHAR (MAX) NULL,
    [LeyendaTres]             VARCHAR (MAX) NULL,
    [LeyendaCuatro]           VARCHAR (MAX) NULL,
    CONSTRAINT [PK_Paises1] PRIMARY KEY CLUSTERED ([cod_pais] ASC),
    CONSTRAINT [FK__Paises__consecut__74AE54BC] FOREIGN KEY ([consecutivo_usuario]) REFERENCES [dbo].[sys_usuario] ([consecutivo_usuario]),
    CONSTRAINT [FK__Paises__consecut__75A278F5] FOREIGN KEY ([consecutivo_usuario_ult]) REFERENCES [dbo].[sys_usuario] ([consecutivo_usuario])
);

