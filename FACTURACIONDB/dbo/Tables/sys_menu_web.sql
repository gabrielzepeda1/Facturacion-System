CREATE TABLE [dbo].[sys_menu_web] (
    [cod_menu]     INT            NOT NULL,
    [cod_padre]    INT            NULL,
    [posicion]     INT            NOT NULL,
    [etiqueta]     NVARCHAR (100) NOT NULL,
    [ruta]         NVARCHAR (200) NULL,
    [pagina]       NVARCHAR (50)  NULL,
    [ruta_icono]   NVARCHAR (200) NULL,
    [ocultar_menu] BIT            NOT NULL,
    [activo]       BIT            NOT NULL,
    PRIMARY KEY CLUSTERED ([cod_menu] ASC)
);

