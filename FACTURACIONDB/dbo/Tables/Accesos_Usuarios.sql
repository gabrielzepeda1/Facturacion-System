CREATE TABLE [dbo].[Accesos_Usuarios] (
    [consecutivo_usuario] INT NOT NULL,
    [cod_pais]            INT NOT NULL,
    [cod_empresa]         INT NOT NULL,
    [cod_puesto]          INT NOT NULL,
    [crea_movimientos]    BIT CONSTRAINT [DF_Accesos_Usuarios_crea_movimientos] DEFAULT ((0)) NULL,
    CONSTRAINT [PK_Accesos_Usuarios] PRIMARY KEY CLUSTERED ([consecutivo_usuario] ASC, [cod_pais] ASC, [cod_empresa] ASC, [cod_puesto] ASC)
);

