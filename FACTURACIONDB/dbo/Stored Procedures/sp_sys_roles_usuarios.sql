CREATE PROCEDURE [dbo].[sp_sys_roles_usuarios]
@cod_rol INT,
@nombre_rol NVARCHAR(30),
@cod_usuario INT,
@accion NVARCHAR(30)
AS 

IF @accion = 'INSERTAR' BEGIN
	SET @cod_rol = (SELECT MAX(cod_rol) + 1 FROM sys_roles_usuarios) --INCREMENTAR EL ID DE cod_rol (nuevo ID) 
	INSERT INTO sys_roles_usuarios (cod_rol, descripcion)
	VALUES (@cod_rol, @nombre_rol)
END /* INSERTAR */

IF @accion = 'ELIMINAR' BEGIN
	UPDATE sys_roles_usuarios 
	SET activo = 0 
	WHERE cod_rol = @cod_rol
END /* ELIMINAR */

IF @accion = 'CONSULTA' BEGIN 
	SELECT cod_rol, UPPER(descripcion) AS rol
	FROM sys_roles_usuarios 
	WHERE activo = 1
	ORDER BY rol
END /* CONSULTA */

