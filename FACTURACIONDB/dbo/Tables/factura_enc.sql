﻿CREATE TABLE [dbo].[factura_enc] (
    [cod_pais]                INT             NOT NULL,
    [cod_empresa]             INT             NOT NULL,
    [cod_puesto]              INT             NOT NULL,
    [no_factura]              INT             NOT NULL,
    [fecha]                   DATETIME        NOT NULL,
    [cod_sector_mercado]      INT             NOT NULL,
    [consecutivo_usuario]     INT             NOT NULL,
    [consecutivo_usuario_ult] INT             NOT NULL,
    [porc_descuento]          DECIMAL (5, 2)  NOT NULL,
    [valor_descuento]         DECIMAL (12, 2) NOT NULL,
    [valor_iva]               DECIMAL (12, 2) NOT NULL,
    [anulada]                 BIT             CONSTRAINT [DF_factura_enc_anulada] DEFAULT ((0)) NOT NULL,
    [fechaHora_anulacion]     DATETIME        NOT NULL,
    [sub_total]               DECIMAL (12, 2) NOT NULL,
    [neto]                    DECIMAL (12, 2) NOT NULL,
    [paridad]                 DECIMAL (10, 4) NOT NULL,
    [cod_vendedor]            INT             NULL,
    [cod_cliente]             VARCHAR (4)     NULL,
    [contado]                 INT             NOT NULL,
    [fecha_cancelacion]       DATETIME        NULL,
    [cedula_ruc]              VARCHAR (30)    NULL,
    [externo]                 BIT             NULL,
    [Nombre_comercial]        VARCHAR (50)    NULL,
    [Notas]                   VARCHAR (MAX)   NULL,
    CONSTRAINT [PK_factura_enc] PRIMARY KEY CLUSTERED ([cod_pais] ASC, [cod_empresa] ASC, [cod_puesto] ASC, [no_factura] ASC, [fecha] ASC),
    CONSTRAINT [FK_factura_enc_pais] FOREIGN KEY ([cod_pais]) REFERENCES [dbo].[Paises] ([cod_pais])
);

