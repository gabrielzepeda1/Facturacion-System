CREATE TABLE [dbo].[sys_usuario] (
    [consecutivo_usuario] INT            IDENTITY (1, 1) NOT NULL,
    [usuario]             NVARCHAR (50)  NOT NULL,
    [contrasenia]         NVARCHAR (200) NOT NULL,
    [email]               VARCHAR (30)   NULL,
    [cod_pais]            INT            NULL,
    [cod_empresa]         INT            NULL,
    [cod_puesto]          NCHAR (10)     NULL,
    [cod_rol]             INT            NOT NULL,
    [es_administrador]    BIT            NULL,
    [activo]              BIT            CONSTRAINT [DF__sys_usuar__activ__44FF419A] DEFAULT ((1)) NOT NULL,
    [Fecha_Registro]      DATETIME       CONSTRAINT [DF__sys_usuar__Fecha__45F365D3] DEFAULT (getdate()) NOT NULL,
    [ultima_conexion]     DATETIME       NULL,
    [nombre]              NVARCHAR (50)  NULL,
    [apellido]            NVARCHAR (50)  NULL,
    [telefono]            VARCHAR (15)   NULL,
    [cedula_ruc]          VARCHAR (30)   NULL,
    [direccion]           VARCHAR (50)   NULL,
    CONSTRAINT [PK__sys_usuario__0519C6AF] PRIMARY KEY CLUSTERED ([consecutivo_usuario] ASC)
);

