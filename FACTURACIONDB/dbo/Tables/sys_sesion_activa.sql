CREATE TABLE [dbo].[sys_sesion_activa] (
    [consecutivo_usuario] INT      NOT NULL,
    [cod_sesion]          INT      NOT NULL,
    [Fecha]               DATETIME DEFAULT (getdate()) NOT NULL,
    [activo]              BIT      DEFAULT ((1)) NOT NULL,
    PRIMARY KEY CLUSTERED ([consecutivo_usuario] ASC),
    CONSTRAINT [FK__sys_sesio__cod_s__403A8C7D] FOREIGN KEY ([cod_sesion]) REFERENCES [dbo].[sys_Control_Sesiones] ([cod_sesion]),
    CONSTRAINT [FK__sys_sesio__cod_u__3F466844] FOREIGN KEY ([consecutivo_usuario]) REFERENCES [dbo].[sys_usuario] ([consecutivo_usuario])
);

