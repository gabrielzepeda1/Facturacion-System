CREATE TABLE [dbo].[Distribucion_Pago] (
    [Consecutivo]      INT             IDENTITY (1, 1) NOT NULL,
    [No_Factura]       NUMERIC (18)    NOT NULL,
    [Fecha_factura]    DATETIME        NOT NULL,
    [Cod_Pais]         INT             NOT NULL,
    [Cod_Empresa]      INT             NOT NULL,
    [Cod_Puesto]       INT             NOT NULL,
    [cod_FormaPago]    CHAR (1)        NULL,
    [Cod_Moneda]       INT             NULL,
    [contado_credito]  CHAR (1)        NULL,
    [ValorFacturaCor]  NUMERIC (18, 2) NULL,
    [ValorFacturaDol]  NUMERIC (18, 2) NULL,
    [ValorRecibidoCor] NUMERIC (18, 2) NULL,
    [ValorRecibidoDol] NUMERIC (18, 2) NULL,
    [Vuelto]           NUMERIC (18, 2) NULL,
    [Paridad]          NUMERIC (18, 4) NULL,
    [Anulada]          BIT             NULL,
    [ComisionTarjeta]  NUMERIC (18, 2) NULL,
    [Cod_tarjeta]      INT             NULL,
    CONSTRAINT [PK_Distribucion_Pago] PRIMARY KEY CLUSTERED ([Consecutivo] ASC, [No_Factura] ASC, [Fecha_factura] ASC, [Cod_Empresa] ASC, [Cod_Puesto] ASC, [Cod_Pais] ASC)
);

