CREATE TABLE [dbo].[sys_menu_web_parent] (
    [cod_menu] INT            NOT NULL,
    [etiqueta] NVARCHAR (100) NOT NULL,
    [posicion] INT            NOT NULL,
    [ruta]     NVARCHAR (200) NULL,
    [pagina]   NVARCHAR (50)  NULL,
    [icono]    NVARCHAR (200) NULL,
    [ocultar]  BIT            NOT NULL,
    [activo]   BIT            NOT NULL,
    PRIMARY KEY CLUSTERED ([cod_menu] ASC)
);

