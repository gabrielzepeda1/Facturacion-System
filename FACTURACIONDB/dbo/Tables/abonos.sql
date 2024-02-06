CREATE TABLE [dbo].[abonos] (
    [Cod_pais]            INT             NOT NULL,
    [Cod_Empresa]         INT             NOT NULL,
    [Cod_Puesto]          INT             NOT NULL,
    [Num_Recibo]          CHAR (10)       NOT NULL,
    [Tipo_Docum]          CHAR (1)        NOT NULL,
    [Num_Docum]           NUMERIC (18)    NOT NULL,
    [fechadia]            DATETIME        NOT NULL,
    [Interno_Externo]     CHAR (1)        NOT NULL,
    [Cod_Cliente]         CHAR (15)       NOT NULL,
    [Valor_Pend]          NUMERIC (18, 2) NOT NULL,
    [Valor_Apli]          NUMERIC (18, 2) NOT NULL,
    [Saldo]               NUMERIC (18, 2) NOT NULL,
    [consecutivo_usuario] INT             NOT NULL,
    [Fecha_docum]         DATETIME        NOT NULL,
    [cod_sector_mercado]  INT             NOT NULL,
    [cod_vendedor]        INT             NOT NULL,
    CONSTRAINT [PK_abo_nota] PRIMARY KEY CLUSTERED ([Num_Recibo] ASC, [Tipo_Docum] ASC, [Num_Docum] ASC, [fechadia] ASC, [Cod_Puesto] ASC, [Cod_Empresa] ASC, [cod_vendedor] ASC) WITH (FILLFACTOR = 90)
);

