CREATE TABLE [dbo].[Productos_pais] (
    [cod_pais]                INT          NOT NULL,
    [cod_producto]            VARCHAR (6)  NOT NULL,
    [cod_empresa]             INT          NOT NULL,
    [cod_und_medida]          INT          NOT NULL,
    [descripcion]             VARCHAR (50) NOT NULL,
    [desc_imprimir]           VARCHAR (25) NOT NULL,
    [num_producto]            INT          NOT NULL,
    [activo]                  BIT          CONSTRAINT [DF_Productos_pais_activo] DEFAULT ((1)) NOT NULL,
    [excento_imp]             BIT          CONSTRAINT [DF_Productos_pais_excento_imp] DEFAULT ((1)) NOT NULL,
    [consecutivo_usuario]     INT          NOT NULL,
    [consecutivo_usuario_ult] INT          NOT NULL,
    CONSTRAINT [PK_Productos_pais_1] PRIMARY KEY CLUSTERED ([cod_pais] ASC, [cod_producto] ASC, [cod_empresa] ASC)
);

