CREATE PROC sp_ValidarUsuario ( 

@CorreoElectronico varchar(100), 
@Password varchar(500) 

)

AS 
BEGIN 
	IF (EXISTS(SELECT * FROM Usuarios WHERE CorreoElectronico = @CorreoElectronico AND Password = @Password)) 
		SELECT CodigoUser FROM Usuarios WHERE CorreoElectronico = @CorreoElectronico AND Password = @Password

		ELSE 
			SELECT '0' 

END