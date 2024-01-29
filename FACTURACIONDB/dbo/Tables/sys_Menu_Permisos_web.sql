CREATE TABLE [dbo].[sys_Menu_Permisos_web] (
    [cod_menu]            INT NOT NULL,
    [consecutivo_usuario] INT NOT NULL,
    PRIMARY KEY CLUSTERED ([cod_menu] ASC, [consecutivo_usuario] ASC),
    FOREIGN KEY ([cod_menu]) REFERENCES [dbo].[sys_menu_web] ([cod_menu]),
    CONSTRAINT [FK__sys_Menu___cod_u__0CBAE877] FOREIGN KEY ([consecutivo_usuario]) REFERENCES [dbo].[sys_usuario] ([consecutivo_usuario])
);

