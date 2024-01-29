CREATE TABLE [dbo].[Siglas] (
    [cod_sigla]               INT          IDENTITY (1, 1) NOT NULL,
    [sigla]                   VARCHAR (20) NOT NULL,
    [consecutivo_usuario]     INT          NOT NULL,
    [consecutivo_usuario_ult] INT          NOT NULL,
    CONSTRAINT [PK_Siglas] PRIMARY KEY CLUSTERED ([sigla] ASC),
    CONSTRAINT [FK__Siglas__cod_usua__6477ECF3] FOREIGN KEY ([consecutivo_usuario]) REFERENCES [dbo].[sys_usuario] ([consecutivo_usuario]),
    CONSTRAINT [FK__Siglas__cod_usua__656C112C] FOREIGN KEY ([consecutivo_usuario_ult]) REFERENCES [dbo].[sys_usuario] ([consecutivo_usuario]),
    CONSTRAINT [FK__Siglas__consecut__7A672E12] FOREIGN KEY ([consecutivo_usuario]) REFERENCES [dbo].[sys_usuario] ([consecutivo_usuario]),
    CONSTRAINT [FK__Siglas__consecut__7B5B524B] FOREIGN KEY ([consecutivo_usuario_ult]) REFERENCES [dbo].[sys_usuario] ([consecutivo_usuario])
);

