CREATE TABLE [dbo].[Empresas] (
    [cod_pais]                INT           NOT NULL,
    [cod_empresa]             INT           NOT NULL,
    [descripcion]             VARCHAR (50)  NOT NULL,
    [consecutivo_usuario]     INT           NOT NULL,
    [consecutivo_usuario_ult] INT           NOT NULL,
    [nom_impuesto]            VARCHAR (50)  NULL,
    [porc_impuesto]           INT           NULL,
    [descripcion_corta]       VARCHAR (15)  NULL,
    [cedula_ruc]              VARCHAR (30)  NULL,
    [direccion]               VARCHAR (MAX) NULL,
    [autorizacion_mifin]      VARCHAR (50)  NOT NULL,
    CONSTRAINT [PK_Empresas_1] PRIMARY KEY CLUSTERED ([cod_pais] ASC, [cod_empresa] ASC)
);

