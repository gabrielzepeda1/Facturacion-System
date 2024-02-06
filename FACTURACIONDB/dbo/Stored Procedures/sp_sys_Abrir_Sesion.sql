CREATE PROCEDURE [dbo].[sp_sys_Abrir_Sesion]
@Username NVARCHAR(50),
@IpConexion NVARCHAR(15),
@Nombre_Host NVARCHAR(50)
AS 
BEGIN TRANSACTION;
BEGIN TRY

	SET DATEFORMAT DMY;
	DECLARE 
	@CodigoSesion INT, @CodigoUser INT, @IsActive BIT, @UltimaConexion DATETIME, @MinutosRestantes INT,@CodigoPais INT,@Pais VARCHAR(100),@CodigoEmpresa INT,@Empresa VARCHAR(100),@CodigoPuesto INT,@Puesto VARCHAR(100)
	
	--cod_sesion
	SET @CodigoSesion = (SELECT ISNULL(MAX(scs.cod_sesion)+1,1) AS CodigoSesion FROM sys_Control_Sesiones scs) 
	
	--consecutivo_usuario
	SET @CodigoUser = (SELECT su.consecutivo_usuario AS CodigoUser FROM sys_usuario su WHERE su.usuario = @Username)

	--Activo, query devuelve el estado del @CodigoUSer
	SET @IsActive = (SELECT ssa.activo As Activo FROM sys_sesion_activa ssa WHERE ssa.consecutivo_usuario = @CodigoUser)

	--Ultima conexion del usuario, guardamos la columna "Fecha" de la tabla sys_usuario en la variable @UltimaConexion
	SET @UltimaConexion = (SELECT ssa.Fecha AS UltimaConexion FROM sys_sesion_activa ssa WHERE ssa.consecutivo_usuario = @CodigoUser)

	--@Diferencia, calcula la cantidad de minutos que pasaron desde la ultima conexion del usuario. 
	SET @MinutosRestantes = (SELECT DATEDIFF(Minute, @UltimaConexion, GETDATE()) % 60 As Minutos)


	SET @CodigoPais = (SELECT cod_pais FROM sys_usuario  WHERE consecutivo_usuario = @CodigoUser)
	SET @CodigoEmpresa= (SELECT cod_empresa FROM sys_usuario  WHERE consecutivo_usuario = @CodigoUser)
	SET @CodigoPuesto=(SELECT cod_puesto FROM sys_usuario WHERE consecutivo_usuario = @CodigoUser)
	SET @Pais = (SELECT descripcion FROM Paises WHERE cod_pais = @CodigoPais)
	SET @Empresa = (SELECT descripcion FROM Empresas WHERE cod_empresa = @CodigoEmpresa) 
	SET @Puesto = (SELECT descripcion FROM Puestos WHERE cod_puesto = @CodigoPuesto)
	 
	--Devuelve MENSAJE si el @CodigoUser se encuentra conectado. Si han pasado menos de 15 minutos desde la ultima conexion, no permite que inicie otra sesión a menos que cierre la sesión que se encuentra activa. 
	IF @IsActive = 1 AND @MinutosRestantes < 15 
	BEGIN
		SELECT 'El usuario ' + @Username + ' se encuentra conectado en' + @Nombre_Host 
		+ ', espere ' 
		+ CONVERT(NVARCHAR, (15 - @MinutosRestantes)) 
		+ ' minutos o cierre sesión en todos los dispositivos para acceder desde el equipo actual.' AS Status
	END 

	--Si el usuario no tiene una sesión iniciada, abrimos la sesion para insertarla en la tabla sys_Control_Sesiones
	ELSE 
	BEGIN

		--Tabla donde almacenamos todas las sesiones del sistema. 
		INSERT INTO sys_Control_Sesiones (cod_sesion, consecutivo_usuario, Fecha_Inicio_Sesion, Fecha_Final_Sesion, IpConexion, Nombre_Host)

		VALUES (@CodigoSesion, @CodigoUser, GETDATE(), NULL, @IpConexion, @Nombre_Host)
	
		--SP que abre la sesion del usuario especificado. 	
		EXEC sp_sys_sesion_activa
			@cod_sesion = @CodigoSesion,
			@cod_usuario = @CodigoUser,
			@estado = 'ACTIVAR'
	
		SELECT 
			scs.cod_sesion AS CodigoSesion,
			scs.consecutivo_usuario AS CodigoUser,
			su.usuario AS Username,
			su.contrasenia [Password], 
			su.cod_rol as CodigoRol,
			'SESION INICIADA' AS Status,
			@CodigoPais [CodigoPais],
			@CodigoEmpresa [CodigoEmpresa],
			@CodigoPuesto [CodigoPuesto]

		FROM sys_Control_Sesiones AS scs
		INNER JOIN sys_usuario AS su ON su.consecutivo_usuario = scs.consecutivo_usuario
		WHERE scs.cod_sesion = @CodigoSesion
	END

END TRY
BEGIN CATCH
    SELECT 
        ERROR_NUMBER() AS ErrorNumber,
        ERROR_SEVERITY() AS ErrorSeverity,
        ERROR_STATE() as ErrorState,
        ERROR_PROCEDURE() as ErrorProcedure,
        ERROR_LINE() as ErrorLine,
        ERROR_MESSAGE() as ErrorMessage;
    IF @@TRANCOUNT > 0
        ROLLBACK TRANSACTION;
END CATCH;
IF @@TRANCOUNT > 0
    COMMIT TRANSACTION;
