CREATE proc [dbo].[sp_RegistrarUsuario](
	@CodigoRol int,
	@Password varchar(100), 
	@Nombres nvarchar(50), 
	@Apellidos nvarchar(50), 
	@CorreoElectronico nvarchar(50), 
	@Activo bit,
	@Registrado bit output,
	@FechaRegistrado datetime, 
	@FechaInactivo datetime,
	@Mensaje varchar(100) output




)
as 
BEGIN 

	IF (NOT EXISTS(SELECT * FROM USUARIO WHERE CorreoElectronico = @CorreoElectronico))
	BEGIN 
		INSERT INTO USUARIO(CodigoRol,Password, Nombres, Apellidos, CorreoElectronico, Activo, FechaRegistrado, FechaInactivo) 
		VALUES (@CodigoRol, @Password, @Nombres, @Apellidos, @CorreoElectronico, @Activo, @FechaRegistrado, @FechaInactivo)
		SET @Registrado = 1 
		SET @Mensaje = 'Usuario registrado'
		END 

		ELSE 
		BEGIN 
			SET @Registrado = 0 
			SET @Mensaje = 'Usuario ya existe'

		END 
	END 