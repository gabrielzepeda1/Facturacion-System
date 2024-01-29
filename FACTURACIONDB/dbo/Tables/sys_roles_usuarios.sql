CREATE TABLE [dbo].[sys_roles_usuarios] (
    [cod_rol]     INT          NOT NULL,
    [descripcion] VARCHAR (50) NOT NULL,
    [activo]      BIT          NULL,
    CONSTRAINT [PK__sys_role__F0D13057DB439D0F] PRIMARY KEY CLUSTERED ([cod_rol] ASC)
);

