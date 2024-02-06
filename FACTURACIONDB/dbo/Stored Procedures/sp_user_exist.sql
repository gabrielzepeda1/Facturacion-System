CREATE PROCEDURE [dbo].[sp_user_exist]
@usario NVARCHAR(30)
AS 
IF EXISTS(SELECT su.USUARIO FROM sys_usuario su WHERE su.usuario = @usario) BEGIN
	SELECT 'Si' AS Usuario                                                                                          	
END ELSE BEGIN
	SELECT 'No' AS Usuario
END

--SELECT * FROM sys_usuario su