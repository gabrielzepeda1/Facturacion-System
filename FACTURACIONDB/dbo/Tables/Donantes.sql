CREATE TABLE [dbo].[Donantes] (
    [Cod_empleado] INT           NOT NULL,
    [Nom_Empleado] VARCHAR (MAX) COLLATE Modern_Spanish_CI_AS NULL,
    [Email]        VARCHAR (MAX) COLLATE Modern_Spanish_CI_AS NULL,
    [cod_pais]     INT           NOT NULL,
    [cod_empresa]  INT           NOT NULL,
    [cod_puesto]   INT           NOT NULL,
    CONSTRAINT [PK_Donantes] PRIMARY KEY CLUSTERED ([Cod_empleado] ASC, [cod_pais] ASC, [cod_empresa] ASC, [cod_puesto] ASC)
);

