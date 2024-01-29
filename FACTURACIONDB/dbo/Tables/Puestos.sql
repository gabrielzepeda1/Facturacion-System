CREATE TABLE [dbo].[Puestos] (
    [cod_puesto]                INT          IDENTITY (1, 1) NOT NULL,
    [cod_empresa]               INT          NOT NULL,
    [cod_pais]                  INT          NOT NULL,
    [descripcion]               VARCHAR (50) NOT NULL,
    [no_nota_debito]            INT          NOT NULL,
    [no_nota_credito]           INT          NOT NULL,
    [no_recibo]                 INT          NOT NULL,
    [no_factura]                INT          NOT NULL,
    [no_nota_credito_retencion] INT          NOT NULL,
    [formato_impresion]         VARCHAR (50) NULL,
    [lineas_imprimir]           INT          NULL,
    [consecutivo_usuario]       INT          NOT NULL,
    [consecutivo_usuario_ult]   INT          NOT NULL,
    [numero_cuotas]             INT          NULL,
    [verificar_inventario]      BIT          NOT NULL,
    [telefono]                  VARCHAR (15) NULL,
    [descripcion_corta]         VARCHAR (15) NULL,
    [cambiar_precio]            BIT          CONSTRAINT [DF_Puestos_cambiar_precio] DEFAULT ((1)) NOT NULL,
    [lista_predeterminada]      INT          NULL,
    CONSTRAINT [PK_Puestos] PRIMARY KEY CLUSTERED ([cod_puesto] ASC, [cod_empresa] ASC, [cod_pais] ASC)
);

