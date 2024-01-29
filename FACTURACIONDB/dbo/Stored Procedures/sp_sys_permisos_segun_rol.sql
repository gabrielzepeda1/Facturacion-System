CREATE PROCEDURE sp_sys_permisos_segun_rol
@cod_rol INT,
@cod_usuario INT
AS 
DELETE FROM sys_Menu_Permisos_web WHERE cod_usuario = @cod_usuario

INSERT INTO sys_Menu_Permisos_web
SELECT 
	srup.cod_menu 
	, @cod_usuario
FROM sys_roles_usuarios_permisos srup
WHERE
	srup.cod_rol = @cod_rol