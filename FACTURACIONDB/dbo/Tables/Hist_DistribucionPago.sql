CREATE TABLE [dbo].[Hist_DistribucionPago] (
    [cod_pais]         INT             NOT NULL,
    [cod_empresa]      INT             NOT NULL,
    [cod_puesto]       INT             NOT NULL,
    [Consecutivo]      NUMERIC (18)    NOT NULL,
    [No_Factura]       NUMERIC (18)    NOT NULL,
    [Fecha_Factura]    DATETIME        NOT NULL,
    [Cod_ForPago]      CHAR (1)        NULL,
    [Desc_ForPago]     CHAR (60)       NULL,
    [Cod_Moneda]       CHAR (2)        NULL,
    [contado]          INT             NULL,
    [ValorFacturaCor]  NUMERIC (18, 2) NULL,
    [ValorFacturaDol]  NUMERIC (18, 2) NULL,
    [ValorRecibidoCor] NUMERIC (18, 2) NULL,
    [ValorRecibidoDol] NUMERIC (18, 2) NULL,
    [Vuelto]           NUMERIC (18, 2) NULL,
    [Paridad]          NUMERIC (18, 4) NULL,
    [Anulada]          BIT             NULL,
    [ComisionTarjeta]  NUMERIC (18, 2) NULL,
    [Cod_tarjeta]      INT             NULL,
    CONSTRAINT [PK_Hist_DistribucionPago] PRIMARY KEY CLUSTERED ([Consecutivo] ASC, [No_Factura] ASC, [Fecha_Factura] ASC, [cod_empresa] ASC, [cod_puesto] ASC)
);

