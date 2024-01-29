CREATE TABLE [dbo].[Forma_pago] (
    [cod_pais]      INT          NOT NULL,
    [cod_empresa]   INT          NOT NULL,
    [cod_puesto]    INT          NOT NULL,
    [cod_FormaPago] CHAR (1)     NOT NULL,
    [descripcion]   VARCHAR (50) NOT NULL,
    [PorDefecto]    BIT          NULL,
    CONSTRAINT [PK_Forma_pago] PRIMARY KEY CLUSTERED ([cod_pais] ASC, [cod_empresa] ASC, [cod_puesto] ASC, [cod_FormaPago] ASC)
);

