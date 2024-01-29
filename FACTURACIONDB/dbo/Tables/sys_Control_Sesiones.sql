CREATE TABLE [dbo].[sys_Control_Sesiones] (
    [cod_sesion]          INT           NOT NULL,
    [consecutivo_usuario] INT           NOT NULL,
    [Fecha_Inicio_Sesion] DATETIME      CONSTRAINT [DF__sys_Contr__Fecha__3A81B327] DEFAULT (getdate()) NOT NULL,
    [Fecha_Final_Sesion]  DATETIME      NULL,
    [IpConexion]          NVARCHAR (15) NOT NULL,
    [Nombre_Host]         NVARCHAR (50) NOT NULL,
    CONSTRAINT [PK__sys_Control_Sesi__398D8EEE] PRIMARY KEY CLUSTERED ([cod_sesion] ASC),
    CONSTRAINT [FK__sys_Contr__cod_u__3B75D760] FOREIGN KEY ([consecutivo_usuario]) REFERENCES [dbo].[sys_usuario] ([consecutivo_usuario])
);

